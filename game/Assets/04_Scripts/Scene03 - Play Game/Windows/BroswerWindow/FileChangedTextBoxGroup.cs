using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Lean.Localization;
using UnityEngine.UI;
using System;

public class FileChangedTextBoxGroup : SerializedMonoBehaviour
{
    #region Variables
    [FoldoutGroup("Render Whole Msg")]
    [SerializeField] int renderQuestNum;
    [FoldoutGroup("Render Whole Msg")]
    //Refresh -> NPC / ApproveByPlayer -> Player
    [SerializeField] string renderActionType;
	 
    [FoldoutGroup("File Changed TextBox")]
    [SerializeField] GameObject fileChangedTextBoxPrefab;
    [FoldoutGroup("File Changed TextBox")]
    [SerializeField] List<PullRequestDetailed_FileChangedTextBox> fileChangedTextBoxList = new();

	[FoldoutGroup("Output")]
	List<GameObject> PendingReplyMsgList = new();

	#endregion

	public bool ValidNeedRender(string actionType, int currentQuestNum)
    {
        return (renderQuestNum == currentQuestNum && actionType == renderActionType);
    }

    //ValidNeedRender is true, render once.
    public void InitializeContent()
    {
        foreach(var TextBox in fileChangedTextBoxList)
        {
            GameObject newTextBox = Instantiate(fileChangedTextBoxPrefab);
            newTextBox.name = $"FileChange_{TextBox.fileName}";
            newTextBox.transform.SetParent(transform);
            newTextBox.transform.localScale = new(1, 1, 1);

            GameObject TitleText = newTextBox.transform.Find("NamePanel/Text").gameObject;
            GameObject CreateLocation = newTextBox.transform.Find("TextBox").gameObject;
			GameObject PendingReplyMsg = TextBox.InitializeMsg("FileChangedField", TitleText, CreateLocation);
			PendingReplyMsgList.Add(PendingReplyMsg);
        }
    }

	public int GetTextBoxCount()
    {
		return fileChangedTextBoxList.Count;
	}

	public bool CheckAllPendingReplyMsgActive()
    {
		int foundIndex = PendingReplyMsgList.FindIndex((item) => item.activeSelf == false);
		//Not found = true
		return (foundIndex == -1);
	}
}



#region Global Class FileChangedTextBox (Use for PullRequestMsg_FileChanged, and FileChangedTextBoxGroup scripts)
//Will be use in PullRequestDetailed Panel -> FileChangedField & FileChangedMsg in CoversationField
[Serializable]
public class PullRequestDetailed_FileChangedTextBox
{
	[Header("Prefab")]
	[SerializeField] GameObject PrefabHead;
	[SerializeField] GameObject PrefabContent;
	[SerializeField] GameObject PrefabReply;

	[Header("Please Fill in these value (i18n key) so TextBox can be generated.")]
	//These string texts are all i18n key
	public string fileName;
	public List<PullRequestDetailed_FileChanged> fileChanged;
	public List<FileContentTextBox_Reply> ReplyMsgList;

