using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class NewCommit : SerializedMonoBehaviour
{
    [Header("References")]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI textBoxId;
    [SerializeField] TextMeshProUGUI textBoxMessage;

    [Header("CommitDatas")]
    [SerializeField] string id;
    [SerializeField] string message;
    [SerializeField] List<NewFile> modifyFileList;
    [SerializeField] Dictionary<string, List<NewFile>> fileLists;
    [SerializeField] Dictionary<string, List<string>> preCommitList = new Dictionary<string, List<string>>();

    public void SetValue(string message, List<NewFile> stageFileList, string preCommitId = "", string nowBranch = "")
    {
        this.message = message;
        textBoxMessage.text = message;

        if(preCommitId != "" && nowBranch != "")
        {
            if (preCommitList.ContainsKey(nowBranch)) preCommitList[nowBranch].Add(preCommitId);
            else
            {
                List<string> newList = new();
                newList.Add(preCommitId);
                preCommitList.Add(nowBranch, newList);
            }
        }
        modifyFileList = new List<NewFile>(stageFileList);
        SetId();
    }

    void SetId()
    {
        id = "";
        string s = "abcdefghjkmnpqrstuvwxy0123456789";
        for (int i = 0; i < 8; i++) id += s[Random.Range(0, s.Length)];
        textBoxId.text = id;
    }

    public string GetId()
    {
        return id;
    }

    public void UpdateSprite(bool isNowCommit)
    {
        if (isNowCommit) icon.sprite = ImageManager.Instance.GetImage("CommitIconFocus");
        else icon.sprite = ImageManager.Instance.GetImage("CommitIconNotFocus");
    }
}
