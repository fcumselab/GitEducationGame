using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;

//Will be use in PullRequestDetailed Panel -> FileChangedField & FileChangedMsg in CoversationField
[Serializable]
public class PullRequestDetailed_FileChangedTextBox
{
	[Header("Prefab")]
	public GameObject FileContentTextBox_Head;
	public GameObject FileContentTextBox_Content;

	[Header("Please Fill in these value (i18n key) so TextBox can be generated.")]
	//These string texts are all i18n key
	public string fileName;
	public List<PullRequestDetailed_FileChanged> fileChanged;

	//generateType -> CoversationField/FileChangedField
	public void InitializeMsg(string generateType, GameObject TitleText, GameObject CloneLocation)
	{
		Debug.Log("PullRequestDetailed_FileChangedTextBox InitializeMsg...");
		bool openScript = false;
		switch (generateType)
		{
			case "CoversationField":
				openScript = false;
				break;
			case "FileChangedField":
				openScript = true;
				break;
		}

		TitleText.GetComponent<LeanLocalizedText>().TranslationName = fileName;

		//Head
		GameObject CloneObj = UnityEngine.Object.Instantiate(FileContentTextBox_Head);
		CloneObj.transform.SetParent(CloneLocation.transform);
		CloneObj.transform.localScale = new(1, 1, 1);
		CloneObj.name = "FileChangedTextBox (Head)";

		//Content
		foreach (PullRequestDetailed_FileChanged fileContent in fileChanged)
        {
			CloneObj = UnityEngine.Object.Instantiate(FileContentTextBox_Content);
			CloneObj.transform.SetParent(CloneLocation.transform);
			CloneObj.transform.localScale = new(1, 1, 1);
			CloneObj.name = "FileChangedTextBox (Content)";

			if (fileContent.type == "A")
            {
				CloneObj.transform.Find("TextBox/AddPanel/NumText").GetComponent<Text>().text = $"{fileContent.addLineNum}";
				CloneObj.transform.Find("TextBox/RemovePanel/NumText").GetComponent<Text>().text = "";
				CloneObj.transform.Find("TextBox/Content/MarkText").GetComponent<Text>().text = "+";
			}
			else if (fileContent.type == "D")
			{
				CloneObj.transform.Find("TextBox/AddPanel/NumText").GetComponent<Text>().text = "";
				CloneObj.transform.Find("TextBox/RemovePanel/NumText").GetComponent<Text>().text = $"{fileContent.deleteLineNum}";
				CloneObj.transform.Find("TextBox/Content/MarkText").GetComponent<Text>().text = $" -";
			}
			else if (fileContent.type == "N") {
				CloneObj.transform.Find("TextBox/AddPanel/NumText").GetComponent<Text>().text = $"{fileContent.addLineNum}";
				CloneObj.transform.Find("TextBox/RemovePanel/NumText").GetComponent<Text>().text = $"{fileContent.deleteLineNum}";
				CloneObj.transform.Find("TextBox/Content/MarkText").GetComponent<Text>().text = "  ";
			}
			else
            {
				Debug.LogError("Warning: Set Wrong values. Please Give add/remove/noChange.");
            }

			CloneObj.transform.Find("TextBox/Content/ContentText").GetComponent<LeanLocalizedText>().TranslationName = fileContent.content;
			
			PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(CloneObj, "Update Color");
			fsm.enabled = true;

			//Do Action by openScript is on or off.
			Transform selectedBorder = CloneObj.transform.Find("SelectedBorder");
			selectedBorder.gameObject.SetActive(openScript);
			fsm = MyPlayMakerScriptHelper.GetFsmByName(CloneObj, "Tooltip");
			fsm.enabled = openScript;
		}
	}
}

[Serializable]
public class PullRequestDetailed_FileChanged
{
	//A -> add/D -> delete /N -> noChange
	public string type;
	public string content;
	public int addLineNum;
	public int deleteLineNum;
	public bool canClick;
}

public class PullRequestDetailedPage : SerializedMonoBehaviour
{
	[SerializeField] bool isInitial = true;
	[SerializeField] bool isLoading = false;

	DateTime startTime;

	[Header("Panel")]
	[SerializeField] Button RefreshPageButton;


	[Header("Panel")]
	[SerializeField] GameObject WebsiteLoadingPanel;
	[SerializeField] GameObject PRDetailedPage;
	[SerializeField] GameObject PRListPage;

	[Header("Page Content Script")]
	[SerializeField] PullRequestDetailedPage_CommitsField commitsField;
	[SerializeField] PullRequestDetailedPage_ConversationField conversationField;

	
    private void Start()
    {
		commitsField = GetComponent<PullRequestDetailedPage_CommitsField>();
		conversationField = GetComponent<PullRequestDetailedPage_ConversationField>();
	}

    public void GetActionByButton(string actionType, int currentQuestNum, bool createByPlayer = false){
		
		StartCoroutine(WaitForFinish());

		if (isInitial)
		{
			PRDetailedPage.SetActive(true);
			string[] branchList = conversationField.InitializePullRequestPage(createByPlayer);
			commitsField.SetPRTargetBranches(branchList);

			if (createByPlayer)
			{
				PRListPage.GetComponent<PRListPage>().ChangeNeedUpdateValue(true);
			}

			isInitial = false;
			UpdatePullRequestPage("", -1);
		}
		else
		{
			UpdatePullRequestPage(actionType, currentQuestNum);
		}
	}

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

	public void UpdatePullRequestPage(string actionType, int currentQuestNum)
	{

		conversationField.AddNewMsg(actionType, currentQuestNum);
		conversationField.UpdateFileChangedMsg(actionType, currentQuestNum);
		
		commitsField.UpdateCommitsField();

		isLoading = false;
	}
}
