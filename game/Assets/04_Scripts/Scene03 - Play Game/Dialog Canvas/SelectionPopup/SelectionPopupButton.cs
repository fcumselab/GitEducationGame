using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;

public class SelectionPopupButton : SerializedMonoBehaviour
{

    [Header("Parent")]
    [SerializeField] SelectionPopup selectionPopup;
    bool isCorrect;
    string textMessage;
    string tooltipMessage;

    [FoldoutGroup("Children")]
    [SerializeField] LeanLocalizedText i18nText;
    [FoldoutGroup("Children")]
    [SerializeField] Text ShowText;
    [FoldoutGroup("Children")]
    [SerializeField] GameObject IconPanel;
    [FoldoutGroup("Children")]
    [SerializeField] GameObject XIcon;
    [FoldoutGroup("Children")]
    [SerializeField] GameObject VIcon;
    [FoldoutGroup("Children")]
    [SerializeField] Button button;
    [FoldoutGroup("Children")]
    [SerializeField] MouseTooltipTrigger mouseTooltipTrigger;

    public void ClickButton()
    {
        if (isCorrect)
        {
            selectionPopup.ShowResult(true, ShowText, gameObject);
        }
        else
        {
            ShowResult(true);
            selectionPopup.ShowResult(false);
        }
    }

    public SelectionPopupButton SetValue(SelectionPopup script, bool isCorrect, string content, string reason)
    {
        ResetStatus();
        textMessage = content;
        tooltipMessage = reason;

        selectionPopup = script;

        this.isCorrect = isCorrect;
        i18nText.TranslationName = textMessage;
        mouseTooltipTrigger.tooltipText = tooltipMessage;

        return this;
    }

    public void ShowResult(bool isClick)
    {
        IconPanel.SetActive(true);

        if (!isCorrect)
        {
            XIcon.SetActive(true);
            VIcon.SetActive(false);
        }
        else
        {
            XIcon.SetActive(false);
            VIcon.SetActive(true);
        }

        if (isClick)
        {
            mouseTooltipTrigger.ClickButtonAction(tooltipMessage, true);
        }

        mouseTooltipTrigger.triggerMode = MouseTooltipTrigger.TriggerMode.Hover;
        button.interactable = false;
    }

    public void ResetStatus()
    {
        mouseTooltipTrigger.triggerMode = MouseTooltipTrigger.TriggerMode.ClickButton;
        button.interactable = true;
        IconPanel.SetActive(false);
    }

    public string GetCorrectText()
    {
        return ShowText.text;
    }
}
