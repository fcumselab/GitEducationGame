using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManagerStageSelect : SerializedMonoBehaviour
{
    [FoldoutGroup("ButtonGroup")]
    [SerializeField] GameObject SelectStageCategory;
    [FoldoutGroup("ButtonGroup")]
    [SerializeField] GameObject StageItemsContent;
    [FoldoutGroup("ButtonGroup")]
    [SerializeField] GameObject SelectStageItemPanel;

    [FoldoutGroup("StageItemDetailedPopup")]
    [SerializeField] StageSelectionDetailedWindow stageSelectionDetailedWindow;

    [FoldoutGroup("PlayerGameRecordsWindow")]
    [SerializeField] PlayerGameRecordsWindow playerGameRecordsWindow;

    [Header("Reference")]
    [SerializeField] SaveManager saveManager;

    public void InitializeScene(string lastSceneName)
    {
        saveManager = GameObject.Find("Save Manager (Main)").GetComponent<SaveManager>();
        playerGameRecordsWindow.InitializeGameProgressData(saveManager);
        InitializeStageCategoryAndStageItem();
        switch (lastSceneName)
        {
            case "Play Game":
                break;
            case "Title Screen":
                SelectStageItemPanel.SetActive(false);
                SelectStageCategory.SetActive(true);
                break;
        }
    }

    void InitializeStageCategory(string type, List<StageData> stageData)
    {
        int totalStar = 0;
        int totalStage = 0;
        int totalClearStage = 0;

        Transform targetStageCategoryButton = SelectStageCategory.transform.Find($"StageCategoryButton - {type}");
        Transform targetStageItems = StageItemsContent.transform.Find($"StageItems - {type}");

        //Stage01 (Tutorial) -> Stage01 (Practice) -> Stage02 (Tutorial) -> Stage02 (Practice)....
        List<StageData> foundTypeData = stageData.FindAll((stageItem) => stageItem.stageType == type);
        
        totalStage = foundTypeData.Count;

        PlayMakerFSM fsm;
        int buttonItemIndex = 0;
        foreach (StageData stageItem in foundTypeData)
        {
            Debug.Log("it is turn for: " + stageItem.stageName);
            if (stageItem.stageName.Contains("(Tutorial)"))
            {
                Transform stageItemButton = targetStageItems.GetChild(buttonItemIndex);
                //Debug.Log("index: " + buttonItemIndex + "-> Button: " + stageItemButton);

                if (stageItem.isStageUnlock)
                {
                    totalStar += stageItem.stageLeaderboardData[0].playerStar;
                    totalClearStage++;
                    stageSelectionDetailedWindow.InitializeStageItem(stageItemButton, stageItem, true);
                }
                else
                {
                    stageSelectionDetailedWindow.InitializeStageItem(stageItemButton, stageItem, false);
                }
                buttonItemIndex++;
            }
            else //Practice
            {
                if (stageItem.isStageUnlock)
                {
                    totalStar += stageItem.stageLeaderboardData[0].playerStar;
                    totalClearStage++;
                    stageSelectionDetailedWindow.InitializeStageItem(null, stageItem, true);
                }
            }
        }

        fsm = MyPlayMakerScriptHelper.GetFsmByName(targetStageCategoryButton.gameObject, "Update Content");
        fsm.FsmVariables.FindFsmInt("totalStar").Value = totalStar;
        fsm.FsmVariables.FindFsmInt("totalStage").Value = totalStage;
        fsm.FsmVariables.FindFsmInt("totalClearStage").Value = totalClearStage;
        if (type == "Basic") fsm.FsmVariables.FindFsmBool("isStageCategoryUnlock").Value = true;
        fsm.enabled = true;
    }
    
    void InitializeStageCategoryAndStageItem()
    {
        Debug.Log("Start InitializeStageCategoryAndStageItem...");
        List<StageData> stageData = SaveManager.Instance.GetStageDataListFromPlayerData();
        InitializeStageCategory("Basic", stageData);
        InitializeStageCategory("Branch", stageData);
        InitializeStageCategory("Remote", stageData);
    }
}
