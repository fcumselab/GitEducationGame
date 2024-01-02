using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;

public class PullRequestProgressField : SerializedMonoBehaviour
{
    #region Variables
    [SerializeField] GameObject PRProgressField;

    #region Basic TextBox
    [FoldoutGroup("Color")]
    [SerializeField] Color blockColor = new(255, 40, 55, 255);
    [FoldoutGroup("Color")]
    [SerializeField] Color blockTextColor = new(174, 22, 33, 255);
    [FoldoutGroup("Color")]
    [SerializeField] Color readyColor = new(31, 136, 61, 255);
    [FoldoutGroup("Color")]
    [SerializeField] Color readyTextColor = new(0, 102, 29, 255);

    [FoldoutGroup("Reviewer Detailed")]
    [SerializeField] GameObject ReviewerDetailedPanel;
    [FoldoutGroup("Reviewer Detailed")]
    [SerializeField] GameObject ReviewerDetailedTitleText;
    [FoldoutGroup("Reviewer Detailed")]
    [SerializeField] GameObject ReviewerDetailedContentText;
    [FoldoutGroup("Reviewer Detailed")]
    [SerializeField] GameObject ReviewerDetailedIconPanel;
    [FoldoutGroup("Reviewer Detailed")]
    [SerializeField] GameObject ReviewerDetailedCheckmarkIcon;
    [FoldoutGroup("Reviewer Detailed")]
    [SerializeField] GameObject ReviewerDetailedFileIcon;
    [FoldoutGroup("Reviewer Detailed")]
    [SerializeField] GameObject ReviewerDetailedLenIcon;

    [FoldoutGroup("Conversation Panel")]
    [SerializeField] GameObject ConversationPanel;
    [FoldoutGroup("Conversation Panel")]
    [SerializeField] GameObject ConversationTitleText;
    [FoldoutGroup("Conversation Panel")]
    [SerializeField] GameObject ConversationContentText;
    [FoldoutGroup("Conversation Panel")]
    [SerializeField] GameObject ConversationIconPanel;
    [FoldoutGroup("Conversation Panel")]
    [SerializeField] GameObject ConversationCheckmarkIcon;
    [FoldoutGroup("Conversation Panel")]
    [SerializeField] GameObject ConversationErrorIcon;

    [FoldoutGroup("Result Panel")]
    [SerializeField] GameObject ResultPanel;
    [FoldoutGroup("Result Panel")]
    [SerializeField] GameObject ResultTitleText;
    [FoldoutGroup("Result Panel")]
    [SerializeField] GameObject ResultContentText;
    [FoldoutGroup("Result Panel")]
    [SerializeField] GameObject ResultIconPanel;
    [FoldoutGroup("Result Panel")]
    [SerializeField] GameObject ResultCheckmarkIcon;
    [FoldoutGroup("Result Panel")]
    [SerializeField] GameObject ResultErrorIcon;

    #endregion

    #region Reference

    [FoldoutGroup("Reference")]
    PullRequestDetailedPage pullRequestDetailedPage;
    PullRequestDetailedPage_ConversationField pullRequestDetailedPage_ConversationField;
    [Header("NPC Action")]
    GameObject CommitHistoryWindow;
    PlayMakerFSM CommitHisotryWindowNPCActionFsm;

    #endregion

    #region Merge Pull Request Related Panel

    bool initial = false;
    [FoldoutGroup("PRProgressTextBox")]
    [SerializeField] GameObject PRProgressTextBox;
    [FoldoutGroup("PRProgressTextBox/Content")]
    [SerializeField] GameObject MergePRButton;
    [FoldoutGroup("PRProgressTextBox/Content")]
    PlayMakerFSM MergePRButtonFsm;

    [FoldoutGroup("MergePRInputField")]
    [SerializeField] GameObject MergePRInputField;
    [FoldoutGroup("MergePRInputField/Content")]
    [SerializeField] GameObject PRTitleText;
    [FoldoutGroup("MergePRInputField/Content")]
    [SerializeField] GameObject PRTitleTextToken;
    [FoldoutGroup("MergePRInputField/Content")]
    [SerializeField] GameObject PRContentText;
    [FoldoutGroup("MergePRInputField/Content")]
    [SerializeField] GameObject MergePRActionButton;

