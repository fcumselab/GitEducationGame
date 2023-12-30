using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;

public class PullRequestProgressField : SerializedMonoBehaviour
{
    #region Variables

    #region Basic TextBox
    [FoldoutGroup("Color")]
    [SerializeField] Color blockColor = new(255,40,55,255);
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


    [FoldoutGroup("ChangeRequest")]
    [SerializeField] GameObject ChangeRequest;
    [SerializeField] Transform ChangeRequestItemLocation;
    [SerializeField] GameObject ChangeRequestItemPrefab;
    [SerializeField] Dictionary<GameObject, PullRequestMsg_FileChanged> ConnectedButtonDict = new();
    [FoldoutGroup("ApproveRequest")]
    [SerializeField] GameObject ApproveRequest;
    [SerializeField] Transform ApproveRequestItemLocation;
    [SerializeField] GameObject ApproveRequestItemPrefab;


    [FoldoutGroup("Reference")]
    PullRequestDetailedPage_ConversationField pullRequestDetailedPage;
    #endregion

    public void CreateChangeRequestItem(PullRequestMsg_FileChanged newFileChangedMsg)
    {
        Debug.Log("New Button created");
        GameObject newItem = Instantiate(ChangeRequestItemPrefab);
        newItem.name = "ChangeRequestItem";
        newItem.transform.SetParent(ChangeRequestItemLocation);
        newItem.transform.localScale = new(1, 1, 1);

        //Add Connection between button and fileChange Msg
        ConnectedButtonDict.Add(newItem, newFileChangedMsg);

        //Apply Msg value to newItem
        newItem.transform.Find("Left/TextBox/AuthorText").GetComponent<LeanLocalizedText>().TranslationName = newFileChangedMsg.authorName;
        newItem.transform.Find("Right/ActionButton").GetComponent<Button>().onClick.AddListener(() => ButtonClickActionMoveToTargetMsg(newItem));
    }

    public void ButtonClickActionMoveToTargetMsg(GameObject ClickButton)
    {
        Debug.Log("Click Action Move");
        PullRequestMsg_FileChanged connectedMsg = ConnectedButtonDict[ClickButton];
        pullRequestDetailedPage.MoveToTargetFileChangedMsg(connectedMsg);
    }
    #region Progress Update


    public void UpdatePRProgress()
    {
        if (!pullRequestDetailedPage)
        {
            pullRequestDetailedPage = transform.parent.GetComponent<PullRequestDetailedPage_ConversationField>();
        }

        DisableAllIcon();


        List<PullRequestMsg_FileChanged> existMsgList = null;
        int totalSolveCount = 0;

        ChangeRequest.SetActive(true);
        ApproveRequest.SetActive(true);

        if (existMsgList == null)
        {
            ApplyNeedReviewProgressStyle();
        }
        else
        {
            //Get Values.
            List<PullRequestMsg_FileChanged> foundList = existMsgList.FindAll((Msg) => (Msg.isSolved == true));

            int showIndex = 0;
            foreach (var Msg in existMsgList)
            {
                if (Msg.isSolved)
                {
                    totalSolveCount++;
                }
                else
                {
                    
                }
            }

            if (existMsgList.Count == totalSolveCount)
            {
                ApplyApproveProgressStyle();
            }
            else
            {
                ApplyFileChangeProgressStyle();
            }
        }
    }


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
}
