using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectGameManager : MonoBehaviour
{
    public void InitializeStageCategoryAndStageItem(GameObject SelectStageCategory, GameObject StageItemsContent)
    {
        List<StageData> stageData = SaveManager.Instance.GetStageDataListFromPlayerData();

        int totalStar = 0;
        int totalStage = 0;
        int totalClearStage = 0;
        PlayMakerFSM fsm;

        string currentStageType = "Basic"; //Basic -> Branch -> Remote
        Transform targetStageCategoryButton = SelectStageCategory.transform.Find("StageCategoryButton - Basic");
        Transform targetStageItems = StageItemsContent.transform.Find("StageItems - Basic");

        foreach (StageData stage in stageData)
        {
            if(stage.stageType != currentStageType)
            {
                targetStageCategoryButton = SelectStageCategory.transform.Find("StageCategoryButton - " + currentStageType);
                targetStageItems = StageItemsContent.transform.Find("StageItems - " + currentStageType);
                fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStageCategoryButton.gameObject, "Update Content");
                fsm.FsmVariables.FindFsmInt("totalStar").Value = totalStar;
                fsm.FsmVariables.FindFsmInt("totalStage").Value = totalStage;
                fsm.FsmVariables.FindFsmInt("totalClearStage").Value = totalClearStage;
                fsm.enabled = true;

                totalStage = 0;
                totalClearStage = 0;
                totalStar = 0;
                currentStageType = stage.stageType;
            }

            //for StageCategory
            totalStage++;
            if (stage.stageClearTimes > 0)
            {
                totalClearStage++;
                totalStar += stage.selfStageLeaderboardData[0].playerStar;
            }

            //Give StageItem Value
            Transform targetStage = targetStageItems.transform.GetChild(totalStage);
            if(targetStage != null)
            {
                fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStage.gameObject, "Content");
                fsm.FsmVariables.FindFsmString("stageName").Value = stage.stageName;
                fsm.FsmVariables.FindFsmBool("isStageUnlock").Value = stage.isStageUnlock;
                if (stage.isStageUnlock)
                {
                    for (int i = 0; i < stage.selfStageLeaderboardData.Count; i++)
                    {
                        fsm.FsmVariables.FindFsmArray("playerNameList").Set(i, stage.selfStageLeaderboardData[i].playerName);
                        fsm.FsmVariables.FindFsmArray("playerScoreList").Set(i, stage.selfStageLeaderboardData[i].playerScore);
                        fsm.FsmVariables.FindFsmArray("playerStarList").Set(i, stage.selfStageLeaderboardData[i].playerStar);
                        fsm.FsmVariables.FindFsmArray("playerClearTimeList").Set(i, stage.selfStageLeaderboardData[i].playerClearTime);
                    }
                }
            }
            else
            {
                Debug.Log("缺少足夠的 StageItem, 請複製到 " + currentStageType);
            }
            
        }

        //Last one -> Remote
        targetStageCategoryButton = SelectStageCategory.transform.Find("StageCategoryButton - " + currentStageType);
        fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStageCategoryButton.gameObject, "Update Content");
        fsm.FsmVariables.FindFsmInt("totalStar").Value = totalStar;
        fsm.FsmVariables.FindFsmInt("totalStage").Value = totalStage;
        fsm.FsmVariables.FindFsmInt("totalClearStage").Value = totalClearStage;
        fsm.enabled = true;
    }
}
