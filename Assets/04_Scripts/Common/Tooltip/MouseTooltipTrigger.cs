using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;
using UnityEngine.EventSystems;

public class MouseTooltipTrigger : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum TriggerMode { Hover, ClickButton, HoverWithClick, Custom }
    public TriggerMode triggerMode;
    public string tooltipText;
    public bool isI18nKey;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");

        if (triggerMode == TriggerMode.Hover || triggerMode == TriggerMode.HoverWithClick)
        {
            // 显示Tooltip
            MouseTooltipManager.Instance.ShowTooltip(tooltipText, isI18nKey);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Leave");
        MouseTooltipManager.Instance.HideTooltip();
    }

    public void ClickButtonAction(string targetTooltipText, bool targetIsI18nKey)
    {
        MouseTooltipManager.Instance.ShowTooltip(targetTooltipText, targetIsI18nKey);
    }
}

