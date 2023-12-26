using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using Sirenix.OdinInspector;

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


public class PullRequestMsg_FileChanged : SerializedMonoBehaviour
{
	[Header("i18n Translator key lists" )]
	[SerializeField]
	Dictionary<string, List<string>> i18nTranlateDict = new(){
		{"TitleMsg", new() }, //1. Author Name, 2. Contents
		{"FileContent", new() },//1. File Name 2~ Contents
		{"ReplyMsg", new() },	//1. Author Name, 2 Author Content, ...
	};
	
	
	[SerializeField] List<ReplyMsg> ReplyMsgList = new();
	

	
	[Header("Render Msg Timing")]
	[SerializeField]int renderQuestNum;
	[SerializeField]string renderActionType;
	
	
	[Header("Reference")]
	[SerializeField]Transform i18nTextTranslator;
	[SerializeField]Myi18nTranslateTool i18nTextTranslatorTool;
	
    // Start is called before the first frame update
    void Start()
    {
    	i18nTextTranslator = transform.Find("i18nTextTranslator");
    	i18nTextTranslatorTool = i18nTextTranslator.GetComponent<Myi18nTranslateTool>();
 
    	
    	ApplyTranslateTextToMsg();
    }
	
	void ApplyTranslateTextToMsg(){
		
		
	}
	
	public void UpdateMsg(string actionType, int currentQuestNum){
		List<ReplyMsg> foundList = ReplyMsgList.FindAll((Msg) => (Msg.renderQuestNum == currentQuestNum && Msg.renderActionType == actionType));
		foreach(ReplyMsg Msg in foundList){
			Msg.gameObject.SetActive(true);	
		}
	}
	
	

}


