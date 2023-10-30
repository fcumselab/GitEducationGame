using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;

public class CommitTool : SerializedMonoBehaviour
{
    [SerializeField] List<string> generateCommitIdList = new();
    [SerializeField] Dictionary<string, int> branchColumn = new();
    [SerializeField] int currentBranchColumnCount = 0;

    private void Start()
    {
        GetBranchColumn("master");
        
    }

    public int GetGenerateCommitIdListSize()
    {
        return generateCommitIdList.Count;
    }

    public List<string> GetGenerateCommitIdList()
    {
        return generateCommitIdList;
    }

    public void RemoveGenerateCommitId(string commitId)
    {
        generateCommitIdList.Remove(commitId);
    }

    public string SetRandomId()
    {
        string key = "0123456789abcdefghijkmnpqrstuvwxyz";
        while (true)
        {
            string result = "";

            for (int i = 0; i < 6; i++)
            {
                int ran = Random.Range(0, key.Length);
                result += key[ran];
            }

            if (!generateCommitIdList.Contains(result))
            {
                generateCommitIdList.Add(result);
                return result;
            }
        }
    }

    public int GetCommitHistoryPanelSizeY()
    {
        int maxHeightLen = 0;

        Transform Branches = gameObject.transform.Find("Branches");
        for (int x = 0; x < Branches.childCount; x++)
        {
            Transform child = Branches.GetChild(x);
            if (child.gameObject.activeSelf)
            {
                List<string> commitList = child.GetComponent<BranchTool>().GetCommitList();
                if(maxHeightLen < commitList.Count)
                {
                    maxHeightLen = commitList.Count;
                }
            }
        }
        maxHeightLen *= 175;
        return maxHeightLen;
    }

    public int GetBranchColumn(string currentBranch)
    {
        if (!branchColumn.ContainsKey(currentBranch))
        {
            branchColumn.Add(currentBranch, currentBranchColumnCount);
            currentBranchColumnCount++;
        }

        return branchColumn[currentBranch];
    }

    public bool UpdateBranchColumn(string deleteBranch, string replaceBranch = "")
    {
        //replace branch ex: git checkout -b branchName (now branchName is HEAD)
        if (replaceBranch.Length != 0)
        {
            int value = branchColumn[deleteBranch];
            branchColumn.Add(replaceBranch, value);
            branchColumn.Remove(deleteBranch);
        }
        else //Remove target branch (git branch -d branchName)
        {
            branchColumn.Remove(deleteBranch);
        }
        return true;
    }

    bool IsDigitsOnly(string str)
    {
        foreach (char c in str) if (c < '0' || c > '9') return false;
        return true;
    }
}
