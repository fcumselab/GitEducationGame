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
        if(TooltipLight == null)
        {
            Debug.Log("Please reference TooltipLight!");
        }
        else
        {
            ToolTipControlFSM = MyPlayMakerScriptHelper.GetFsmByName(TooltipLight, "ToolTip Control");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        string tooltipMessage = Content.FsmVariables.GetFsmString("tooltipMessage").ToString();
        ToolTipControlFSM.FsmVariables.GetFsmString("tooltipMessage").Value = tooltipMessage;
        ToolTipControlFSM.SendEvent("Tooltip/Show Tooltip by Script");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");

        ToolTipControlFSM.SendEvent("Tooltip/Close Tooltip");
    }
}
