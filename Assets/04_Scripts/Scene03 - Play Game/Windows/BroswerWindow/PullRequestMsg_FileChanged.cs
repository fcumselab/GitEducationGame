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
    [SerializeField] string author;
    [SerializeField] string replyMsg;

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
    [FoldoutGroup("Children")]
    [SerializeField] GameObject ReplyInputField;
    [FoldoutGroup("Children")]
    [SerializeField] GameObject ReplyButton;


    [Header("Reference")]
    GameObject QuestTracker;
    PlayMakerFSM QuestTrackerFsm;

    private void Start()
    {
        FoldButton.gameObject.SetActive(false);
        ResolvedButtonTooltipFsm = MyPlayMakerScriptHelper.GetFsmByName(ResolvedButton.gameObject, "Tooltip");
    }

    //When PullRequestDetailedPage Script trigger AddNewMsg function, do this once.
    public void InitializeMsg(string actionType, int currentQuestNum)
    {
        Debug.Log("InitializeMsg FileChange Msg");
        InitializeMainMsg();
        fileChangedTextBox.InitializeMsg("CoversationField", FileChangedTitleText, FileChangedLocation);
        UpdateReplyMsg(actionType, currentQuestNum);
    }

    public void UpdateReplyMsg(string actionType, int currentQuestNum)
    {
        List<PullRequestReplyMsg> foundList = ReplyMsgList.FindAll((Msg) =>
            (Msg.renderQuestNum == currentQuestNum && Msg.renderActionType == actionType && Msg.isRender == false)
        );

        foreach (PullRequestReplyMsg Msg in foundList)
        {
            Msg.InitializeReplyMsg(ReplyMsgPrefab, ReplyMsgLocation);
        }
    }

    public void ResolveConversationAction()
    {
        if (!QuestTracker)
        {
            QuestTracker = GameObject.Find("Quest Tracker");
            QuestTrackerFsm = MyPlayMakerScriptHelper.GetFsmByName(QuestTracker, "Quest Tracker");
        }
        int currentQuestNum = QuestTrackerFsm.FsmVariables.GetFsmInt("CurrentQuestNum").Value;
        int allRenderMsgCount = ReplyMsgList.FindAll((Msg) =>(Msg.isRender == true)).Count;
        if (currentQuestNum != ReplyMsgList[ReplyMsgList.Count-1].renderQuestNum)
        {
            //Error Please follow current quest.
            ResolvedButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(FollowQuest)";
            ResolvedButtonTooltipFsm.enabled = true;
        }
        else if (allRenderMsgCount == ReplyMsgList.Count)
        {
            //Error Please Reply first.
            ResolvedButtonTooltipFsm.FsmVariables.GetFsmString("tooltipMessage").Value = "BrowserWindow/PRDetailed/FilesChanged/ResolvedButton(ReplyFirst)";
            ResolvedButtonTooltipFsm.enabled = true;
        }
        else
        {
            //Success
            isSolved = true;
            FoldButton.gameObject.SetActive(true);
            MyPlayMakerScriptHelper.GetFsmByName(FoldButton.gameObject, "Button").enabled = true;
        }
    }

    public bool ValidNeedRenderThisMsg(string actionType, int currentQuestNum)
    {
        return (actionType == renderActionType && renderQuestNum == currentQuestNum) ? true : false;
    }


    public void InitializeMainMsg()
    {
        AuthorText.GetComponent<LeanLocalizedText>().TranslationName = authorName;
        CommitMsgText.GetComponent<LeanLocalizedText>().TranslationName = commitMsg;
        TimeText.text = System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
    }
}


