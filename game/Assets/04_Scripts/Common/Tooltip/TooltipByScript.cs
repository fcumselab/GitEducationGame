using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using HutongGames.PlayMaker;

public class TooltipByScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        bool usingi18nKey = Content.FsmVariables.GetFsmBool("usingi18nKey").Value;
        ToolTipControlFSM.FsmVariables.GetFsmString("tooltipMessage").Value = tooltipMessage;
        ToolTipControlFSM.FsmVariables.GetFsmBool("usingi18nKey").Value = usingi18nKey;
        ToolTipControlFSM.SendEvent("Tooltip/Show Tooltip by Script");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipControlFSM.SendEvent("Tooltip/Close Tooltip");
    }
}
