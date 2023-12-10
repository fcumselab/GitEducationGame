using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectGameManager : MonoBehaviour
{
    public void InitializeStageCategoryAndStageItem(GameObject SelectStageCategory, GameObject StageItemsContent)
    {
        SelectStageCategory.gameObject.SetActive(true);
        List<StageData> stageData = SaveManager.Instance.GetStageDataListFromPlayerData();

        int totalStar = 0;
        int totalStage = 0;
        int totalClearStage = 0;
        PlayMakerFSM fsm;

        string currentStageType = "Basic"; //Basic -> Branch -> Remote
        Transform targetStageCategoryButton = SelectStageCategory.transform.Find("StageCategoryButton - Basic");
        Transform targetStageItems = StageItemsContent.transform.Find("StageItems - Basic");
        targetStageCategoryButton.gameObject.SetActive(true);
        targetStageItems.gameObject.SetActive(true);
        foreach (StageData stage in stageData)
        {
            
            if(stage.stageType != currentStageType)
            {
                //Give to Current Category
                fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStageCategoryButton.gameObject, "Update Content");
                fsm.FsmVariables.FindFsmInt("totalStar").Value = totalStar;
                fsm.FsmVariables.FindFsmInt("totalStage").Value = totalStage;
                fsm.FsmVariables.FindFsmInt("totalClearStage").Value = totalClearStage;
                if(currentStageType == "Basic") fsm.FsmVariables.FindFsmBool("isStageCategoryUnlock").Value = true;
                fsm.enabled = true;

                //Next Category
                currentStageType = stage.stageType;

                targetStageCategoryButton = SelectStageCategory.transform.Find("StageCategoryButton - " + currentStageType);
                targetStageItems = StageItemsContent.transform.Find("StageItems - " + currentStageType);
                targetStageCategoryButton.gameObject.SetActive(true);
                targetStageItems.gameObject.SetActive(true);

                fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStageCategoryButton.gameObject, "Update Content");
                fsm.FsmVariables.FindFsmBool("isStageCategoryUnlock").Value = (totalStage == totalClearStage || totalClearStage == 1);

                totalStage = 0;
                totalClearStage = 0;
                totalStar = 0;
            }

            //for StageCategory
            totalStage++;
            if (stage.stageClearTimes > 0)
            {
                totalClearStage++;
                totalStar += stage.stageLeaderboardData[0].playerStar;
            }

            //Give StageItem Value
            if (targetStageItems.transform.childCount >= totalStage)
            {
                Transform targetStage = targetStageItems.transform.GetChild(totalStage - 1);

                fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStage.gameObject, "Update Content");
                fsm.FsmVariables.FindFsmString("stageName").Value = stage.stageName;
                fsm.FsmVariables.FindFsmBool("isStageUnlock").Value = stage.isStageUnlock;
                fsm.FsmVariables.FindFsmInt("stageClearTimes").Value = stage.stageClearTimes;

                if (stage.isStageUnlock)
                {
                    for (int i = 0; i < stage.stageLeaderboardData.Count; i++)
                    {
                        fsm.FsmVariables.FindFsmArray("playerNameList").Set(i, stage.stageLeaderboardData[i].playerName);
                        fsm.FsmVariables.FindFsmArray("playerScoreList").Set(i, stage.stageLeaderboardData[i].playerScore);
                        fsm.FsmVariables.FindFsmArray("playerStarList").Set(i, stage.stageLeaderboardData[i].playerStar);
                        fsm.FsmVariables.FindFsmArray("playerClearTimeList").Set(i, stage.stageLeaderboardData[i].playerClearTime);
                    }
                }
                fsm.enabled = true;
            }
            else
            {
                Debug.Log("Doesnt have enough StageItem, Please add: " + currentStageType);
            }
            
        }

        //Last one -> Remote
        targetStageCategoryButton = SelectStageCategory.transform.Find("StageCategoryButton - " + currentStageType);
        fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStageCategoryButton.gameObject, "Update Content");
        fsm.FsmVariables.FindFsmInt("totalStar").Value = totalStar;
        fsm.FsmVariables.FindFsmInt("totalStage").Value = totalStage;
        fsm.FsmVariables.FindFsmInt("totalClearStage").Value = totalClearStage;

        fsm.enabled = true;
            
        SelectStageCategory.gameObject.SetActive(false);

    }
}
