using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PauseMenuPopup : SerializedMonoBehaviour
{
    [FoldoutGroup("Managers")]
    [SerializeField] GameManagerPlayGame gameManager;

    [FoldoutGroup("PauseMenuPopup")]
    [SerializeField] PlayMakerFSM WindowFsm;
    [FoldoutGroup("PauseMenuPopup/Button")]
    [SerializeField] Button pauseMenuButton;

    [FoldoutGroup("Popup Panels")]
    [SerializeField] SelfLeaderboard selfLeaderboard;
    [FoldoutGroup("Popup Panels")]
    [SerializeField] StageIntroductionPanel stageIntroductionPanel;

    [FoldoutGroup("Button")]
    [SerializeField] Button swtichSceneButton;
    [FoldoutGroup("Button")]
    [SerializeField] Button replayButton;
    [FoldoutGroup("Button")]
    [SerializeField] Button backToStageSelectionButton;

    [FoldoutGroup("Web Connection")]
    [SerializeField] EventTrackerTrigger eventTrackerTrigger;

    [SerializeField] string switchSceneKey;
    public void InitializePauseMenuPopupContent(StageData stageData)
    {
        selfLeaderboard.SetSelectedStageData(stageData);
        selfLeaderboard.UpdateSelfLeaderBoardContent();
        stageIntroductionPanel.UpdateContentInPlayGameScene(stageData.stageName);
    }

    public void OpenWindow()
    {
        WindowFsm.SendEvent("Common/Window/Show Window");
    }

    void Start()
    {
        pauseMenuButton.onClick.AddListener(() => OpenWindow());
        swtichSceneButton.onClick.AddListener(() => ClickSwtichSceneButtonAction());
        replayButton.onClick.AddListener(() => SetSwitchsceneTarget("replay"));
        backToStageSelectionButton.onClick.AddListener(() => SetSwitchsceneTarget("stageSelection"));
    }

    void ClickSwtichSceneButtonAction()
    {
        swtichSceneButton.interactable = false;
        switch (switchSceneKey)
        {
            case "replay":
                eventTrackerTrigger.SendEvent("Restart Stage(Not Clear)", $"Stage: {SaveManager.Instance.GetSelectedStageName()}");
                gameManager.GoToPlayGameScene();
                break;
            case "stageSelection":
                eventTrackerTrigger.SendEvent("Back To Stage Select(Not Clear)", $"Stage: {SaveManager.Instance.GetSelectedStageName()}");
                gameManager.GoToStageSelectScene();
                break;
            default:
                Debug.LogError("Not found Correct switchSceneKey");
                break;
        }
    }

    void SetSwitchsceneTarget(string key)
    {
        switchSceneKey = key;
    }
}
