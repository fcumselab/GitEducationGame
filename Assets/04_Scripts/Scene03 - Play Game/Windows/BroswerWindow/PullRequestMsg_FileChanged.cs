using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[System.Serializable]
public class PullRequestReplyMsg{
	public GameObject gameObject;
	
	[Header("Render Timing")]
	public int renderQuestNum;
	//Initial = Show, NewQuest, Refresh
	public string renderActionType;

	[Header("Content")]
	[SerializeField] string author;
	[SerializeField] string replyMsg;

	public void InitializeMsg()
    {

    }
}

public class PullRequestMsg_FileChanged : SerializedMonoBehaviour
{
	[FoldoutGroup("Render Whold Msg")]
	[SerializeField] int renderQuestNum;
	[FoldoutGroup("Render Whold Msg")]
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
	[SerializeField] List<PullRequestReplyMsg> ReplyMsgList = new();

    public void UpdateReplyMsg(string actionType, int currentQuestNum){
		List<PullRequestReplyMsg> foundList = ReplyMsgList.FindAll((Msg) => (Msg.renderQuestNum == currentQuestNum && Msg.renderActionType == actionType));
		foreach(PullRequestReplyMsg Msg in foundList){
			Msg.InitializeMsg();
			Msg.gameObject.SetActive(true);
		}
	}

	//When PullRequestDetailedPage Script trigger AddNewMsg function, do this once.
	public void InitializeMsg(string actionType, int currentQuestNum)
	{
		Debug.Log("InitializeMsg FileChange Msg");
		InitializeMainMsg();
		fileChangedTextBox.InitializeMsg("CoversationField", FileChangedTitleText, FileChangedLocation);
		UpdateReplyMsg(actionType, currentQuestNum);
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


