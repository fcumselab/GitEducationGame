using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[System.Serializable]
public class PullRequestReplyMsg
{
    [Header("Render Timing")]
    public int renderQuestNum;
    //Initial = Show, NewQuest, Refresh
    public string renderActionType;
    public bool isRender;

    [Header("Content")]
    public string author;
    public string replyMsg;

    public void InitializeReplyMsg(GameObject ReplyMsgPrefab, GameObject ReplyMsgLocation)
    {
        GameObject CloneObj = Object.Instantiate(ReplyMsgPrefab);
        CloneObj.transform.SetParent(ReplyMsgLocation.transform);
        CloneObj.transform.localScale = new(1, 1, 1);
        CloneObj.name = $"ReplyMsg_{renderActionType}_{renderQuestNum}";
        CloneObj.SetActive(true);

        CloneObj.transform.Find("Title/TextPanel/AuthorText").GetComponent<LeanLocalizedText>().TranslationName = author;
        CloneObj.transform.Find("Title/TextPanel/TimeText").GetComponent<Text>().text = System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
        CloneObj.transform.Find("Content/Text").GetComponent<LeanLocalizedText>().TranslationName = replyMsg;

        isRender = true;
    }
}

public class PullRequestMsg_FileChanged : SerializedMonoBehaviour
{
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

    [FoldoutGroup("Reply Msg")]
    [SerializeField] GameObject ReplyMsgPrefab;
    [FoldoutGroup("Reply Msg")]
    [SerializeField] GameObject ReplyMsgLocation;
    [FoldoutGroup("Reply Msg")]
    [SerializeField] List<PullRequestReplyMsg> ReplyMsgList = new();

    [FoldoutGroup("Children")]
    [SerializeField] Button ResolvedButton;
    PlayMakerFSM ResolvedButtonTooltipFsm;
    [FoldoutGroup("Children")]
    [SerializeField] Button FoldButton;

    [Header("Reference")]
    GameObject QuestTracker;
    PlayMakerFSM QuestTrackerFsm;
    PlayMakerFSM QuestValiderFsm;

    PullRequestProgressField pullRequestProgressField;

    private void Awake()
{
        FoldButton.gameObject.SetActive(false);
        ResolvedButtonTooltipFsm = MyPlayMakerScriptHelper.GetFsmByName(ResolvedButton.gameObject, "Tooltip");
        InitialReplyButton();

        if (!QuestTracker)
        {
            QuestTracker = GameObject.Find("Quest Tracker");
            QuestTrackerFsm = MyPlayMakerScriptHelper.GetFsmByName(QuestTracker, "Quest Tracker");
            QuestValiderFsm = MyPlayMakerScriptHelper.GetFsmByName(QuestTracker, "Quest Valider");
        }
    }

    #region MainUpdate Function (Other scripts will call it)
    //When PullRequestDetailedPage Script trigger AddNewMsg function, do this once.
    public void InitializeMsg(string actionType, int currentQuestNum)
    {
        Debug.Log("InitializeMsg FileChange Msg");
        InitializeMainMsg();
        fileChangedTextBox.InitializeMsg("CoversationField", FileChangedTitleText, FileChangedLocation);
        UpdateReplyMsg(actionType, currentQuestNum);
    }

    public void InitializeMainMsg()
    {
        AuthorText.GetComponent<LeanLocalizedText>().TranslationName = authorName;
        CommitMsgText.GetComponent<LeanLocalizedText>().TranslationName = commitMsg;
        TimeText.text = System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
    }

    //If update function activate, update replyMsg field (including: replyBtn/replyInputField/replyMsg)
    public void UpdateReplyMsg(string actionType, int currentQuestNum)
    {
        List<PullRequestReplyMsg> foundList = ReplyMsgList.FindAll((Msg) =>
            (Msg.renderQuestNum == currentQuestNum && Msg.renderActionType == actionType && Msg.isRender == false)
        );

        foreach (PullRequestReplyMsg Msg in foundList)
        {
            Msg.InitializeReplyMsg(ReplyMsgPrefab, ReplyMsgLocation);
        }

        //Update ReplyInputField/Button function
        UpdateReplyButtonStatus(currentQuestNum);
    }

    public bool ValidNeedRenderThisMsg(string actionType, int currentQuestNum)
    {
        return (actionType == renderActionType && renderQuestNum == currentQuestNum) ? true : false;
    }
    #endregion 

    public void ResolveConversationAction()
    {
        int currentQuestNum = QuestTrackerFsm.FsmVariables.GetFsmInt("CurrentQuestNum").Value;
        int allRenderMsgCount = ReplyMsgList.FindAll((Msg) =>(Msg.isRender == true)).Count;

        Debug.Log("Count : " + allRenderMsgCount + " = " + ReplyMsgList.Count);

        if (currentQuestNum != ReplyMsgList[ReplyMsgList.Count-1].renderQuestNum)
        {
            //Error Please follow current quest.
            Debug.Log("FollowQuest.");

            ResolvedButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(FollowQuest)";
            ResolvedButtonTooltipFsm.enabled = true;
        }
        else if (allRenderMsgCount < ReplyMsgList.Count)
        {
            //Error Please Reply first.
            Debug.Log(" Reply first.");

            ResolvedButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(ReplyFirst)";
            ResolvedButtonTooltipFsm.enabled = true;
        }
        else if (allRenderMsgCount == ReplyMsgList.Count)
        {
            //Success -> fold Msg
            isSolved = true;
            FoldButton.gameObject.SetActive(true);
            MyPlayMakerScriptHelper.GetFsmByName(FoldButton.gameObject, "Button").enabled = true;
        }
    }

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
        int currentQuestNum = QuestTrackerFsm.FsmVariables.GetFsmInt("CurrentQuestNum").Value;
        int foundIndex = ReplyMsgList.FindIndex((Msg) => (Msg.renderQuestNum == currentQuestNum && Msg.isRender == false && Msg.author == "Common/Player"));
        
        Debug.Log("Fill Result: " + foundIndex);

        if (foundIndex != -1)
        {
            ReplyInputFieldReplyText.GetComponent<LeanLocalizedText>().TranslationName = ReplyMsgList[foundIndex].replyMsg;
            ReplyInputFieldReplyText.SetActive(true);
            ReplyInputFieldPlaceHolder.SetActive(false);
        }
    }

    void UpdateReplyButtonStatus(int currentQuestNum)
    {
        //Initialize
        bool canClick = (ReplyMsgList.FindIndex((Msg) => (Msg.renderQuestNum == currentQuestNum && Msg.isRender == false && Msg.author == "Common/Player")) != -1);
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
            Debug.Log("reply first:");
            //Warning
            ReplyButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(ReplyFirst)";
            ReplyButtonTooltipFsm.enabled = true;
        }
        else
        {
            Debug.Log("Run Reply!");

            //Do Reply
            int currentQuestNum = QuestTrackerFsm.FsmVariables.GetFsmInt("CurrentQuestNum").Value;
            UpdateReplyMsg("Reply", currentQuestNum);

            ReplyInputFieldReplyText.SetActive(false);
            ReplyInputFieldPlaceHolder.SetActive(true);

            //Quest Valider Enable
            MyPlayMakerScriptHelper.GetFsmByName(ReplyInputField, "Update Content");
            QuestValiderFsm.FsmVariables.GetFsmGameObject("Sender").Value = ReplyButton;
            QuestValiderFsm.FsmVariables.GetFsmString("senderName").Value = "Button";
            QuestValiderFsm.enabled = true;
        }
    }
    #endregion
}


