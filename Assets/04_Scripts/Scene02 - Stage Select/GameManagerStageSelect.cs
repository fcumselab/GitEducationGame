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

    [FoldoutGroup("GameManualWindow")]
    [SerializeField] GameManual gameManualWindow;

    [FoldoutGroup("Prefabs")]
    [SerializeField] GameObject gameManualWindowPrefab;

    [Header("Reference")]
    [SerializeField] SaveManager saveManager;

    public void InitializeScene(string lastSceneName)
    {
        saveManager = GameObject.Find("Save Manager (Main)").GetComponent<SaveManager>();
        playerGameRecordsWindow.InitializeGameProgressData(saveManager);
        InitializeStageCategoryAndStageItem();

        gameManualWindow = Instantiate(gameManualWindowPrefab).GetComponent<GameManual>();
        gameManualWindow.InitializeGameManualData();

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

        
        int buttonItemIndex = 0;
        foreach (StageData stageItem in foundTypeData)
        {
            //Debug.Log("it is turn for: " + stageItem.stageName);
            if (stageItem.stageName.Contains("(Tutorial)"))
            {
                Transform stageItemButton = targetStageItems.GetChild(buttonItemIndex);
                //Debug.Log("index: " + buttonItemIndex + "-> Button: " + stageItemButton);

                if (stageItem.isStageUnlock)
                {
                    if(stageItem.stageClearTimes > 0)
                    {
                        totalStar += stageItem.stageLeaderboardData[0].playerStar;
                        totalClearStage++;
                    }
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

        PlayMakerFSM categoryFsm = MyPlayMakerScriptHelper.GetFsmByName(targetStageCategoryButton.gameObject, "Update Content");

        categoryFsm.FsmVariables.FindFsmInt("totalStar").Value = totalStar;
        categoryFsm.FsmVariables.FindFsmInt("totalStage").Value = totalStage;
        categoryFsm.FsmVariables.FindFsmInt("totalClearStage").Value = totalClearStage;
        UpdateStageCategoryButtonStatus(categoryFsm, type, stageData);
        categoryFsm.enabled = true;
    }
    
    void InitializeStageCategoryAndStageItem()
    {
        List<StageData> stageData = SaveManager.Instance.GetStageDataListFromPlayerData();
        InitializeStageCategory("Basic", stageData);
        InitializeStageCategory("Branch", stageData);
        InitializeStageCategory("Remote", stageData);
    }

    void UpdateStageCategoryButtonStatus(PlayMakerFSM categoryFsm, string categoryType, List<StageData> stageData)
    {
        bool isUnlock = false;
        switch (categoryType)
        {
            case "Basic":
                isUnlock = (stageData.FindIndex((item) => item.stageName == "Game Introduction (Tutorial)" && item.isStageUnlock == true) != -1);
                break;
            case "Branch":
                isUnlock = (stageData.FindIndex((item) => item.stageName == "Git Branching Basics (Tutorial)" && item.isStageUnlock == true) != -1);
                break;
            case "Remote":
                isUnlock = (stageData.FindIndex((item) => item.stageName == "Create Remote Repository (Tutorial)" && item.isStageUnlock == true) != -1);
                break;
        }
        categoryFsm.FsmVariables.FindFsmBool("isStageCategoryUnlock").Value = isUnlock;
    }
}
