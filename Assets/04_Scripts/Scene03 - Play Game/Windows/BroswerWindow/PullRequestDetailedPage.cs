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
	
	[Header("Create PR")]
	[SerializeField] CreatePRMsg createPRMsg;
	[SerializeField] PRMainTitle pRMainTitle;
	
	[Header("Reference")]
	Transform QuestTracker;
	Transform StageManager;
	Transform RepoQuestData;
	PlayMakerFSM RepoQuestFsm;
	Transform ConversationField;
	Transform FilesChangedField;
	
    
	public void GetActionByButton(string actionType, int currentQuestNum, bool createByPlayer = false){
		WebsiteLoadingPanel.SetActive(true);
		PRDetailedPage.SetActive(true);
		
		if(isInitial){
			InitializePullRequestPage(createByPlayer);
		}else{
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
		ConversationField = RepoQuestData.Find("ConversationField"); 
		FilesChangedField = ConversationField.Find("FilesChangedField"); 

		if(createdByPlayer){
			Debug.Log("create by player");
			pRMainTitle.PRMainTitleText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPR1Title").Value;
			pRMainTitle.PRAuthorText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
			pRMainTitle.PRIDText.text = $"#{RepoQuestFsm.FsmVariables.GetFsmInt("createPRNum").Value}";
			
			string[] branchList = RepoQuestFsm.FsmVariables.GetFsmString("correctBranchName").Value.Split("/");
			pRMainTitle.BaseBranchText.text = branchList[0];
			pRMainTitle.CompareBranchText.text = branchList[1];
			
			createPRMsg.CreatePRMsgAuthorText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPRAuthor").Value;
			createPRMsg.CreatePRMsgDetailedText.text = RepoQuestFsm.FsmVariables.GetFsmString("createPR2Des").Value;
			createPRMsg.CreatePRMsgTime.text = System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
		
			
			//(Need to give this new PR to FSM (RepoData)
			SaveNewPRItemToFsm();	
			PRListPage.GetComponent<PRListPage>().ChangeNeedUpdateValue(true);
		}else{
			
			
		}
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
	
	void AddNewMsg(string actionType){
		while(true){
			Transform Msg = ConversationField.GetChild(0);
			if(Msg){
				Msg.SetParent(TextMessageGroup.transform);
				Msg.gameObject.SetActive(true);
				
				switch(Msg.tag){
					case "PRDetailedMsg/Approve":
					
						break;
					case "PRDetailedMsg/FileChanged":
						ExistFileChangedMsgList.Add(Msg.GetComponent<PullRequestMsg_FileChanged>());
						break;
					case "PRDetailedMsg/ShortMsg":
					
						break;
				}
				
			}else{
				break;
			}
		}
	}
	
	
	
	
	public void UpdatePullRequestPage(string actionType, int currentQuestNum){
		AddNewMsg(actionType);
		UpdateFileChangedMsg(actionType, currentQuestNum);
	}
	
	void UpdateFileChangedMsg(string actionType, int currentQuestNum){
		foreach(var script in ExistFileChangedMsgList){
			script.UpdateMsg(actionType, currentQuestNum);
			
		}
		
	}
	
}
