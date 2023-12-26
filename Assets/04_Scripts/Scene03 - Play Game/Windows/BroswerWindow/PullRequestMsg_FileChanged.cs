using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;

class MainMsg
{
	public Text AuthorText;
	public Text CommitMsgText;
	public Text TimeText;
	public string authorName;
	public string commitMsg;

	public void InitializeMsg(Myi18nTranslateTool i18nTextTranslatorTool)
    {
		AuthorText.text = i18nTextTranslatorTool.TranslateText(authorName);
		CommitMsgText.text = i18nTextTranslatorTool.TranslateText(commitMsg);
		TimeText.text = System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
	}
}

class ReplyMsg{
	public GameObject gameObject;
	
	[Header("Render Timing")]
	public int renderQuestNum;
	//Initial = show, newQuest, Refresh
	public string renderActionType;
	
	[Header("Content")]
	[SerializeField] string author;
	[SerializeField] string replyMsg;
}

class FileContent
{
	public string fileName;
	public List<string> fileContentList;

	public void InitializeMsg(Myi18nTranslateTool i18nTextTranslatorTool)
	{
		//Fix all Msg Script -> How to render it...
	}
}

public class PullRequestMsg_FileChanged : SerializedMonoBehaviour
{
	[Header("This Msg Render Timing")]
	[SerializeField] int renderQuestNum;
	[SerializeField] string renderActionType;

	[Header("Message Data" )]
	[SerializeField] MainMsg mainMsg;
	[SerializeField] FileContent fileContent;
	[SerializeField] List<ReplyMsg> ReplyMsgList = new();
	
	[Header("Reference")]
	Transform i18nTextTranslator;
	Myi18nTranslateTool i18nTextTranslatorTool;
	
    // Start is called before the first frame update
    void Start()
    {
    	i18nTextTranslator = transform.Find("i18nTextTranslator");
    	i18nTextTranslatorTool = i18nTextTranslator.GetComponent<Myi18nTranslateTool>();
    }
	
	public void UpdateMsg(string actionType, int currentQuestNum){
		List<ReplyMsg> foundList = ReplyMsgList.FindAll((Msg) => (Msg.renderQuestNum == currentQuestNum && Msg.renderActionType == actionType));
		foreach(ReplyMsg Msg in foundList){
			Msg.gameObject.SetActive(true);	
		}
	}
	
	public void InitializeMsg()
    {
		mainMsg.InitializeMsg(i18nTextTranslatorTool);

	}

	public bool ValidNeedRenderThisMsg(string actionType, int currentQuestNum)
    {
		return (actionType == renderActionType && renderQuestNum == currentQuestNum) ? true : false;
	}
}


