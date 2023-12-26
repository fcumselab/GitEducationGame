using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

class PRList{
	public GameObject obj;
	public int PRID;
	public string Author;
	public string PRTitle;
	public bool canEnter;
}


public class PRListPage : SerializedMonoBehaviour
{
	[Header("Prefab")]
	[SerializeField] GameObject PrefabPullRequestListItem;
	
	[SerializeField] bool isInitial = true;
	[SerializeField] bool needUpdate = true;
	
	[SerializeField] string createByKey = "BrowserWindow/PRList/CreateBy";
	
	
	[Header("Panel")]
	[SerializeField] Text OpenedPRCountText;
	[SerializeField] GameObject OpenedPRList;
	[SerializeField] GameObject OpenedPRListNone;
	[SerializeField] Text ClosedPRCountText;
	[SerializeField] GameObject ClosedPRList;
	[SerializeField] GameObject ClosedPRListNone;
	
	[Header("PRList")]
	[SerializeField] List<PRList> OpenPRList = new();
	[SerializeField] List<PRList> ClosePRList = new();
	
	[Header("Reference")]
	Transform QuestTracker;
	Transform StageManager;
	Transform RepoQuestData;
	PlayMakerFSM RepoQuestFsm;

	[SerializeField]Transform i18nTextTranslator;
	[SerializeField]Myi18nTranslateTool i18nTextTranslatorTool;

	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{

		//Debug.Log("This is PRListPage start :" + isInitial);
		if(isInitial){
			//Get All values	
			QuestTracker = GameObject.Find("Quest Tracker").transform;
			StageManager = GameObject.Find("Stage Manager").transform;
			RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
			RepoQuestFsm = MyPlayMakerScriptHelper.GetFsmByName(RepoQuestData.gameObject, "Repo Quest");

			i18nTextTranslator = transform.Find("i18nTextTranslator");
			i18nTextTranslatorTool = i18nTextTranslator.GetComponent<Myi18nTranslateTool>();
		    
			UpdatePRList();
			isInitial = false;	 
		}else if(needUpdate){
			UpdatePRList();
		}
	}

	public void UpdatePRList(){
		//Debug.Log("UpdatePRList");
		for(int i=0;i< RepoQuestFsm.FsmVariables.GetFsmArray("existPRAuthorList").Length; i++){
			PRList curPRList = new();

			if(OpenPRList.Count - 1 < i){
				curPRList.obj = Instantiate(PrefabPullRequestListItem);
				curPRList.obj.name = "OpenedPRItem";
				curPRList.obj.transform.SetParent(OpenedPRList.transform);
				curPRList.obj.transform.localScale = new(1, 1, 1);
				curPRList.Author = (string)RepoQuestFsm.FsmVariables.GetFsmArray("existPRAuthorList").Get(i);
				curPRList.canEnter = (bool)RepoQuestFsm.FsmVariables.GetFsmArray("existPREnteredList").Get(i);
				curPRList.PRID = (int)RepoQuestFsm.FsmVariables.GetFsmArray("existPRNumList").Get(i);
				curPRList.PRTitle = (string)RepoQuestFsm.FsmVariables.GetFsmArray("existPRTitleList").Get(i);
				
				OpenPRList.Add(curPRList);
			}else{
				curPRList = OpenPRList[i];
			}
			
			//# {PRNum} created by {Author}
			string translatedText = i18nTextTranslatorTool.TranslateText(createByKey);
			translatedText = translatedText.Replace("{PRNum}",curPRList.PRID.ToString());
			translatedText = translatedText.Replace("{Author}",curPRList.Author);
			
			Transform targetText = curPRList.obj.transform.Find("TextWithIcon/PRTitleText");
			targetText.GetComponent<Text>().text = curPRList.PRTitle;
			targetText = targetText.Find("PRDetailedText");
			targetText.GetComponent<Text>().text = translatedText; 
		}
		
		UpdatePRListCountText();
		needUpdate = false;
	}
	
	public void UpdatePRListCountText(){
		OpenedPRCountText.text = $"{OpenPRList.Count} Opened";
		ClosedPRCountText.text = $"{ClosePRList.Count} Closed";
	}
	
	public void ChangeNeedUpdateValue(bool needUpdate){
		this.needUpdate	= needUpdate;
	}

	public int GetPRListCount(string type)
    {
		if(type == "closed")
        {
			return ClosePRList.Count;
		}
		else if(type == "opened")
        {
			return OpenPRList.Count;
        }
		return -1;
    }
}
