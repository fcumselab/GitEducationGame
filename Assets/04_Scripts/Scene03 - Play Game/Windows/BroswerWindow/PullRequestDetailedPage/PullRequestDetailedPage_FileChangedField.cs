using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PullRequestDetailedPage_FileChangedField : SerializedMonoBehaviour
{

    [SerializeField] Transform FileChangedTextBoxLocation;

    [Header("Reference")]
    Transform StageManager;
    Transform RepoQuestData;
    Transform RepoQuest_FilesChangedField;

    public void InitializeField()
    {
        StageManager = GameObject.Find("Stage Manager").transform;
        RepoQuestData = StageManager.Find("DefaultData/RepoQuestData");
        RepoQuest_FilesChangedField = RepoQuestData.Find("FilesChangedField");
    }

    public void UpdateFileChangedField(string actionType, int currentQuestNum) {
        
        Debug.Log("File Changed data: " + actionType + " num: " + currentQuestNum);
        if (RepoQuest_FilesChangedField.childCount > 0)
        {
            FileChangedTextBoxGroup TextBox = RepoQuest_FilesChangedField.GetChild(0).GetComponent<FileChangedTextBoxGroup>();
            if (TextBox.ValidNeedRender(actionType, currentQuestNum))
            {
                if(FileChangedTextBoxLocation.childCount != 0)
                {
                    Debug.Log("Delete Old TextBox");
                    Transform LastTextBox = FileChangedTextBoxLocation.transform.GetChild(0);
                    Destroy(LastTextBox.gameObject);
                }
                Debug.Log("A new TextBox");
                    
                TextBox.transform.SetParent(FileChangedTextBoxLocation.transform);
                TextBox.transform.localScale = new(1, 1, 1);
                TextBox.gameObject.SetActive(true);
                TextBox.InitializeContent();
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
