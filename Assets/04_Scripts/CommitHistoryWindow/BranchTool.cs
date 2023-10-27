using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class BranchTool : SerializedMonoBehaviour
{
    [SerializeField] List<string> CommitList = new();

    public List<string> GetCommitList()
    {
        return CommitList;
    }

    public string[] GetCommitListToPlayMaker(bool needReverse)
    {
        List<string> CL = CommitList;
        if (needReverse) CL.Reverse();
        return CL.ToArray();
    }

    public void AddCommit(string commitId)
    {
        CommitList.Add(commitId);
    }

    public void RemoveCommit(string commitId)
    {
        CommitList.Remove(commitId);
    }

    public bool RemoveThisBranch(GameObject Commits)
    {
        foreach (string commitID in CommitList)
        {

            Transform foundCommit = Commits.transform.Find(commitID);
            PlayMakerFSM targetFsm = MyPlayMakerScriptHelper.GetFsmByName(foundCommit.gameObject, "Content");

            List<string> branchList = new();
            for (int y = 0; y < targetFsm.FsmVariables.GetFsmArray("branchList").Length; y++)
            {
                string branchName = (string)targetFsm.FsmVariables.GetFsmArray("branchList").Get(y);

                if (name == branchName)
                {
                    branchList.Add(branchName);
                    break;
                }
                else branchList.Add(branchName);
            }

            if (branchList.Contains(name)) continue;
            else targetFsm.FsmVariables.GetFsmArray("branchList").InsertItem(name, branchList.Count);
        }

        return true;
    }

    public bool UpdateCommitBranchList(GameObject Commits)
    {
        UpdateTargetCommitBranchList(CommitList, Commits);
        return true;
    }

    void UpdateTargetCommitBranchList(List<string> TargetCommitList, GameObject Commits)
    {
        foreach (string commitID in TargetCommitList)
        {
            // if a commit's branchList already has thisGameObject's name. 
            // true -> skip / false -> give this commit's branchList this name 
            Transform foundCommit = Commits.transform.Find(commitID);
            PlayMakerFSM targetFsm = MyPlayMakerScriptHelper.GetFsmByName(foundCommit.gameObject, "Content");

            List<string> branchList = new();
            for (int y = 0; y < targetFsm.FsmVariables.GetFsmArray("branchList").Length; y++)
            {
                string branchName = (string)targetFsm.FsmVariables.GetFsmArray("branchList").Get(y);

                if (name == branchName)
                {
                    branchList.Add(branchName);
                    break;
                }
                else branchList.Add(branchName);
            }

            if (branchList.Contains(name)) continue;
            else targetFsm.FsmVariables.GetFsmArray("branchList").InsertItem(name, branchList.Count);
        }
    }


    public bool UpdateCommitsColor(string branchName, Color TextColor, Color ImageColor)
    {
        /*
        Image image;
        Text text;*/
        return true;
    }

    public string FindMergeType(GameObject TargetBranch)
    {   // current master C8  keyword newF C2 (A > B)
        // current master C2  keyword newF C8 (A < B)
        // current master C8  keyword newF D2 (A < B)
        // current master C2  keyword newF D8 (A > B)
        List<string> TBCommitList = TargetBranch.GetComponent<BranchTool>().GetCommitList();

        if(TBCommitList.Count > CommitList.Count)
        {
            string latestCommitID = CommitList[CommitList.Count - 1];
            if (TBCommitList.Contains(latestCommitID)) return "FastForward";
            else return "AutoMerge";
        }
        else
        {
            string latestCommitID = TBCommitList[TBCommitList.Count - 1];
            if (CommitList.Contains(latestCommitID)) return "UpToDate";
            else return "AutoMerge";
        }
    }

    public void FastForwardMerge(GameObject TargetBranch, GameObject Commits)
    {   
        List<string> TBCommitList = TargetBranch.GetComponent<BranchTool>().GetCommitList();
        List<string> NeedUpdateCommitList = new();
        for (int i = 0; i< TBCommitList.Count;i++)
        {
            if (!CommitList.Contains(TBCommitList[i]))
            {
                NeedUpdateCommitList.Add(TBCommitList[i]);
            }
        }

        UpdateTargetCommitBranchList(NeedUpdateCommitList, Commits);
        
        foreach(string commitID in NeedUpdateCommitList)
        {
            CommitList.Add(commitID);
        }
    }

    public void AutoMergeMerge(GameObject TargetBranch, GameObject Commits)
    {
        List<string> TBCommitList = TargetBranch.GetComponent<BranchTool>().GetCommitList();
        List<string> NeedUpdateCommitList = new();

        for (int i = 0; i < TBCommitList.Count; i++)
        {
            if (!CommitList.Contains(TBCommitList[i]))
            {
                NeedUpdateCommitList.Add(TBCommitList[i]);
            }
        }

        UpdateTargetCommitBranchList(NeedUpdateCommitList, Commits);

        foreach (string commitID in NeedUpdateCommitList)
        {
            CommitList.Add(commitID);
        }
    }
}
