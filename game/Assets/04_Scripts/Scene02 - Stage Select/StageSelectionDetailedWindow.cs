using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;


public class StageSelectionDetailedWindow : SerializedMonoBehaviour
{
    #region StageItemDetailedPopup
    [FoldoutGroup("StageItemDetailedPopup")]
    [SerializeField] PlayMakerFSM WindowFsm;
    [FoldoutGroup("StageItemDetailedPopup/Main Elements")]
    [SerializeField] SelfLeaderboard selfLeaderboard;
    [FoldoutGroup("StageItemDetailedPopup/Main Elements")]
    [SerializeField] StageIntroductionPanel stageIntroductionPanel;
    [FoldoutGroup("StageItemDetailedPopup/Button")]
    [SerializeField] Button modeButtonTutorial;
    [FoldoutGroup("StageItemDetailedPopup/Button")]
    [SerializeField] PlayMakerFSM modeButtonTutorialUpdateFsm;
    [FoldoutGroup("StageItemDetailedPopup/Button")]
    [SerializeField] Button modeButtonPractice;
    [FoldoutGroup("StageItemDetailedPopup/Button")]
    [SerializeField] PlayMakerFSM modeButtonPracticeInitialFsm;
    [FoldoutGroup("StageItemDetailedPopup/Button")]
    [SerializeField] PlayMakerFSM modeButtonPracticeUpdateFsm;
    [FoldoutGroup("StageItemDetailedPopup/Button")]
    [SerializeField] Button GoToPlayGameSceneButton;
    #endregion

    #region Data
    [FoldoutGroup("Data")]
    [SerializeField] EventTrackerTrigger eventTrackerTrigger;
    [FoldoutGroup("Data")]
    [SerializeField] string clickedStageName;
    [FoldoutGroup("Data")]
    [SerializeField] string selectedModeType;
    [FoldoutGroup("Data")]
    [SerializeField] Dictionary<string, StageData> UnlockStageDict = new();
    #endregion

    private void Start()
    {
        modeButtonTutorial.onClick.AddListener(() => SwitchModeAction("Tutorial"));
        modeButtonPractice.onClick.AddListener(() => SwitchModeAction("Practice"));
        GoToPlayGameSceneButton.onClick.AddListener(() => GoToPlayGameScene());
    }

    #region Initialize
    public void InitializeStageItem(Transform stageItemButton, StageData stageItem, bool isUnlock)
    {
        //Update when type is Tutorial
        if (stageItemButton != null)
        {
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(stageItemButton.gameObject, "Update Content");
            LeanLocalizedText i18nText = stageItemButton.Find("DetailedPopup/Content").GetComponent<LeanLocalizedText>();
            string trimKey = stageItem.stageName.Split("(Tutorial)")[0].Trim();
            i18nText.TranslationName = $"StageItemButton/DetailedPopup/{trimKey}";

            fsm.FsmVariables.GetFsmBool("isStageUnlock").Value = isUnlock;
            fsm.enabled = true;
        }

        if (isUnlock)
        {
            //Only Tutorial has Button
            if (stageItem.stageName.Contains("(Tutorial)"))
            {
                stageItemButton.GetComponent<StageItemButton>().Initialize(this, stageItem.stageName);
            }
            UnlockStageDict.Add(stageItem.stageName, stageItem);
        }
    }
    #endregion

    public void OpenWindow(string clickStageName)
    {
        clickedStageName = clickStageName.Split("(Tutorial)")[0].Trim();
        WindowFsm.SendEvent("Common/Window/Show Window");

        stageIntroductionPanel.UpdateContentInStageSelect(clickedStageName);

        //Click StageItemButton -> unlock/lock button.
        SetModeButtonInteraction();

        //Default click Tutorial Mode Button
        SwitchModeAction("Tutorial");
    }

    void SetModeButtonInteraction()
    {
        string practiceKey = clickedStageName + " (Practice)";
        bool enable = (UnlockStageDict.ContainsKey(practiceKey));
        modeButtonPracticeInitialFsm.FsmVariables.GetFsmBool("isStageUnlock").Value = enable;
        modeButtonPracticeInitialFsm.enabled = true;
    }

    public void SwitchModeAction(string modeType)
    {
        switch (modeType) {
            case "Tutorial":
                if (modeButtonPracticeInitialFsm.FsmVariables.GetFsmBool("isStageUnlock").Value == true)
                {
                    modeButtonPracticeUpdateFsm.FsmVariables.GetFsmString("runType").Value = "unselect";
                    modeButtonPracticeUpdateFsm.enabled = true;
                }
                modeButtonTutorialUpdateFsm.FsmVariables.GetFsmString("runType").Value = "select";
                modeButtonTutorialUpdateFsm.enabled = true;

                break;
            case "Practice":
                modeButtonPracticeUpdateFsm.FsmVariables.GetFsmString("runType").Value = "select";
                modeButtonPracticeUpdateFsm.enabled = true;
                modeButtonTutorialUpdateFsm.FsmVariables.GetFsmString("runType").Value = "unselect";
                modeButtonTutorialUpdateFsm.enabled = true;
               // UpdateDetailedContent(,type);
                break;
            default:
                Debug.LogError("StageSelectionDetailedWindow SwitchModeAction Error");
                break;
        }
        selectedModeType = modeType;
        UpdateSelfLeaderBoardContent(modeType);
    }

    void UpdateSelfLeaderBoardContent(string modeType)
    {
        string key = clickedStageName + " (" + modeType + ")";
        StageData selectedStageItem = UnlockStageDict[key];

        //Update SelfLeaderBoardContent
        selfLeaderboard.SetSelectedStageData(selectedStageItem);
        selfLeaderboard.UpdateSelfLeaderBoardContent();
    }

    void GoToPlayGameScene()
    {
        GoToPlayGameSceneButton.interactable = false;
        string key = clickedStageName + " (" + selectedModeType + ")";
        eventTrackerTrigger.SendEvent("Start Stage", key);
        SaveManager.Instance.GoToPlayGameScene(key);
    }

    public string GetSelectedModeType()
    {
        return selectedModeType;
    }

    public string GetClickedStageName()
    {
        return clickedStageName;
    }
}
