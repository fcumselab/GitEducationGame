using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;
using Sirenix.OdinInspector;

public class PullRequestDetailedPage_ConversationField : SerializedMonoBehaviour
{
    [Header("Data")]
    public List<PullRequestMsg_FileChanged> ExistFileChangedMsgList;

    [FoldoutGroup("PR Status Icon")]
    [SerializeField] GameObject OpenButton;
    [FoldoutGroup("PR Status Icon")]
    [SerializeField] GameObject MergedButton;

    [Header("Children")]
    [SerializeField] GameObject TextMessageGroup_Conversation;
    [SerializeField] Text FieldSelectionNumText;

    [Header("Review List")]
    [SerializeField] GameObject ReviewerListPanel_Obj;
    ReviewerListPanel reviewerListPanel_Script;

    [Header("Reference")]
    [SerializeField] Transform BrowserWindowContentPanel;
    [SerializeField] Transform BrowserWindowScrollView;

    Transform StageManager;
    Transform RepoQuestData;
    PlayMakerFSM RepoQuestFsm;
    Transform RepoQuest_ConversationField;

    void Start()
    {
        reviewerListPanel_Script = ReviewerListPanel_Obj.GetComponent<ReviewerListPanel>();
    }

    #region PRProgressField

    [FoldoutGroup("PRProgressField")]
    [SerializeField] PullRequestProgressField PRProgressFieldScript;
    public void InitializePRProgressField()
    {
        PRProgressFieldScript.InitializePRProgressField(RepoQuestFsm);
    }

    public void UpdatePRProgressField()
    {
        PRProgressFieldScript.UpdatePRProgress();
    }

    //If this pull request has been merge.
    public void UpdateMergedPRContent()
    {
        PRProgressFieldScript.ClosePanel();
        OpenButton.SetActive(false);
        MergedButton.SetActive(true);
    }

    #endregion

    public void MoveToTargetFileChangedMsg(PullRequestMsg_FileChanged targetMsg)
    {
        RectTransform targetRect = targetMsg.GetComponent<RectTransform>();
        ScrollRect scrollRect = BrowserWindowScrollView.GetComponent<ScrollRect>();
        RectTransform contentPanelRect = BrowserWindowContentPanel.GetComponent<RectTransform>();

        Canvas.ForceUpdateCanvases();
        contentPanelRect.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanelRect.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(targetRect.position);
    }

    public void AddNewMsg(string actionType, int currentQuestNum)
    {
        while (true)
        {
            if (RepoQuest_ConversationField.childCount > 0)
            {
                Transform Msg = RepoQuest_ConversationField.GetChild(0);
                switch (Msg.tag)
                {
                    case "PRDetailedMsg/Approve":
                        PullRequestMsg_Approve approveMsg = Msg.GetComponent<PullRequestMsg_Approve>();
                        if (approveMsg.ValidNeedRenderThisMsg(actionType, currentQuestNum))
                        {
                            Msg.SetParent(TextMessageGroup_Conversation.transform);
                            Msg.transform.localScale = new(1, 1, 1);
                            Msg.gameObject.SetActive(true);
                            approveMsg.InitializeMsg(actionType, currentQuestNum);
                            break;
                        }
                        else
                        {
                            return;
                        }
                    case "PRDetailedMsg/FileChanged":
                        PullRequestMsg_FileChanged fileChangedMsg = Msg.GetComponent<PullRequestMsg_FileChanged>();
                        if (fileChangedMsg.ValidNeedRenderThisMsg(actionType, currentQuestNum))
                        {
                            PRProgressFieldScript.CreateChangeRequestItem(fileChangedMsg);
                            Msg.SetParent(TextMessageGroup_Conversation.transform);
                            Msg.transform.localScale = new(1, 1, 1);
                            Msg.gameObject.SetActive(true);
                            fileChangedMsg.InitializeMsg(actionType, currentQuestNum);
                            break;
                        }
                        else
                        {
                            return;
                        }
                    case "PRDetailedMsg/ShortMsg":
                        PullRequestMsg_ShortMsg shortMsg = Msg.GetComponent<PullRequestMsg_ShortMsg>();
                        if (shortMsg.ValidNeedRenderThisMsg(actionType, currentQuestNum))
                        {
                            Msg.SetParent(TextMessageGroup_Conversation.transform);
                            Msg.transform.localScale = new(1, 1, 1);
                            Msg.gameObject.SetActive(true);
                            shortMsg.InitializeMsg(actionType, currentQuestNum);
                            break;
                        }
                        else
                        {
                            return;
                        }
                }
            }
            else
            {
                return;
            }
        }
    }

