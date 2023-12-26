using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


class CreatePRMsg{
	public GameObject createPRMsg;
	public Text CreatePRMsgAuthorText;
	public Text CreatePRMsgDetailedText;
	public Text CreatePRMsgTime;
}

class PRMainTitle{
	public Text PRMainTitleText;
	public Text PRIDText;
	public Text PRAuthorText;
	public Text CompareBranchText;
	public Text BaseBranchText;

}
	
public class PullRequestDetailedPage : SerializedMonoBehaviour
{
	[SerializeField] bool isInitial = true;
	[Header("Children")]
	[SerializeField] GameObject TextMessageGroup;

	[Header("Msg")]
	[SerializeField] List<PullRequestMsg_FileChanged> ExistFileChangedMsgList;



	[Header("Panel")]
	[SerializeField] GameObject WebsiteLoadingPanel;
	[SerializeField] GameObject PRDetailedPage;
	[SerializeField] GameObject PRListPage;

	[Header("Page Content")]
	[SerializeField] PullRequestDetailedPage_CommitsField commitsField;


	[Header("Create PR")]
	[SerializeField] CreatePRMsg createPRMsg;
	[SerializeField] PRMainTitle pRMainTitle;

	[Header("Reference")]

	Transform QuestTracker;
	Transform StageManager;
	Transform RepoQuestData;
	PlayMakerFSM RepoQuestFsm;
	Transform RepoQuest_ConversationField;
	Transform RepoQuest_FilesChangedField;

    private void Start()
    {
		
		commitsField = GetComponent<PullRequestDetailedPage_CommitsField>();
	}

    public void GetActionByButton(string actionType, int currentQuestNum, bool createByPlayer = false){
		WebsiteLoadingPanel.SetActive(true);
		PRDetailedPage.SetActive(true);
		
		if(isInitial){
			InitializePullRequestPage(createByPlayer);
			UpdatePullRequestPage("", -1);
		}
		else
		{
			UpdatePullRequestPage(actionType, currentQuestNum);
		}
		
		WebsiteLoadingPanel.SetActive(false);
	}
	
	public void InitializePullRequestPage(bool createdByPlayer){

		//Get All values	
		QuestTracker = GameObject.Find("Quest Tracker").transform;
		StageManager = GameObject.Find("Stage Manager").transform;
		RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
		RepoQuestFsm = MyPlayMakerScriptHelper.GetFsmByName(RepoQuestData.gameObject, "Repo Quest");
		RepoQuest_ConversationField = RepoQuestData.Find("ConversationField");
		RepoQuest_FilesChangedField = RepoQuest_ConversationField.Find("FilesChangedField");


		//Get Value from Fsm
		pRMainTitle.PRMainTitleText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPR1Title").Value;
		pRMainTitle.PRAuthorText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
		pRMainTitle.PRIDText.text = $"#{RepoQuestFsm.FsmVariables.GetFsmInt("createPRNum").Value}";

		string[] branchList = RepoQuestFsm.FsmVariables.GetFsmString("correctBranchName").Value.Split("/");
		pRMainTitle.BaseBranchText.text = branchList[0];
		pRMainTitle.CompareBranchText.text = branchList[1];
		commitsField.SetPRTargetBranches(branchList[0], branchList[1]);

		createPRMsg.CreatePRMsgAuthorText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
		createPRMsg.CreatePRMsgDetailedText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPR2Des").Value;
		createPRMsg.CreatePRMsgTime.text = System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");


		if (createdByPlayer){
			SaveNewPRItemToFsm();	
			PRListPage.GetComponent<PRListPage>().ChangeNeedUpdateValue(true);
		}
	}

	public void UpdatePullRequestPage(string actionType, int currentQuestNum)
	{
		AddNewMsg(actionType, currentQuestNum);
		UpdateFileChangedMsg(actionType, currentQuestNum);
		commitsField.UpdateCommitsField();
	}

	public void SaveNewPRItemToFsm(){
		int listLen = RepoQuestFsm.FsmVariables.GetFsmArray("existPRAuthorList").Length;
		string value = RepoQuestFsm.FsmVariables.GetFsmString("createPR1Title").Value;
		RepoQuestFsm.FsmVariables.GetFsmArray("existPRTitleList").InsertItem(value, listLen);
		value = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
		RepoQuestFsm.FsmVariables.GetFsmArray("existPRAuthorList").InsertItem(value, listLen);
		int intValue = RepoQuestFsm.FsmVariables.GetFsmInt("createPRNum").Value;
		RepoQuestFsm.FsmVariables.GetFsmArray("existPRNumList").InsertItem(intValue, listLen);
		RepoQuestFsm.FsmVariables.GetFsmArray("existPREnteredList").InsertItem(true, listLen);
	}
	
	void AddNewMsg(string actionType, int currentQuestNum)
	{
		Debug.Log("addNewMsg");
		while(true){
			Transform Msg = RepoQuest_ConversationField.GetChild(0);
			if(Msg){
				switch(Msg.tag){
					case "PRDetailedMsg/Approve":

						return;
					case "PRDetailedMsg/FileChanged":
						PullRequestMsg_FileChanged fileChangedMsg = Msg.GetComponent<PullRequestMsg_FileChanged>();
						if (fileChangedMsg.ValidNeedRenderThisMsg(actionType, currentQuestNum))
                        {
							fileChangedMsg.InitializeMsg();
							ExistFileChangedMsgList.Add(fileChangedMsg);
							break;
						}
                        else
                        {
							return;
						}
					case "PRDetailedMsg/ShortMsg":

						return;
				}

				Msg.SetParent(TextMessageGroup.transform);
				Msg.gameObject.SetActive(true);
			}
			else{
				break;
			}
		}
	}

	void UpdateFileChangedMsg(string actionType, int currentQuestNum){
		foreach(var script in ExistFileChangedMsgList){
			script.UpdateMsg(actionType, currentQuestNum);
		}
	}
}
