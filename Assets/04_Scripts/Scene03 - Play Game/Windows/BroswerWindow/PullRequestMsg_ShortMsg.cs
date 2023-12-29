using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class PullRequestMsg_ShortMsg : SerializedMonoBehaviour
{
    //For action
    bool isLoading = false;

    [FoldoutGroup("Render Whole Msg")]
    GameObject CommitHistoryWindow;
    PlayMakerFSM CommitHisotryWindowNPCActionFsm;

    #region Main Msg
    [FoldoutGroup("Render Whole Msg")]
    [SerializeField] int renderQuestNum;
    [FoldoutGroup("Render Whole Msg")]
    [SerializeField] string renderActionType;

    [FoldoutGroup("Action after Generated (Use for NPC Action Fsm)")]
    [SerializeField] string actionTypeForNPC;
    //This vari decide NPC commit push location.
    [FoldoutGroup("Action after Generated (Use for NPC Action Fsm)")]
    [SerializeField] string addCommitBranch;

    [FoldoutGroup("Data")]
    [SerializeField] string authorName;
    [FoldoutGroup("Data")]
    [SerializeField] string actionText;

    [FoldoutGroup("Children")]
    [SerializeField] Text AuthorText;
    [FoldoutGroup("Children")]
    [SerializeField] Text ActionText;
    [FoldoutGroup("Children")]
    [SerializeField] Text TimeText;
    [FoldoutGroup("Children")]
    [SerializeField] LeanLocalToken CommitIDToken;

    public bool ValidNeedRenderThisMsg(string actionType, int currentQuestNum)
    {
        return (actionType == renderActionType && renderQuestNum == currentQuestNum) ? true : false;
    }

    public void InitializeMsg(string actionType, int currentQuestNum)
    {
        Debug.Log("InitializeMsg Short Msg");
        //Initial Main Text
        AuthorText.GetComponent<LeanLocalizedText>().TranslationName = authorName;
        switch (actionType) {
            case "Push":
                ActionText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/ShortMsg/CreateCommit";
                break;
            default:
                ActionText.GetComponent<LeanLocalizedText>().TranslationName = actionText;
                break;
        }
        TimeText.text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");


        //Action after generated.
        DoActionByActionTypeForNPC();
    }

    void DoActionByActionTypeForNPC()
    {
        Debug.Log("Do action in shortMsg");
        if (!CommitHistoryWindow)
        {
            CommitHistoryWindow = GameObject.Find("CommitHistoryWindow");
            CommitHisotryWindowNPCActionFsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "NPC Action");
        }

        switch (actionTypeForNPC)
        {
            case "Push":
                DialogueLua.SetVariable("NPCCommitBranch", addCommitBranch);
                CommitHisotryWindowNPCActionFsm.FsmVariables.GetFsmString("runType").Value = "NPC-Commit";
                CommitHisotryWindowNPCActionFsm.enabled = true;
                break;
            default:
                //Skip the action
                break;
        }
    }
    #endregion
}


