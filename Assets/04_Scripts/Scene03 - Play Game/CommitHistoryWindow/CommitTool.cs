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

    [SerializeField] List<string> newCommitIdList = new();

    private void Start()
    {
        GetBranchColumn("master");
    }

    public void GenerateNewCommitIDList()
    {
        newCommitIdList = GenerateNewRandomId();
        UpdateCommitIDListBranch();
        UpdateCommitIDListCommit();
        UpdateCommitIDListLines();
        generateCommitIdList = newCommitIdList;
    }

    void UpdateCommitIDListBranch()
    {
        Transform Branches = transform.Find("Branches");
        for (int i = 1; i < Branches.childCount; i++)
        {
            GameObject Branch = Branches.GetChild(i).gameObject;
            //Update LatestCommit in Branch
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Branch, "Branch");
            string latestCommitID = fsm.FsmVariables.GetFsmString("LatestCommit").Value;
            fsm.FsmVariables.GetFsmString("LatestCommit").Value = newCommitIdList[generateCommitIdList.FindIndex((item) => item == latestCommitID)];
            BranchTool branchTool = Branch.GetComponent<BranchTool>();
            for (int n = 0; n < branchTool.CommitList.Count; n++)
            {
                int foundIndex = generateCommitIdList.FindIndex((item) => item == branchTool.CommitList[n]);
                branchTool.CommitList[n] = newCommitIdList[foundIndex];
            }
        }
    }

    void UpdateCommitIDListCommit()
    {
        Transform Commits = transform.Find("Commits");
        for (int i = 1; i < Commits.childCount; i++)
        {
            GameObject Commit = Commits.GetChild(i).gameObject;
            //Update Content in Commit
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Commit, "Content");
            string commitId = fsm.FsmVariables.GetFsmString("commitId").Value;
            fsm.FsmVariables.GetFsmString("commitId").Value = newCommitIdList[generateCommitIdList.FindIndex((item) => item == commitId)];
            Commit.name = fsm.FsmVariables.GetFsmString("commitId").Value;
             
            for (int n = 0; n < fsm.FsmVariables.GetFsmArray("commitParentList").Length; n++)
            {
                commitId = fsm.FsmVariables.GetFsmArray("commitParentList").Get(n).ToString();
                int foundIndex = generateCommitIdList.FindIndex((item) => item == commitId);
                fsm.FsmVariables.GetFsmArray("commitParentList").Set(n, newCommitIdList[foundIndex]);
            }
        }
    }
    void UpdateCommitIDListLines()
    {
        Transform Lines = transform.Find("Lines");
        for (int i = 1; i < Lines.childCount; i++)
        {
            GameObject Line = Lines.GetChild(i).gameObject;
            string lineName = Line.name;
            string[] splitList = lineName.Split("-");
            string newCommitID1 = newCommitIdList[generateCommitIdList.FindIndex((item) => item == splitList[0])];
            string newCommitID2 = newCommitIdList[generateCommitIdList.FindIndex((item) => item == splitList[1])];
            Line.name = $"{newCommitID1}-{newCommitID2}";
        }
    }

    public List<string> GenerateNewRandomId()
    {
        List<string> newCommitIdList = new();
        string key = "0123456789abcdefghijkmnpqrstuvwxyz";
        for (int i = 0; i < generateCommitIdList.Count; i++)
        {
            while (true)
            {
                string result = "";

                for (int k = 0; k < 6; k++)
                {
                    int ran = Random.Range(0, key.Length);
                    result += key[ran];
                }

                if (!generateCommitIdList.Contains(result) && !newCommitIdList.Contains(result))
                {
                    newCommitIdList.Add(result);
                    break;
                }
            }
        }
        return newCommitIdList;
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
                if (maxHeightLen < commitList.Count)
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
