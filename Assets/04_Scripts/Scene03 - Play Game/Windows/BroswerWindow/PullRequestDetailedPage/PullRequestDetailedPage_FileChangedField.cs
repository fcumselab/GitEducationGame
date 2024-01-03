using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PullRequestDetailedPage_FileChangedField : SerializedMonoBehaviour
{
    [Header("Children")]
    [SerializeField] Transform FileChangedTextBoxLocation;
    [SerializeField] Text FieldSelectionNumText;

    [Header("Reference")]
    Transform StageManager;
    Transform RepoQuestData;
    Transform RepoQuest_FilesChangedField;
    Transform RepoQuest_ConversationField;
    [SerializeField] PullRequestDetailedPage PullRequestDetailedPageScript;

    public void InitializeField()
    {
        Debug.Log("InitializeField: PullRequestDetailedPage_FileChangedField");
        StageManager = GameObject.Find("Stage Manager").transform;
        RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
        RepoQuest_FilesChangedField = RepoQuestData.Find("FilesChangedField");
        RepoQuest_ConversationField = RepoQuestData.Find("ConversationField");
        PullRequestDetailedPageScript = GetComponent<PullRequestDetailedPage>();
        InitializeReviewChangePopup();
    }

    public void UpdateFileChangedField(string actionType, int currentQuestNum) {
        
        Debug.Log("File Changed data: " + actionType + " num: " + currentQuestNum);
        if (RepoQuest_FilesChangedField.childCount > 0)
        {
            FileChangedTextBoxGroup TextBox = RepoQuest_FilesChangedField.GetChild(0).GetComponent<FileChangedTextBoxGroup>();
            if (TextBox.ValidNeedRender(actionType, currentQuestNum))
            {
                if(FileChangedTextBoxLocation.childCount != 0)
                {
                    Debug.Log("Delete Old TextBox");
                    Transform LastTextBox = FileChangedTextBoxLocation.transform.GetChild(0);
                    Destroy(LastTextBox.gameObject);
                }
                //Debug.Log("A new TextBox");
                    
                TextBox.transform.SetParent(FileChangedTextBoxLocation.transform);
                TextBox.transform.localScale = new(1, 1, 1);
                TextBox.gameObject.SetActive(true);
                TextBox.InitializeContent();

                //Commits Field Button Switcher NumText
                FieldSelectionNumText.text = $"{TextBox.GetTextBoxCount()}";
            }
            else
            {
                //Debug.Log("Found TextBox but not the right one");
                return;
            }
        }
        else
        {
            //Debug.Log("All finish!");
            return;
        }

    }


    #region ReviewChangePopup
    [FoldoutGroup("Review Change Popup")]
    [SerializeField] GameObject ReviewChangeInputField;
    LeanLocalizedText ReviewChangeInputFieldText;
    PlayMakerFSM ReviewChangeInputFieldFsm;

    [FoldoutGroup("Review Change Popup")]
    [SerializeField] GameObject ReviewChangeButton;
    MouseTooltipTrigger ReviewChangeButtonTooltip;
    [FoldoutGroup("Review Change Popup")]
    [SerializeField] Button ReviewChangeAutoFillButton;
    [FoldoutGroup("Review Change Popup")]
    [SerializeField] string autoFillText;
    PlayMakerFSM ReviewChangeButtonFsm;
    [FoldoutGroup("Review Change Popup")]
    [SerializeField] GameObject SelectionReviewTypeGroup;
    [FoldoutGroup("Review Change Popup")]
    [SerializeField] List<GameObject> SelectionReviewTypeButtonList = new();
    PlayMakerFSM SelectionReviewTypeGroupFsm;

    void InitializeReviewChangePopup()
    {
        ReviewChangeInputFieldFsm = MyPlayMakerScriptHelper.GetFsmByName(ReviewChangeInputField, "Update Content");
        ReviewChangeButtonFsm = MyPlayMakerScriptHelper.GetFsmByName(ReviewChangeButton, "Button Controller");
        SelectionReviewTypeGroupFsm = MyPlayMakerScriptHelper.GetFsmByName(SelectionReviewTypeGroup, "Update Button");
        ReviewChangeAutoFillButton.onClick.AddListener(() => ButtonClickActionAutoFillText());
        ReviewChangeButton.GetComponent<Button>().onClick.AddListener(() => ButtonClickActionSubmitReview());
        ReviewChangeInputFieldText = ReviewChangeInputField.transform.Find("Panel/CommentText").GetComponent<LeanLocalizedText>();
    }

    #region Button Action
    void ButtonClickActionAutoFillText()
    {
        Debug.Log("Click auto fill text...");
        ReviewChangeInputFieldText.TranslationName = autoFillText;
    }

    void ButtonClickActionSubmitReview()
    {
        if (!ReviewChangeButtonTooltip) { ReviewChangeButtonTooltip = ReviewChangeButton.GetComponent<MouseTooltipTrigger>(); }
        Debug.Log("Submit review: ");
        //check text
        if(ReviewChangeInputFieldText.GetComponent<Text>().text == "")
        {
            ReviewChangeButtonTooltip.ClickButtonAction("BrowserWindow/PRDetailed/FilesChanged/ReviewChangePopup/Warning(Review)", true);
            return;
        }

        //check type
        GameObject SelectBtn = SelectionReviewTypeButtonList.Find((Btn) => Btn.activeSelf == true);
        //Comment, Approve, RequestChanges
        string buttonType = SelectBtn.name.Split('_')[1];
        Transform firstMsg = RepoQuest_ConversationField.GetChild(0);
        int currentQuestNum = QuestFilterManager.Instance.GetCurrentQuestNum();
        
        switch (firstMsg.tag)
        {
            case "PRDetailedMsg/Approve":
                if(buttonType == "Approve")
                {
                    PullRequestDetailedPageScript.GetActionByButton("ReviewChange(Approve)", currentQuestNum);
                }
                else
                {
                    ReviewChangeButtonTooltip.ClickButtonAction("BrowserWindow/PRDetailed/FilesChanged/ReviewChangePopup/Warning(Type)", true);
                    return;
                }
                break;
            case "PRDetailedMsg/FileChanged":
                if (buttonType == "RequestChanges")
                {
                    PullRequestDetailedPageScript.GetActionByButton("ReviewChange(FileChange)", currentQuestNum);
                }
                else
                {
                    ReviewChangeButtonTooltip.ClickButtonAction("BrowserWindow/PRDetailed/FilesChanged/ReviewChangePopup/Warning(Type)", true);
                    return;
                }
                break;
            default:
                ReviewChangeButtonTooltip.ClickButtonAction("BrowserWindow/PRDetailed/FilesChanged/ReviewChangePopup/Warning(Type)", true);
                return;
        }
    }

    #endregion

    public void UpdateReviewChangePopup()
    {
        Debug.Log("UpdateReviewChangePopup...");

        bool canClick = false;
        if(RepoQuest_ConversationField.transform.childCount != 0)
        {
            Transform firstMsg = RepoQuest_ConversationField.GetChild(0);
            switch (firstMsg.tag)
            {
                case "PRDetailedMsg/Approve":
                    PullRequestMsg_Approve approveMsg = firstMsg.GetComponent<PullRequestMsg_Approve>();
                    if(approveMsg.ValidAllowPlayerReviewThisMsg("ReviewChange(Approve)", "Common/Player"))
                    {
                        Debug.Log("ApproveMsg Ok");
                        autoFillText = approveMsg.reviewText;
                        canClick = true;
                        
                    }
                    break;
                case "PRDetailedMsg/FileChanged":
                    PullRequestMsg_FileChanged fileChangedMsg = firstMsg.GetComponent<PullRequestMsg_FileChanged>();
                    if (fileChangedMsg.ValidAllowPlayerReviewThisMsg("ReviewChange(FileChange)", "Common/Player"))
                    {
                        Debug.Log("fileChangedMsg Ok");
                        autoFillText = fileChangedMsg.commitMsg;
                        canClick = true;
                    }
                    break;
                default:
                    //canClick = false
                    break;
            }
        }
        if (!canClick)
        {
            autoFillText = "";
            Debug.Log("Disable Review Button");
        }
        ReviewChangeInputFieldFsm.FsmVariables.GetFsmBool("canClick").Value = canClick;
        ReviewChangeInputFieldFsm.enabled = true;
        ReviewChangeButtonFsm.FsmVariables.GetFsmBool("enableButton").Value = canClick;
        ReviewChangeButtonFsm.enabled = true;
        SelectionReviewTypeGroupFsm.FsmVariables.GetFsmBool("isPlayer").Value = canClick;
        SelectionReviewTypeGroupFsm.enabled = true;
    }

    #endregion
}
