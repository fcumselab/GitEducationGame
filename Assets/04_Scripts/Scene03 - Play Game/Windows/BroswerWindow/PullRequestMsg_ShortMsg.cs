using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class PullRequestMsg_ShortMsg : SerializedMonoBehaviour
{
    #region Variables
    
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

    [Header("Reference")]
    GameObject CommitHistoryWindow;
    PlayMakerFSM CommitHisotryWindowNPCActionFsm;
    GameObject BrowserWindow;
    Transform PRProgressFieldObj;

    #endregion

    #region function
    public bool ValidNeedRenderThisMsg(string actionType, int currentQuestNum)
    {
        return (actionType == renderActionType && renderQuestNum == currentQuestNum) ? true : false;
    }

    public void InitializeMsg(string actionType, int currentQuestNum)
    {
        GetReferenceValues();

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

    void GetReferenceValues()
    {
        CommitHistoryWindow = GameObject.Find("CommitHistoryWindow");
        CommitHisotryWindowNPCActionFsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "NPC Action");
        BrowserWindow = GameObject.Find("BrowserWindow");
        PRProgressFieldObj = BrowserWindow.transform.Find("ControllerGroup/PRDetailedPagePanel/PRProgressField");
    }

    void DoActionByActionTypeForNPC()
    {
        switch (actionTypeForNPC)
        {
            case "Push":
                DialogueLua.SetVariable("NPCCommitBranch", addCommitBranch);
                CommitHisotryWindowNPCActionFsm.FsmVariables.GetFsmString("runType").Value = "NPC-Commit";
                CommitHisotryWindowNPCActionFsm.enabled = true;
                int msgIndex = transform.GetSiblingIndex();
                PullRequestMsg_FileChanged LastFileChangedMsg = transform.parent.GetChild(msgIndex - 1).GetComponent<PullRequestMsg_FileChanged>();
                LastFileChangedMsg.ResolveConversationAction(); 
                break;
            case "MergePullRequest":
                PRProgressFieldObj.GetComponent<PullRequestProgressField>().ButtonClickActionMergePullRequest(AuthorText.text);
                break;
            default:
                //Skip the action
                break;
        }
    }
    #endregion
}


