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
	Transform StageManager;
	Transform RepoQuestData;
	PlayMakerFSM RepoQuestFsm;
	PlayMakerFSM RepoQuestRemovePRListFsm;

	public void EnablePage()
    {
		if (isInitial)
		{
			//Get All values	
			StageManager = GameObject.Find("Stage Manager").transform;

			//Only Stage like Create/Review PR will use them.
			RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
			if (RepoQuestData)
			{
				RepoQuestFsm = MyPlayMakerScriptHelper.GetFsmByName(RepoQuestData.gameObject, "Repo Quest");
				RepoQuestRemovePRListFsm = MyPlayMakerScriptHelper.GetFsmByName(RepoQuestData.gameObject, "RemoveExistPRList");
			}

			UpdatePRList();
			isInitial = false;
		}
		else if (needUpdate)
		{
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

				//#{PRNUM} Pull Request creator:
				LeanLocalToken token = curPRList.obj.transform.Find("TextWithIcon/Content/TextPanel/DetailedText/PRNUM").GetComponent<LeanLocalToken>();
				token.SetValue(curPRList.PRID);

				Transform targetText = curPRList.obj.transform.Find("TextWithIcon/Content/Title/Text");
				targetText.GetComponent<LeanLocalizedText>().TranslationName = curPRList.PRTitle;

				targetText = curPRList.obj.transform.Find("TextWithIcon/Content/TextPanel/Author");
				targetText.GetComponent<LeanLocalizedText>().TranslationName = curPRList.Author;

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

	public void MoveOpenPRListToClose()
    {
		PRList foundItem = OpenPRList.Find((item) => item.canEnter == true);
		foundItem.obj.transform.SetParent(ClosedPRList.transform);
		RepoQuestRemovePRListFsm.enabled = true;
		foundItem.obj.name = "ClosedPRItem";
		ClosePRList.Add(foundItem);
		OpenPRList.Remove(foundItem);
		UpdatePRListCountText();
	}
}
