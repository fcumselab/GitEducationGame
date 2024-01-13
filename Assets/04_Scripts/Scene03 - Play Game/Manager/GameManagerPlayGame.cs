using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;

public class GameManagerPlayGame : SerializedMonoBehaviour
{
    [FoldoutGroup("Managers")]
    [SerializeField] StageManagerController stageManagerController;
    [FoldoutGroup("Managers")]
    [SerializeField] QuestTrackerManager questTrackerManager;
    [FoldoutGroup("Managers")]
    [SerializeField] DialogueSystemManager dialogueSystemManager;
    [FoldoutGroup("Managers")]
    [SerializeField] DialogueSystemFeatureManager dialogueSystemFeatureManager;

    [FoldoutGroup("Windows")]
    [SerializeField] PauseMenuPopup pauseMenuPopup;

    [FoldoutGroup("Prefabs")]
    [SerializeField] GameObject gameManualWindowPrefab;
    GameManual gameManualWindow;

    [FoldoutGroup("Datas")]
    [SerializeField] StageData stageData;

    public void InitializeScene(string lastSceneName)
    {
        stageData = SaveManager.Instance.GetPlayingStageData();
        stageManagerController.Initialize(stageData.stageName);
        questTrackerManager.Initialize(stageData.stageName);

        dialogueSystemManager.InitializeReference(stageData.stageName);
        dialogueSystemFeatureManager.RegisterFunction();

        gameManualWindow = Instantiate(gameManualWindowPrefab).GetComponent<GameManual>();
        gameManualWindow.InitializeGameManualData();

        pauseMenuPopup.InitializePauseMenuPopupContent(stageData);
    }

    public void GoToPlayGameScene()
    {
        dialogueSystemFeatureManager.UnregisterFunction();
        SaveManager.Instance.GoToPlayGameScene("");
    }

    public void GoToStageSelectScene()
    {
        dialogueSystemFeatureManager.UnregisterFunction();
        SaveManager.Instance.GoToStageSelectScene();
    }

}
