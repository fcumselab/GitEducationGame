using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;

public class GameManagerPlayGame : SerializedMonoBehaviour
{
    [SerializeField] PauseMenuPopup pauseMenuPopup;
    [SerializeField] StageData stageData;
    public void InitializeScene(string lastSceneName)
    {
        stageData = SaveManager.Instance.GetPlayingStageData();

        pauseMenuPopup.InitializePauseMenuPopupContent(stageData);
    }
}