    [FoldoutGroup("SaveValue")]
    [SerializeField] Text playerText;
    #endregion

    #endregion

    public void InitializePRProgressField(PlayMakerFSM RepoQuestFsm)
    {
        PRContentText.GetComponent<LeanLocalizedText>().TranslationName = RepoQuestFsm.FsmVariables.GetFsmString("createPR1Title").Value;
        PRTitleTextToken.GetComponent<LeanLocalToken>().Value = RepoQuestFsm.FsmVariables.GetFsmInt("createPRNum").Value.ToString();

        pullRequestDetailedPage = transform.parent.GetComponent<PullRequestDetailedPage>();
        pullRequestDetailedPage_ConversationField = pullRequestDetailedPage.GetComponent<PullRequestDetailedPage_ConversationField>();
        MergePRButtonFsm = MyPlayMakerScriptHelper.GetFsmByName(MergePRButton, "Button Controller");
        MergePRButton.GetComponent<Button>().onClick.AddListener(() => SwitchToMergeInputField());
        MergePRActionButton.GetComponent<Button>().onClick.AddListener(() => ButtonClickActionMergePullRequest(playerText.text));
        CommitHistoryWindow = GameObject.Find("CommitHistoryWindow");
        CommitHisotryWindowNPCActionFsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "NPC Action");
        initial = true;
    } 
    
    public void UpdatePRProgress()
    {
        SwitchToPRProgressTextBox();
        DisableAllIcon();

        if (ConnectedButtonDict.Count == 0 && ApproveList.Count == 0)
        {
            ApplyNeedReviewProgressStyle();
            MergePRButtonFsm.FsmVariables.GetFsmBool("enableButton").Value = false;
            MergePRButtonFsm.enabled = true;
        }
        else if (ConnectedButtonDict.Count > 0)
        {
            ApplyFileChangeProgressStyle();
            MergePRButtonFsm.FsmVariables.GetFsmBool("enableButton").Value = false;
            MergePRButtonFsm.enabled = true;
        }
        else if (ConnectedButtonDict.Count == 0 && ApproveList.Count > 0)
        {
            ApplyApproveProgressStyle();
        }

        bool enable = (ConnectedButtonDict.Count != 0);
        ChangeRequest.SetActive(enable);
        enable = (ApproveList.Count != 0);
        ApproveRequest.SetActive(enable);
    }

    public void ClosePanel()
    {
        PRProgressField.SetActive(false);
    }

    #region Button Action

    public void ButtonClickActionMoveToTargetMsg(GameObject ClickButton)
    {
        Debug.Log("Click Action Move");
        PullRequestMsg_FileChanged connectedMsg = ConnectedButtonDict[ClickButton];
        pullRequestDetailedPage_ConversationField.MoveToTargetFileChangedMsg(connectedMsg);
    }

    public void SwitchToPRProgressTextBox()
    {
        PRProgressTextBox.SetActive(true);
        MergePRInputField.SetActive(false);
    }

    public void SwitchToMergeInputField()
    {
        Debug.Log("Switch Merge...");

        PRProgressTextBox.SetActive(false);
        MergePRInputField.SetActive(true);
    }

    public void ButtonClickActionMergePullRequest(string authorName)
    {
        Debug.Log("Start Merge...");
        SwitchToMergeInputField();
        string titleText = PRTitleText.GetComponent<Text>().text;
        string desText = PRContentText.GetComponent<Text>().text;
        string commitMsg = (titleText + "\n" + desText);
        CommitHisotryWindowNPCActionFsm.FsmVariables.GetFsmString("commitMessage").Value = commitMsg;
        CommitHisotryWindowNPCActionFsm.FsmVariables.GetFsmString("commitAuthor").Value = authorName;

        CommitHisotryWindowNPCActionFsm.FsmVariables.GetFsmString("runType").Value = "PRMerge";
        CommitHisotryWindowNPCActionFsm.enabled = true;

        StartCoroutine(pullRequestDetailedPage.WaitForMergePullRequestFinish(CommitHisotryWindowNPCActionFsm));

    }

    #endregion

    #region Progress Update

    #region Request Panel
    [FoldoutGroup("Request Panel")]
    [SerializeField] GameObject ChangeRequest;
    [FoldoutGroup("Request Panel/ChangeRequest")]
    [SerializeField] Transform ChangeRequestItemLocation;
    [FoldoutGroup("Request Panel/ChangeRequest")]
    [SerializeField] GameObject ChangeRequestItemPrefab;
    [FoldoutGroup("Request Panel/ChangeRequest")]
    [SerializeField] Dictionary<GameObject, PullRequestMsg_FileChanged> ConnectedButtonDict = new();
    [FoldoutGroup("Request Panel")]
    [SerializeField] GameObject ApproveRequest;
    [FoldoutGroup("Request Panel/ApproveRequest")]
    [SerializeField] Transform ApproveRequestItemLocation;
    [FoldoutGroup("Request Panel/ApproveRequest")]
    [SerializeField] GameObject ApproveRequestItemPrefab;
    [FoldoutGroup("Request Panel/ApproveRequest")]
    [SerializeField] List<PullRequestMsg_Approve> ApproveList = new();

    public void CreateChangeRequestItem(PullRequestMsg_FileChanged newFileChangedMsg)
    {
        Debug.Log("New ChangeRequest created");
        GameObject newItem = Instantiate(ChangeRequestItemPrefab);
        newItem.name = "ChangeRequestItem";
        newItem.transform.SetParent(ChangeRequestItemLocation);
        newItem.transform.localScale = new(1, 1, 1);

        //Add Connection between button and fileChange Msg
        ConnectedButtonDict.Add(newItem, newFileChangedMsg);
        pullRequestDetailedPage_ConversationField.ExistFileChangedMsgList.Add(newFileChangedMsg);

        //Apply Msg value to newItem
        newItem.transform.Find("Left/TextBox/AuthorText").GetComponent<LeanLocalizedText>().TranslationName = newFileChangedMsg.authorName;
        newItem.transform.Find("Right/ActionButton").GetComponent<Button>().onClick.AddListener(() => ButtonClickActionMoveToTargetMsg(newItem));
    }

    public void RemoveChangeRequestItem(GameObject resolvedFileChangedMsg)
    {
        PullRequestMsg_FileChanged targetMsg = resolvedFileChangedMsg.GetComponent<PullRequestMsg_FileChanged>();
        GameObject targetButton = null;
        foreach (var dict in ConnectedButtonDict)
        {
            if (dict.Value == targetMsg)
            {
                targetButton = dict.Key;
                break;
            }
        }
        ConnectedButtonDict.Remove(targetButton);
        pullRequestDetailedPage_ConversationField.ExistFileChangedMsgList.Remove(targetMsg);
    }

    public void CreateApproveItem(GameObject newApproveMsg)
    {
        Debug.Log("New CreateApproveItem created");

        PullRequestMsg_Approve script = newApproveMsg.GetComponent<PullRequestMsg_Approve>();

        GameObject newItem = Instantiate(ApproveRequestItemPrefab);
        newItem.name = "ApproveRequestItem";
        newItem.transform.SetParent(ApproveRequestItemLocation);
        newItem.transform.localScale = new(1, 1, 1);

        //Add completed fileChange Msg
        ApproveList.Add(script);

        //Apply Msg value to newItem
        newItem.transform.Find("Left/TextBox/AuthorText").GetComponent<LeanLocalizedText>().TranslationName = script.authorName;
    }
    #endregion

    #region Progress Content

    void ApplyNeedReviewProgressStyle()
    {
        ReviewerDetailedLenIcon.SetActive(true);
        ReviewerDetailedIconPanel.GetComponent<Image>().color = blockColor;
        ReviewerDetailedTitleText.GetComponent<Text>().color = blockTextColor;
        ReviewerDetailedTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Detailed/Title(Review)";
        ReviewerDetailedContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Detailed/Content(Review)";

        ConversationCheckmarkIcon.SetActive(true);
        ConversationIconPanel.GetComponent<Image>().color = readyColor;
        ConversationTitleText.GetComponent<Text>().color = readyTextColor;
        ConversationTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Conversation/Title(Ready)";
        ConversationContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Conversation/Content(Ready)";

        ResultErrorIcon.SetActive(true);
        ResultIconPanel.GetComponent<Image>().color = blockColor;
        ResultTitleText.GetComponent<Text>().color = blockTextColor;
        ResultTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Result/Title(Block)";
        ResultContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Result/Content(Block)";

        MergePRButtonFsm.FsmVariables.GetFsmBool("enableButton").Value = false;
        MergePRButtonFsm.enabled = true;
    }

    void ApplyApproveProgressStyle()
    {
        ReviewerDetailedCheckmarkIcon.SetActive(true);
        ReviewerDetailedIconPanel.GetComponent<Image>().color = readyColor;
        ReviewerDetailedTitleText.GetComponent<Text>().color = readyTextColor;
        ReviewerDetailedTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Detailed/Title(Ready)";
        ReviewerDetailedContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Detailed/Content(Ready)";

        ConversationCheckmarkIcon.SetActive(true);
        ConversationIconPanel.GetComponent<Image>().color = readyColor;
        ConversationTitleText.GetComponent<Text>().color = readyTextColor;
        ConversationTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Conversation/Title(Ready)";
        ConversationContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Conversation/Content(Ready)";

        ResultCheckmarkIcon.SetActive(true);
        ResultIconPanel.GetComponent<Image>().color = readyColor;
        ResultTitleText.GetComponent<Text>().color = readyTextColor;
        ResultTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Result/Title(Ready)";
        ResultContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Result/Content(Ready)";

        MergePRButtonFsm.FsmVariables.GetFsmBool("enableButton").Value = true;
        MergePRButtonFsm.enabled = true;
    }

    void ApplyFileChangeProgressStyle()
    {
        ReviewerDetailedFileIcon.SetActive(true);
        ReviewerDetailedIconPanel.GetComponent<Image>().color = blockColor;
        ReviewerDetailedTitleText.GetComponent<Text>().color = blockTextColor;
        ReviewerDetailedTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Detailed/Title(FileChange)";
        ReviewerDetailedContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Detailed/Content(FileChange)";

        ConversationErrorIcon.SetActive(true);
        ConversationIconPanel.GetComponent<Image>().color = blockColor;
        ConversationTitleText.GetComponent<Text>().color = blockTextColor;
        ConversationTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Conversation/Title(Block)";
        ConversationContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Conversation/Content(Block)";

        ResultErrorIcon.SetActive(true);
        ResultIconPanel.GetComponent<Image>().color = blockColor;
        ResultTitleText.GetComponent<Text>().color = blockTextColor;
        ResultTitleText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Result/Title(Block)";
        ResultContentText.GetComponent<LeanLocalizedText>().TranslationName = "BrowserWindow/PRDetailed/PRProgressTextBox/Result/Content(Block)";

        MergePRButtonFsm.FsmVariables.GetFsmBool("enableButton").Value = false;
        MergePRButtonFsm.enabled = true;
    }

    void DisableAllIcon()
    {
        ReviewerDetailedFileIcon.SetActive(false);
        ReviewerDetailedLenIcon.SetActive(false);
        ReviewerDetailedCheckmarkIcon.SetActive(false);
        ConversationCheckmarkIcon.SetActive(false);
        ConversationErrorIcon.SetActive(false);
        ResultCheckmarkIcon.SetActive(false);
        ResultErrorIcon.SetActive(false);
    }
    #endregion

    #endregion
}
