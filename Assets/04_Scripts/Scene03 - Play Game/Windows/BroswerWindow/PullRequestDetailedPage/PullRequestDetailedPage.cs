using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;

public class PullRequestDetailedPage : SerializedMonoBehaviour
{
	[Header("Get value from click PRList item")]
	[SerializeField] bool isMerge = false;

	[SerializeField] bool isInitial = true;
	[SerializeField] bool isLoading = false;

	DateTime startTime;

	[Header("Action Eventer")]
	[SerializeField] Button RefreshPageButton;

	[Header("Panel")]
	[SerializeField] PlayMakerFSM BrowserURLFsm;
	[SerializeField] GameObject WebsiteLoadingPanel;
	[SerializeField] GameObject PRDetailedPage;
	[SerializeField] GameObject PRListPageObj;
	[SerializeField] PRListPage PRListPageScript;

	[Header("Page Content Script")]
	[SerializeField] PullRequestDetailedPage_ConversationField conversationField;
	[SerializeField] PullRequestDetailedPage_CommitsField commitsField;
	[SerializeField] PullRequestDetailedPage_FileChangedField fileChangedField;


	private void Awake()
    {
		commitsField = GetComponent<PullRequestDetailedPage_CommitsField>();
		conversationField = GetComponent<PullRequestDetailedPage_ConversationField>();
		fileChangedField = GetComponent<PullRequestDetailedPage_FileChangedField>();
	}

	public void SetIsMerge(bool isMerge)
    {
		this.isMerge = isMerge;
	}

    public void GetActionByButton(string actionType, int currentQuestNum, bool createByPlayer = false){
		
		StartCoroutine(WaitForFinish());

        if (isMerge)
        {
			UpdateMergedPullRequestPage();
		}
        else
        {
			if (isInitial && actionType == "EnterPRList")
            {
                PRDetailedPage.SetActive(true);

				string[] branchList = conversationField.InitializePullRequestPage(createByPlayer);

				conversationField.InitializePRProgressField();
				commitsField.SetPRTargetBranches(branchList);
				fileChangedField.InitializeField();

				if (createByPlayer)
				{
					PRListPageScript.UpdatePRList();
				}

				isInitial = false;
				UpdatePullRequestPage("Initial", currentQuestNum);
			}
			else if(!isInitial)
			{
				UpdatePullRequestPage(actionType, currentQuestNum);
            }
            else
            {
				isLoading = false;
			}
		}
	}

	public void UpdateMergedPullRequestPage()
    {
		conversationField.UpdateMergedPRContent();
		isLoading = false;
	}

	public void UpdatePullRequestPage(string actionType, int currentQuestNum)
	{
		conversationField.UpdateFileChangedMsg(actionType, currentQuestNum);

		conversationField.AddNewMsg(actionType, currentQuestNum);
		conversationField.UpdatePRProgressField();
		conversationField.UpdateFieldText();

		commitsField.UpdateCommitsField();
		
		fileChangedField.UpdateFileChangedField(actionType, currentQuestNum);
		fileChangedField.UpdateReviewChangePopup();

		isLoading = false;
	}

	public void UpdateFileChangedMsg(string actionType, int currentQuestNum)
    {
		conversationField.UpdateFileChangedMsg(actionType, currentQuestNum);
	}

	#region Waiting Animation
	IEnumerator WaitForFinish()
    {
		isLoading = true;
		startTime = DateTime.Now;
		WebsiteLoadingPanel.SetActive(true);
		RefreshPageButton.interactable = false;

		while (isLoading)
        {
			yield return null;
		}

		float elapsed = (float)(DateTime.Now - startTime).TotalSeconds;
		if (elapsed < 1)
		{
			Debug.Log($"Loading Finish Time: {1 - elapsed}");
			yield return new WaitForSeconds(1 - elapsed);
		}

		WebsiteLoadingPanel.SetActive(false);
		RefreshPageButton.interactable = true;
	}

	public IEnumerator WaitForMergePullRequestFinish(GameObject MergePRActionButton, PlayMakerFSM CommitHisotryWindowNPCActionFsm)
	{
		isLoading = true;
		startTime = DateTime.Now;
		WebsiteLoadingPanel.SetActive(true);
		RefreshPageButton.interactable = false;

		while (CommitHisotryWindowNPCActionFsm.enabled)
		{
			yield return null;
		}

		float elapsed = (float)(DateTime.Now - startTime).TotalSeconds;
		if (elapsed < 1)
		{
			Debug.Log($"Loading Finish Time: {1 - elapsed}");
			yield return new WaitForSeconds(1 - elapsed);
		}

		PRListPageScript.MoveOpenPRListToClose();

		PRDetailedPage.SetActive(false);
		PRListPageObj.SetActive(true);
		WebsiteLoadingPanel.SetActive(false);
		BrowserURLFsm.FsmVariables.GetFsmGameObject("OpenPage").Value = PRListPageObj;
		BrowserURLFsm.enabled = true;

		QuestTrackerManager.Instance.RunQuestValider(MergePRActionButton, "Button");
	}
	#endregion
}
