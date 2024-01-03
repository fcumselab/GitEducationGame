using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PullRequestMsg_FileChanged : SerializedMonoBehaviour
{
    #region Variables

    [Header("Data")]
    public bool isSolved = false;

    [FoldoutGroup("Render Whole Msg")]
    [SerializeField] int renderQuestNum;
    [FoldoutGroup("Render Whole Msg")]
    [SerializeField] string renderActionType;

    [FoldoutGroup("MainMsg")]
    public Text AuthorText;
    [FoldoutGroup("MainMsg")]
    public Text CommitMsgText;
    [FoldoutGroup("MainMsg")]
    public Text TimeText;
    [FoldoutGroup("MainMsg")]
    public string authorName;
    [FoldoutGroup("MainMsg")]
    public string commitMsg;

    [FoldoutGroup("File Changed TextBox")]
    [SerializeField] GameObject FileChangedTitleText;
    [FoldoutGroup("File Changed TextBox")]
    [SerializeField] GameObject FileChangedLocation;
    [FoldoutGroup("File Changed TextBox")]
    [SerializeField] PullRequestDetailed_FileChangedTextBox fileChangedTextBox = new();
    [FoldoutGroup("File Changed TextBox")]
    [SerializeField] GameObject ReplyMsgLocation;

    [FoldoutGroup("Children")]
    [SerializeField] Button FoldButton;

    [Header("Reference")]
    GameObject BrowserWindow;
    Transform pRDetailedPagePanel;
    Transform pullRequestProgressField;

    #endregion

    #region Main Update Function (Other scripts will call it)
    //When PullRequestDetailedPage Script trigger AddNewMsg function, do this once.
    public void InitializeMsg(string actionType, int currentQuestNum)
    {
        FoldButton.gameObject.SetActive(false);

        ResolvedButtonTooltipFsm = MyPlayMakerScriptHelper.GetFsmByName(ResolvedButton.gameObject, "Tooltip");
        InitialReplyButton();
        InitializeMainMsg();
        fileChangedTextBox.InitializeMsg("CoversationField", FileChangedTitleText, FileChangedLocation);
        UpdateReplyMsg(actionType, currentQuestNum);
    }

    public void InitializeMainMsg()
    {
        AuthorText.GetComponent<LeanLocalizedText>().TranslationName = authorName;
        CommitMsgText.GetComponent<LeanLocalizedText>().TranslationName = commitMsg;
        TimeText.text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    //If update function activate, update replyMsg field (including: replyBtn/replyInputField/replyMsg)
    public void UpdateReplyMsg(string actionType, int currentQuestNum)
    {
        fileChangedTextBox.UpdateReplyMsg(actionType, currentQuestNum, ReplyMsgLocation);

        //Update ReplyInputField/Button function
        UpdateReplyButtonStatus(currentQuestNum);
    }

    public bool ValidNeedRenderThisMsg(string actionType, int currentQuestNum)
    {
        return (actionType == renderActionType && renderQuestNum == currentQuestNum) ? true : false;
    }

    public bool ValidAllowPlayerReviewThisMsg(string actionType, string authorName)
    {
        return (actionType == renderActionType && authorName == this.authorName) ? true : false;
    }

    #endregion 

    #region Resolve Conversation Button/Function

    [FoldoutGroup("Children")]
    [SerializeField] Button ResolvedButton;
    PlayMakerFSM ResolvedButtonTooltipFsm;

    public void ResolveConversationAction()
    {
        //solveByNPC -> run resolve directly.
        if (!isSolved)
        {
            int currentQuestNum = QuestFilterManager.Instance.GetCurrentQuestNum();
            int replyMsgListCount = fileChangedTextBox.ReplyMsgList.Count;
            int allRenderMsgCount = fileChangedTextBox.ReplyMsgList.FindAll((Msg) => (Msg.isRender == true)).Count;

            if (currentQuestNum != fileChangedTextBox.ReplyMsgList[replyMsgListCount - 1].renderQuestNum)
            {
                ResolvedButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(FollowQuest)";
                ResolvedButtonTooltipFsm.enabled = true;
            }
            else if (allRenderMsgCount < replyMsgListCount)
            {
                ResolvedButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(ReplyFirst)";
                ResolvedButtonTooltipFsm.enabled = true;
            }
            else if (allRenderMsgCount == replyMsgListCount)
            {
                //Success -> fold Msg
                isSolved = true;
                FoldButton.gameObject.SetActive(true);
                MyPlayMakerScriptHelper.GetFsmByName(FoldButton.gameObject, "Button").enabled = true;

                if (!BrowserWindow)
                {
                    BrowserWindow = GameObject.Find("BrowserWindow");
                    pRDetailedPagePanel = BrowserWindow.transform.Find("ControllerGroup/PRDetailedPagePanel");
                    pullRequestProgressField = pRDetailedPagePanel.transform.Find("PRProgressField");
                }

                pullRequestProgressField.GetComponent<PullRequestProgressField>().RemoveChangeRequestItem(gameObject);

                ResolvedButton.transform.Find("TextBox/Text").GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(Unsolve)";
                //Quest Valider Enable
                QuestFilterManager.Instance.RunQuestValider(ResolvedButton.gameObject, "Button");

                //Loading for updating PR Progress (Show loading animation)
                pRDetailedPagePanel.GetComponent<PullRequestDetailedPage>().GetActionByButton("Resolve", currentQuestNum, false);
            }
        }
        else
        {
            ResolvedButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(SolveMsg)";
            ResolvedButtonTooltipFsm.enabled = true;
        }
    }

    #endregion

    #region ReplyButton/InputField Function

    [FoldoutGroup("Children")]
    [SerializeField] GameObject ReplyInputField;
    GameObject ReplyInputFieldPlaceHolder;
    GameObject ReplyInputFieldReplyText;

    [FoldoutGroup("Children")]
    [SerializeField] GameObject ReplyButton;
    PlayMakerFSM ReplyInputFieldFsm;
    PlayMakerFSM ReplyButtonContentFsm;
    PlayMakerFSM ReplyButtonTooltipFsm;

    void InitialReplyButton()
    {
        ReplyInputFieldReplyText = ReplyInputField.transform.Find("Panel/ReplyText").gameObject;
        ReplyInputFieldPlaceHolder = ReplyInputField.transform.Find("Panel/placeHolder").gameObject;
        ReplyInputFieldFsm = MyPlayMakerScriptHelper.GetFsmByName(ReplyInputField, "Update Content");
        ReplyButtonContentFsm = MyPlayMakerScriptHelper.GetFsmByName(ReplyButton, "Update Content");
        ReplyButtonTooltipFsm = MyPlayMakerScriptHelper.GetFsmByName(ReplyButton, "Tooltip");
    }

    public void FillInReplyInputField()
    {
        int currentQuestNum = QuestFilterManager.Instance.GetCurrentQuestNum();
        int foundIndex = fileChangedTextBox.ReplyMsgList.FindIndex((Msg) => (Msg.renderQuestNum == currentQuestNum && Msg.isRender == false && Msg.author == "Common/Player"));

        if (foundIndex != -1)
        {
            ReplyInputFieldReplyText.GetComponent<LeanLocalizedText>().TranslationName = fileChangedTextBox.ReplyMsgList[foundIndex].replyMsg;
            ReplyInputFieldReplyText.SetActive(true);
            ReplyInputFieldPlaceHolder.SetActive(false);
        }
    }

    void UpdateReplyButtonStatus(int currentQuestNum)
    {
        //Initialize
        bool canClick = (fileChangedTextBox.ReplyMsgList.FindIndex((Msg) => (Msg.renderQuestNum == currentQuestNum && Msg.isRender == false && Msg.author == "Common/Player")) != -1);
        ReplyInputFieldFsm.FsmVariables.GetFsmBool("canClick").Value = canClick;
        ReplyButtonContentFsm.FsmVariables.GetFsmBool("canClick").Value = canClick;
        ReplyInputFieldFsm.enabled = true;
        ReplyButtonContentFsm.enabled = true;
    }

    public void ClickReplyButton()
    {
        ReplyButtonContentFsm.FsmVariables.GetFsmBool("canClick").Value = false;
        ReplyButtonContentFsm.enabled = true;

        //Valid
        if (ReplyInputFieldPlaceHolder.activeSelf)
        {
            //Warning
            ReplyButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(ReplyFirst)";
            ReplyButtonTooltipFsm.enabled = true;
        }
        else
        {
            //Do Reply
            int currentQuestNum = QuestFilterManager.Instance.GetCurrentQuestNum();
            UpdateReplyMsg("Reply", currentQuestNum);

            ReplyInputFieldReplyText.SetActive(false);
            ReplyInputFieldPlaceHolder.SetActive(true);

            //Quest Valider Enable
            QuestFilterManager.Instance.RunQuestValider(ReplyButton, "Button");
        }
    }
    #endregion
}


