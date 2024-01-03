using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PullRequestMsg_Approve : SerializedMonoBehaviour
{
    #region Main Msg
    [FoldoutGroup("Render Whole Msg")]
    [SerializeField] int renderQuestNum;
    [FoldoutGroup("Render Whole Msg")]
    //Refresh -> NPC / ApproveByPlayer -> Player
    [SerializeField] string renderActionType;

    [FoldoutGroup("Data")]
    public string authorName;
    [FoldoutGroup("Data")]
    public string reviewText;

    [FoldoutGroup("Children")]
    [SerializeField] Text AuthorText;
    [FoldoutGroup("Children")]
    [SerializeField] Text ReviewText;
    [FoldoutGroup("Children")]
    [SerializeField] Text TimeText;
    #endregion

    [Header("Reference")]
    GameObject BrowserWindow;
    Transform pullRequestProgressField;

    public void InitializeMsg(string actionType, int currentQuestNum)
    {
        Debug.Log("InitializeMsg Approve Msg");
        //Initial Main Text
        AuthorText.GetComponent<LeanLocalizedText>().TranslationName = authorName;
        ReviewText.GetComponent<LeanLocalizedText>().TranslationName = reviewText;
        TimeText.text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        AddApproveItemInPRProgressField();
    }

    void AddApproveItemInPRProgressField()
    {
        Debug.Log("Add ApproveItem In PRProgressField");
        if (!BrowserWindow)
        {
            BrowserWindow = GameObject.Find("BrowserWindow");
            pullRequestProgressField = BrowserWindow.transform.Find("ControllerGroup/PRDetailedPagePanel/PRProgressField");
        }
        pullRequestProgressField.GetComponent<PullRequestProgressField>().CreateApproveItem(gameObject);
    }

    public bool ValidNeedRenderThisMsg(string actionType, int currentQuestNum)
    {
        return (actionType == renderActionType && renderQuestNum == currentQuestNum) ? true : false;
    }

    public bool ValidAllowPlayerReviewThisMsg(string actionType, string authorName)
    {
        return (actionType == renderActionType && authorName == this.authorName) ? true : false;
    }
}
