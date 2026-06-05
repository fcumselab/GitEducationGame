using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Lean.Localization;


public class ReviewerListPanel : SerializedMonoBehaviour
{
	
	[SerializeField] int showCount = 0;

	[Header("Children")]
	[SerializeField] GameObject ExistReviewerGroup;
	[SerializeField] GameObject SelectReviewerGroup;

	[Header("Reference")]
	[SerializeField] PlayMakerFSM RepoQuestFsm;

    private void Start()
    {
		for (int i = 0; i < ExistReviewerGroup.transform.childCount; i++)
		{
			ExistReviewerGroup.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

    //If clicked button it's not the reviewer, add it to Repo data.
    public void ClickButtonAction(GameObject NameText)
    {
		object[] reviewerList = RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerNameList").Values;
		Debug.Log("Click!!");
		
		for (int i = 0; i < reviewerList.Length; i++)
		{
			if(NameText.GetComponent<LeanLocalizedText>().TranslationName == reviewerList[i].ToString())
            {
				RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerShowList").Set(i, true);
				break;
			}
		}

		UpdateReviewerList();
	}

	//Every time click the reviewer button
	public void UpdateSelectionPopupList()
    {
		for (int i = 0; i < SelectReviewerGroup.transform.childCount; i++)
		{
			SelectReviewerGroup.transform.GetChild(i).gameObject.SetActive(false);
		}

		for (int i=0; i< RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerShowList").Length; i++)
        {
			Transform ReviewerItem = SelectReviewerGroup.transform.GetChild(i);
			ReviewerItem.gameObject.SetActive(true);

			LeanLocalizedText text = ReviewerItem.Find("NameText").GetComponent<LeanLocalizedText>();
			text.TranslationName = RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerNameList").Get(i).ToString();
			GameObject Icon = ReviewerItem.Find("IconPanel").gameObject;
			bool setActive = (RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerShowList").Get(i).ToString() == "True");
			
			Icon.SetActive(setActive);
		}
	}

	//Every time open detailed PR page.
	public void UpdateReviewerList(PlayMakerFSM RepoQuestFsm = null)
	{
        if (!this.RepoQuestFsm)
        {
			this.RepoQuestFsm = RepoQuestFsm;
		}

		int totalShowCount = 0;
		foreach (var isShow in this.RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerShowList").Values)
		{
			if (isShow.ToString() == "True")
			{
				totalShowCount++;
			}
		}

		//Need Update
		if (showCount < totalShowCount)
		{
			int reviewIndex = 0;
			int totalNameListCount = this.RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerNameList").Length;
			for (int i = 0; i < totalNameListCount; i++)
			{
				if (this.RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerShowList").Get(i).ToString() == "True")
				{
					Transform Msg = ExistReviewerGroup.transform.GetChild(reviewIndex);
					LeanLocalizedText text = Msg.Find("ReviewerNameText").GetComponent<LeanLocalizedText>();
					text.TranslationName = this.RepoQuestFsm.FsmVariables.GetFsmArray("ReviewerNameList").Get(i).ToString();
					Msg.gameObject.SetActive(true);
					reviewIndex++;
				}
			}
			showCount = totalShowCount;
		}
	}
}
