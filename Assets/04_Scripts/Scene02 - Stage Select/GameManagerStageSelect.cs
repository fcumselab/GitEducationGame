using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManagerStageSelect : SerializedMonoBehaviour
{
    [FoldoutGroup("ButtonGroup")]
    [SerializeField] GameObject SelectStageCategory;
    [FoldoutGroup("ButtonGroup")]
    [SerializeField] GameObject SelectStageItemPanel;
    [FoldoutGroup("ButtonGroup/StageItemsContent")]
    [SerializeField] GameObject StageItemsBasic;
    [FoldoutGroup("ButtonGroup/StageItemsContent")]
    [SerializeField] GameObject StageItemsBranch;
    [FoldoutGroup("ButtonGroup/StageItemsContent")]
    [SerializeField] GameObject StageItemsRemote;

    [FoldoutGroup("Main System")]
    [SerializeField] PlayMakerFSM LoadingBG;
    [FoldoutGroup("Main System/StageSelectionDetailed")]
    [SerializeField] StageSelectionDetailedWindow stageSelectionDetailedWindow;
    [FoldoutGroup("Main System/PlayerGameRecords")]
    [SerializeField] PlayerGameRecordsWindow playerGameRecordsWindow;
    [FoldoutGroup("Main System/SelectStageCategoryPopup")]
    [SerializeField] PlayMakerFSM categoryPopupUpdateContentFsm;

    [FoldoutGroup("Prefabs")]
    [SerializeField] GameObject gameManualWindowPrefab;

    [FoldoutGroup("Reference")]
    [SerializeField] SaveManager saveManager;
    [FoldoutGroup("Reference")]
    [SerializeField] GameManual gameManualWindow;

    #region Initialize
    #region Category and Items
    public void InitializeScene(string lastSceneName, string selectedStageName)
    {
        SelectStageItemPanel.SetActive(true);
        SelectStageCategory.SetActive(true);

        saveManager = GameObject.Find("Save Manager (Main)").GetComponent<SaveManager>();
        playerGameRecordsWindow.InitializeGameProgressData(saveManager);
        InitializeStageCategoryAndStageItem();

        gameManualWindow = Instantiate(gameManualWindowPrefab).GetComponent<GameManual>();
        gameManualWindow.InitializeGameManualData();

        switch (lastSceneName)
        {
            case "Play Game":
                SelectStageCategory.SetActive(false);
                ShowLastStageItemButton();
                break;
            case "Title Screen":
                SelectStageItemPanel.SetActive(false);
                break;
        }

        LoadingBG.SendEvent("Common/Window/Hide Window");
    }

    void InitializeStageCategory(string type, List<StageData> stageData, GameObject targetStageItems)
    {
        int totalStar = 0;
        int totalStage = 0;
        int totalClearStage = 0;

        Transform targetStageCategoryButton = SelectStageCategory.transform.Find($"StageCategoryButton - {type}");

        //Stage01 (Tutorial) -> Stage01 (Practice) -> Stage02 (Tutorial) -> Stage02 (Practice)....
        List<StageData> foundTypeData = stageData.FindAll((stageItem) => stageItem.stageType == type);

        totalStage = foundTypeData.Count;


        int buttonItemIndex = 0;
        foreach (StageData stageItem in foundTypeData)
        {
            //Debug.Log("it is turn for: " + stageItem.stageName);
            if (stageItem.stageName.Contains("(Tutorial)"))
            {
                Transform stageItemButton = targetStageItems.transform.GetChild(buttonItemIndex);
                //Debug.Log("index: " + buttonItemIndex + "-> Button: " + stageItemButton);

                if (stageItem.isStageUnlock)
                {
                    if (stageItem.stageClearTimes > 0)
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
        InitializeStageCategory("Basic", stageData, StageItemsBasic);
        InitializeStageCategory("Branch", stageData, StageItemsBranch);
        InitializeStageCategory("Remote", stageData, StageItemsRemote);
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
    #endregion

    #region Display

    void ShowLastStageItemButton()
    {
        StageItemsBasic.SetActive(false);
        StageItemsBranch.SetActive(false);
        StageItemsRemote.SetActive(false);

        switch (saveManager.GetPlayingStageData().stageType)
        {
            case "Basic":
                StageItemsBasic.SetActive(true);
                break;
            case "Branch":
                StageItemsBranch.SetActive(true);
                break;
            case "Remote":
                StageItemsRemote.SetActive(true);
                break;
        }
        categoryPopupUpdateContentFsm.enabled = true;
    }

    #endregion

    #endregion
}
