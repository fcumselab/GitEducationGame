using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using HutongGames.PlayMaker;

public class CommandHistoryTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject TooltipLight;
    [SerializeField] PlayMakerFSM Content;
    PlayMakerFSM ToolTipControlFSM;

    private void Start()
    {
        TooltipLight = GameObject.Find("Tooltip Light");
        ToolTipControlFSM = MyPlayMakerScriptHelper.GetFsmByName(TooltipLight, "ToolTip Control");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string tooltipMessage = Content.FsmVariables.GetFsmString("tooltipMessage").ToString();
        ToolTipControlFSM.FsmVariables.GetFsmString("tooltipMessage").Value = tooltipMessage;
        ToolTipControlFSM.SendEvent("Tooltip/Show Tooltip by Script");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipControlFSM.SendEvent("Tooltip/Close Tooltip");
    }
}
