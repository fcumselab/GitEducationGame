using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class HelpDialogueSystem : SerializedMonoBehaviour
{
    [SerializeField] string selectStageKey;
    [SerializeField] string lastDialogKey;
    [Header("Reference")]
    [SerializeField] PlayMakerFSM StarIcon;
    [SerializeField] Button DialogueButton;
    [SerializeField] Transform GameManager;
    [SerializeField] Transform StageManagerParent;
    [SerializeField] PlayMakerFSM ScoreFsm;
    PlayMakerFSM StageManagerParentFsm;
    public void InitializeReference()
    {
        GameManager = transform.parent;
        StageManagerParent = GameManager.Find("Stage Manager Parent");
        StageManagerParentFsm = MyPlayMakerScriptHelper.GetFsmByName(StageManagerParent.gameObject, "Loading StageManager");
        selectStageKey = StageManagerParentFsm.FsmVariables.GetFsmString("selectStageName").Value;
        Debug.Log("origin key: " + selectStageKey);

        selectStageKey = BuildDialogKey();
    }

    public void EnableDialog(string runType)
    {
        Debug.Log("EnableDialog: " + runType);
        
        switch (runType)
        {
            case "Help":
                Debug.Log("Help enable ");
                DialogueManager.StartConversation("Common/Help Selection");
                break;
            default:
                Debug.Log("Not found HelpDialogueSystem Key : " + runType);
                break;
        }
    }

    public string HelpDialogAction(string runType)
    {
        switch (runType)
        {
            case "Bye":
                Debug.Log("Bye enable ");
                return "End";
            case "Hint":
            case "Answer":
                Debug.Log("Help Answer enable ");
                PlayMakerFSM.BroadcastEvent("Help Dialogue System/Help Message Popup/Open  Popup");
                break;
            case "Hint(Action)":
                DialogueManager.StopConversation();
                DialogueManager.StartConversation(selectStageKey + "Hint");
                StarIcon.FsmVariables.GetFsmString("runType").Value = "usedHint";
                StarIcon.enabled = true;
                ScoreFsm.FsmVariables.GetFsmBool("usedHint").Value = true;
                break;
            case "Answer(Action)":
                DialogueManager.StopConversation();
                DialogueManager.StartConversation(selectStageKey + "Answer");
                StarIcon.FsmVariables.GetFsmString("runType").Value = "usedAnswer";
                StarIcon.enabled = true;
                ScoreFsm.FsmVariables.GetFsmBool("usedAnswer").Value = true;
                break;
            case "Summary":
                DialogueManager.StopConversation();
                DialogueManager.StartConversation(selectStageKey + runType);
                break;
            case "Current":
                lastDialogKey = DialogueLua.GetVariable("LastConversationKey").asString;
                DialogueManager.StopConversation();
                DialogueManager.StartConversation(lastDialogKey);
                break;
            default:
                Debug.Log("Not found HelpDialogueSystem Key : " + runType);
                break;
        }

        return "Continue";

    }

    string BuildDialogKey()
    {
        Debug.Log("Build  Key");
        string[] splitArr;
        string buildKey = "";
        if (selectStageKey.Contains("Tutorial"))
        {
            splitArr = selectStageKey.Split("(Tutorial)");
            splitArr[0] = splitArr[0].Trim();
            splitArr[0] = splitArr[0].Insert(splitArr[0].Length, "/Tutorial/");
            buildKey = splitArr[0];
        }
        else if(selectStageKey.Contains("Practice"))
        {
            splitArr = selectStageKey.Split("(Practice)");
            splitArr[0] = splitArr[0].Trim();
            splitArr[0] = splitArr[0].Insert(splitArr[0].Length, "/Practice/");
            buildKey = splitArr[0];
        }
        Debug.Log(buildKey);
        return buildKey;
    }
}
