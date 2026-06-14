using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;

public class StageIntroductionPanel : SerializedMonoBehaviour
{
    [FoldoutGroup("Common")]
    [SerializeField] LeanLocalizedText StageTitleText;
    [FoldoutGroup("Common")]
    [SerializeField] LeanLocalizedText StageOverviewContent;
    [FoldoutGroup("Common")]
    [SerializeField] LeanLocalizedText StageObjectiveContent;

    [FoldoutGroup("Play Game Scene")]
    [SerializeField] GameObject GameModeImagePanel;
    [FoldoutGroup("Play Game Scene")]
    [SerializeField] GameObject ImagePanelTutorial;
    [FoldoutGroup("Play Game Scene")]
    [SerializeField] GameObject ImagePanelPractice;

    public void UpdateContentInStageSelect(string stageKey)
    {
        StageTitleText.TranslationName = $"StageItemButton/DetailedPopup/{stageKey}";
        StageOverviewContent.TranslationName = $"StageIntroductionPanel/StageOverview/{stageKey}";
        StageObjectiveContent.TranslationName = $"StageIntroductionPanel/StageObjective/{stageKey}";
    }

    public void UpdateContentInPlayGameScene(string stageKey)
    {
        GameModeImagePanel.SetActive(true);
        if (stageKey.Contains("Tutorial"))
        {
            ImagePanelTutorial.SetActive(true);
        }
        else if (stageKey.Contains("Practice"))
        {
            ImagePanelPractice.SetActive(true);
        }

        string trimKey = stageKey.Split("(")[0].Trim();
        StageTitleText.TranslationName = "StageTitle";
        StageOverviewContent.TranslationName = $"StageIntroductionPanel/StageOverview/{trimKey}";
        StageObjectiveContent.TranslationName = $"StageIntroductionPanel/StageObjective/{trimKey}";
    }
}
