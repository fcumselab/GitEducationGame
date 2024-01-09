using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PauseMenuPopup : SerializedMonoBehaviour
{

    [FoldoutGroup("PauseMenuPopup")]
    [SerializeField] PlayMakerFSM WindowFsm;
    [FoldoutGroup("PauseMenuPopup/Button")]
    [SerializeField] Button pauseMenuButton;

    [FoldoutGroup("Popup Panels")]
    [SerializeField] SelfLeaderboard selfLeaderboard;
    [FoldoutGroup("Popup Panels")]
    [SerializeField] StageIntroductionPanel stageIntroductionPanel;

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
    }
}
