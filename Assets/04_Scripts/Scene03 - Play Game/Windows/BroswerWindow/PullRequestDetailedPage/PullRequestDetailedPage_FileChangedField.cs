using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PullRequestDetailedPage_FileChangedField : SerializedMonoBehaviour
{

    [SerializeField] Transform FileChangedTextBoxLocation;


    Transform StageManager;
    Transform RepoQuestData;
    PlayMakerFSM RepoQuestFsm;
    Transform RepoQuest_FilesChangedField;

    public void InitializeField()
    {
        StageManager = GameObject.Find("Stage Manager").transform;
        RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
        RepoQuestFsm = MyPlayMakerScriptHelper.GetFsmByName(RepoQuestData.gameObject, "Repo Quest");
        RepoQuest_FilesChangedField = RepoQuestData.Find("FilesChangedField");
    }

    public void UpdateFileChangedField(string actionType, int currentQuestNum) {
        while (true)
        {
            Debug.Log("File Changed data: " + actionType + " num: " + currentQuestNum);
            if (RepoQuest_FilesChangedField.childCount > 0)
            {
                FileChangedTextBoxGroup TextBox = RepoQuest_FilesChangedField.GetChild(0).GetComponent<FileChangedTextBoxGroup>();
                if (TextBox.ValidNeedRender(actionType, currentQuestNum))
                {
                    Debug.Log("A new TextBox");
                    
                    TextBox.transform.SetParent(FileChangedTextBoxLocation.transform);
                    TextBox.transform.localScale = new(1, 1, 1);
                    TextBox.gameObject.SetActive(true);
                    TextBox.InitializeContent();
                    break;
                }
                else
                {
                    Debug.Log("Found TextBox but not the right one");
                    return;
                }
            }
            else
            {
                Debug.Log("All finish!");
                return;
            }
        }
    }
}