	GameObject TextBoxLocation;
	//generateType -> CoversationField/FileChangedField
	public GameObject InitializeMsg(string generateType, GameObject TitleText, GameObject CloneLocation)
	{
		TextBoxLocation = CloneLocation;
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
		GameObject CloneObj = UnityEngine.Object.Instantiate(PrefabHead);
		CloneObj.transform.SetParent(CloneLocation.transform);
		CloneObj.transform.localScale = new(1, 1, 1);
		CloneObj.name = "FileChangedTextBox (Head)";

		//Content
		foreach (PullRequestDetailed_FileChanged fileContent in fileChanged)
		{
			CloneObj = UnityEngine.Object.Instantiate(PrefabContent);
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
			else if (fileContent.type == "N")
			{
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

			//Button function
			Button button = CloneObj.GetComponent<Button>();
			button.interactable = openScript;
			GameObject currentObj= button.gameObject;
			if (openScript)
            {
				button.onClick.AddListener(() => ClickFileChangedTextBoxItem(currentObj));
			}
		}

		
		if (generateType == "FileChangedField" && ReplyMsgList.Count != 0)
        {
			return ReplyMsgList[0].InitializeReplyPendingMsg(PrefabReply, CloneLocation);
        }
		else //If type is CoversationField
		{
			return null;
        }
	}

	public void ClickFileChangedTextBoxItem(GameObject ClickedItem)
	{
		PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(ClickedItem, "Tooltip");

		int clickIndex = ClickedItem.transform.GetSiblingIndex();
		//Click the last one
		if (clickIndex == TextBoxLocation.transform.childCount - 1)
        {
			fsm.FsmVariables.GetFsmString("runType").Value = "NoChange";
		}
		else
        {
			Transform nextItem = TextBoxLocation.transform.GetChild(clickIndex + 1);
			if (nextItem.CompareTag("PRDetailedMsg/FileChanged"))
			{
				//Pending? Show? Warning?
				if (nextItem.gameObject.activeSelf)
				{
					if (nextItem.Find("Pending").gameObject.activeSelf)
					{
						fsm.FsmVariables.GetFsmString("runType").Value = "Created";
					}
					else
					{
						fsm.FsmVariables.GetFsmString("runType").Value = "NoChange";
					}
				}
				else
				{
					fsm.FsmVariables.GetFsmString("runType").Value = "Success";
					nextItem.gameObject.SetActive(true);
				}
            }
            else
            {
				fsm.FsmVariables.GetFsmString("runType").Value = "NoChange";
			}
		}
	}

	public void UpdateReplyMsg(string actionType, int currentQuestNum, GameObject ReplyMsgLocation)
    {
		List<FileContentTextBox_Reply> foundList = ReplyMsgList.FindAll((Msg) =>
			(Msg.renderQuestNum == currentQuestNum && Msg.renderActionType == actionType && Msg.isRender == false)
		);

		foreach (FileContentTextBox_Reply Msg in foundList)
		{
			Msg.InitializeReplyMsg(PrefabReply, ReplyMsgLocation);
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

[Serializable]
public class FileContentTextBox_Reply
{
	[Header("Render Timing")]
	//Reply -> renderQuest / ReplyPending -> insert Children Index
	public int renderQuestNum;
	//Reply -> Show, NewQuest, Refresh, Reply / ReplyPending -> Done / Warning / Click(Show) / Click(Hide) 
	public string renderActionType;
	public bool isRender;

	[Header("Content")]
	public string author;
	public string replyMsg;

	public void InitializeReplyMsg(GameObject prefab, GameObject ReplyMsgLocation)
	{
		GameObject CloneObj = UnityEngine.Object.Instantiate(prefab);
		CloneObj.transform.SetParent(ReplyMsgLocation.transform);
		CloneObj.transform.localScale = new(1, 1, 1);
		CloneObj.name = $"{renderQuestNum}_{renderActionType}_ReplyMsg";
		CloneObj.SetActive(true);

		CloneObj.transform.Find("Title/TextPanel/AuthorText").GetComponent<LeanLocalizedText>().TranslationName = author;
		CloneObj.transform.Find("Title/TextPanel/TimeText").GetComponent<Text>().text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
		CloneObj.transform.Find("Content/Text").GetComponent<LeanLocalizedText>().TranslationName = replyMsg;

		isRender = true;
	}

	public GameObject InitializeReplyPendingMsg(GameObject prefab, GameObject ReplyMsgLocation)
    {
		GameObject CloneObj = UnityEngine.Object.Instantiate(prefab);
		CloneObj.transform.SetParent(ReplyMsgLocation.transform);
		CloneObj.transform.localScale = new(1, 1, 1);
		CloneObj.name = $"{renderQuestNum}_{renderActionType}_ReplyMsg";

		Transform Panel;
		switch (renderActionType)
        {
			case "Click(Show)":
			case "Click(Hide)":
				Panel = CloneObj.transform.Find("Pending");
				Panel.Find("TextBox/Title/TextPanel/AuthorText").GetComponent<LeanLocalizedText>().TranslationName = author;
				Panel.Find("TextBox/Content/Text").GetComponent<LeanLocalizedText>().TranslationName = replyMsg;
				Panel.gameObject.SetActive(true);
				CloneObj.SetActive(renderActionType.Contains("Show"));
				break;
			case "Done":
				Panel = CloneObj.transform.Find("Done");
				Panel.gameObject.SetActive(true);
				break;
			case "Warning":
				Panel = CloneObj.transform.Find("Warning");
				Panel.gameObject.SetActive(true);
				break;
			default:
				Debug.LogError("Please Add the correct keyword in FileChangedTextBox (ReplyPending)!");
				break;
        }

		CloneObj.transform.SetSiblingIndex(renderQuestNum);
		return CloneObj;
	}
}

#endregion
