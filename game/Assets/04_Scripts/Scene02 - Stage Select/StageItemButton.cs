using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class StageItemButton : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] StageItemsPanel stageItemsPanel;

    [SerializeField] Button itemButton;
    [SerializeField] PlayMakerFSM detailPopupTooltip;

    //Called StageSelectionDetailedWindow only is unlock
    public void Initialize(StageSelectionDetailedWindow script, string stageName)
    {
        stageItemsPanel = transform.parent.GetComponent<StageItemsPanel>();
        stageItemsPanel.AddNewStageButtonList(itemButton.gameObject);
        itemButton.onClick.AddListener(() => ClickButton(script, stageName));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detailPopupTooltip.SendEvent("Common/Button/PointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailPopupTooltip.SendEvent("Common/Button/PointerLeave");
    }

    void ClickButton(StageSelectionDetailedWindow script, string stageName)
    {
        script.OpenWindow(stageName);
        stageItemsPanel.ScrollRectLockTargetContent(itemButton.gameObject);
    }
}