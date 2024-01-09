using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;

public class StageIntroductionPanel : SerializedMonoBehaviour
{
    [SerializeField] LeanLocalizedText StageTitleText;
    [SerializeField] LeanLocalizedText StageOverviewContent;
    [SerializeField] LeanLocalizedText StageObjectiveContent;
    
    public void UpdateContentInStageSelect(string stageKey)
    {
        StageTitleText.TranslationName = $"StageItemButton/DetailedPopup/{stageKey}";
        StageOverviewContent.TranslationName = $"StageIntroductionPanel/StageOverview/{stageKey}";
        StageObjectiveContent.TranslationName = $"StageIntroductionPanel/StageObjective/{stageKey}";
    }

    public void UpdateContentInPlayGameScene(string stageKey)
    {
        string trimKey = stageKey.Split("(")[0].Trim();
        StageTitleText.TranslationName = "StageTitle";
        StageOverviewContent.TranslationName = $"StageIntroductionPanel/StageOverview/{trimKey}";
        StageObjectiveContent.TranslationName = $"StageIntroductionPanel/StageObjective/{trimKey}";
    }
}