    public void UpdateFileChangedMsg(string actionType, int currentQuestNum)
    {
        foreach (var script in ExistFileChangedMsgList)
        {
            script.UpdateReplyMsg(actionType, currentQuestNum);
        }

        //Update SelectionNumButton Text
        FieldSelectionNumText.text = $"{TextMessageGroup_Conversation.transform.childCount}";
    }

    #region InitializeMsg (Main/CreatePR)

    public string[] InitializePullRequestPage(bool createdByPlayer)
    {
        //Get All values	
        StageManager = GameObject.Find("Stage Manager").transform;
        RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
        RepoQuestFsm = MyPlayMakerScriptHelper.GetFsmByName(RepoQuestData.gameObject, "Repo Quest");
        RepoQuest_ConversationField = RepoQuestData.Find("ConversationField");

        string[] branchList = InitializeMainMsg();
        InitializeCreatePRMsg();

        //reviewerListPanel
        reviewerListPanel_Script.UpdateReviewerList(RepoQuestFsm);

        if (createdByPlayer)
        {
            SaveNewPRItemToFsm();
        }

        return branchList;
    }

    //Only Created By Player
    public void SaveNewPRItemToFsm()
    {
        int listLen = RepoQuestFsm.FsmVariables.GetFsmArray("existPRAuthorList").Length;
        string value = RepoQuestFsm.FsmVariables.GetFsmString("createPR1Title").Value;
        RepoQuestFsm.FsmVariables.GetFsmArray("existPRTitleList").InsertItem(value, listLen);
        value = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
        RepoQuestFsm.FsmVariables.GetFsmArray("existPRAuthorList").InsertItem(value, listLen);
        int intValue = RepoQuestFsm.FsmVariables.GetFsmInt("createPRNum").Value;
        RepoQuestFsm.FsmVariables.GetFsmArray("existPRNumList").InsertItem(intValue, listLen);
        RepoQuestFsm.FsmVariables.GetFsmArray("existPREnteredList").InsertItem(true, listLen);
    }

    #region Initialize MainMsg

    class PRMainTitle
    {
        public Text PRMainTitleText;
        public Text PRIDText;
        public Text PRAuthorText;
        public Text CompareBranchText;
        public Text BaseBranchText;
    }

    [SerializeField] PRMainTitle pRMainTitle = new();

    string[] InitializeMainMsg()
    {
        //Get Value from Fsm
        pRMainTitle.PRMainTitleText.GetComponent<LeanLocalizedText>().TranslationName = RepoQuestFsm.FsmVariables.GetFsmString("createPR1Title").Value;
        pRMainTitle.PRAuthorText.GetComponent<LeanLocalizedText>().TranslationName = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
        pRMainTitle.PRIDText.text = $"#{RepoQuestFsm.FsmVariables.GetFsmInt("createPRNum").Value}";

        string[] branchList = RepoQuestFsm.FsmVariables.GetFsmString("correctBranchName").Value.Split("/");
        pRMainTitle.BaseBranchText.text = branchList[0];
        pRMainTitle.CompareBranchText.text = branchList[1];
        return branchList;
    }
    #endregion

    #region Initialize CreatePRMsg 

    class CreatePRMsg
    {
        public GameObject createPRMsg;
        public Text CreatePRMsgAuthorText;
        public Text CreatePRMsgDetailedText;
        public Text CreatePRMsgTime;
    }

    [SerializeField] CreatePRMsg createPRMsg = new();

    void InitializeCreatePRMsg()
    {
        createPRMsg.CreatePRMsgAuthorText.GetComponent<LeanLocalizedText>().TranslationName = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
        createPRMsg.CreatePRMsgDetailedText.GetComponent<LeanLocalizedText>().TranslationName = RepoQuestFsm.FsmVariables.GetFsmString("createPR2Des").Value;
        createPRMsg.CreatePRMsgTime.text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }
    #endregion
    #endregion
}




