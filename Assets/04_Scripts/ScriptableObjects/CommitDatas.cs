using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CommitDatas", menuName = "ScriptableObjects/Commit")]
public class CommitDatas : SerializedScriptableObject
{
    [Header("CommitDatas")]
    [SerializeField] string id;
    [SerializeField] string message;
    [SerializeField] string commitTime;
    [SerializeField] List<FileDatas> modifyFileList;
    [SerializeField] Dictionary<string, List<FileDatas>> fileLists;
    [SerializeField] Dictionary<string, List<string>> preCommitList = new Dictionary<string, List<string>>();

    public void InitValue(string message, List<FileDatas> stageFileList, string preCommitId = "", string nowBranch = "")
    {
        this.message = message;
        this.commitTime = DateTime.Now.ToString();

        if (preCommitId != "" && nowBranch != "")
        {
            if (preCommitList.ContainsKey(nowBranch)) preCommitList[nowBranch].Add(preCommitId);
            else
            {
                List<string> newList = new();
                newList.Add(preCommitId);
                preCommitList.Add(nowBranch, newList);
            }
        }
        modifyFileList = new List<FileDatas>(stageFileList);
        SetId();
    }

    void SetId()
    {
        id = "";
        string s = "abcdefghjkmnpqrstuvwxy0123456789";
        for (int i = 0; i < 8; i++) id += s[UnityEngine.Random.Range(0, s.Length)];

    }

    public string GetId()
    {
        return id;
    }

    public string GetMessage()
    {
        return message;
    }

    public string GetCommitTime()
    {
        return commitTime;
    }

    public List<FileDatas> GetModifyFileList()
    {
        return modifyFileList;
    }
}
