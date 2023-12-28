using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Lean.Localization;
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

	// This function is called when the object becomes enabled and active.
	protected void OnEnable()
	{
		if(isInitial){
			//Get All values	
			QuestTracker = GameObject.Find("Quest Tracker").transform;
			StageManager = GameObject.Find("Stage Manager").transform;

			//Only Stage like Create/Review PR will use them.
			RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
            if (RepoQuestData)
            {
				RepoQuestFsm = MyPlayMakerScriptHelper.GetFsmByName(RepoQuestData.gameObject, "Repo Quest");
			}

			UpdatePRList();
			isInitial = false;	 
		}else if(needUpdate){

			UpdatePRList();
		}
	}

	public void UpdatePRList(){
        //Debug.Log("UpdatePRList");
        if (RepoQuestFsm) { 
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
				LeanLocalToken token = curPRList.obj.transform.Find("TextWithIcon/TitleText/DetailedText/PRNUM").GetComponent<LeanLocalToken>();
				token.SetValue(curPRList.PRID);
				token = curPRList.obj.transform.Find("TextWithIcon/TitleText/DetailedText/AUTHOR").GetComponent<LeanLocalToken>();
				token.SetValue(curPRList.Author);

				Transform targetText = curPRList.obj.transform.Find("TextWithIcon/TitleText");
				targetText.GetComponent<Text>().text = curPRList.PRTitle;
				PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(curPRList.obj, "Tooltip");
				fsm.FsmVariables.GetFsmBool("canEnter").Value = curPRList.canEnter;
			}
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
