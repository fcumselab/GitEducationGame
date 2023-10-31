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

    public void ClearAllCommitList()
    {
        CommitList.Clear();
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

    public void SyncLocalAndRemoteCommitBranchList(GameObject BaseBranch, GameObject TargetBranch)
    {
        //Base = latest BranchList  Target = wanted to sync.
        Transform BaseBranches = BaseBranch.transform.parent;
        Transform TargetBranches = TargetBranch.transform.parent;
        Transform BaseCommitHistory = BaseBranches.transform.parent;
        Transform TargetCommitHistory = TargetBranches.transform.parent;
        Transform BaseCommits = BaseCommitHistory.Find("Commits");
        Transform TargetCommits = TargetCommitHistory.Find("Commits");
        PlayMakerFSM targetFsm;

        List<string> BaseCommitList = BaseBranch.GetComponent<BranchTool>().GetCommitList();
        List<string> TargetCommitList = TargetBranch.GetComponent<BranchTool>().GetCommitList();
        List<string> TargetGenerateCommitList = TargetCommitHistory.GetComponent<CommitTool>().GetGenerateCommitIdList();


        List<string> NeedAddBranchList = new();
        foreach (string baseCommitID in BaseCommitList)
        {
            if(TargetCommitList.Contains(baseCommitID))
            {
                NeedAddBranchList.Add(baseCommitID);
            }
            else
            {
                Transform foundCommit = TargetCommits.Find(baseCommitID);
                //Debug.Log("found: " + foundCommit.name);
                if (foundCommit != null)
                {
                    NeedAddBranchList.Add(baseCommitID);
                    continue;
                }
                //Debug.Log("next: " + baseCommitID);

                Transform CopyCommit = BaseCommits.Find(baseCommitID);
                Transform NewCommit = Instantiate(CopyCommit, TargetCommits);
                NewCommit.name = CopyCommit.name;

                targetFsm = MyPlayMakerScriptHelper.GetFsmByName(NewCommit.gameObject, "Line Generator");
                targetFsm.enabled = true;

                TargetCommitList.Add(baseCommitID);
                TargetGenerateCommitList.Add(baseCommitID);
            }
        }

        UpdateTargetCommitBranchList(NeedAddBranchList, TargetCommits.gameObject);

        PlayMakerFSM targetBranchFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetBranch, "Branch");
        string latestCommit = targetBranchFsm.FsmVariables.GetFsmString("LatestCommit").Value;
        //Debug.Log("targetB latestCommit : " + latestCommit);

        //Set previous TargetBranch's HEAD Commit to false.
        Transform TargetHEADCommit = TargetCommits.Find(latestCommit);
        //Debug.Log("TargetHEADCommit : " + TargetHEADCommit.name);
        targetFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetHEADCommit.gameObject, "Content");
        targetFsm.FsmVariables.GetFsmBool("isLatestCommit").Value = false;

        //Update TargetBranch's latestCommit.
        targetFsm = MyPlayMakerScriptHelper.GetFsmByName(BaseBranch, "Branch");
        targetBranchFsm.FsmVariables.GetFsmString("LatestCommit").Value = targetFsm.FsmVariables.GetFsmString("LatestCommit").Value;


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
                CommitList.Add(TBCommitList[i]);
            }
        }

        UpdateTargetCommitBranchList(NeedUpdateCommitList, Commits);
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
                CommitList.Add(TBCommitList[i]);
            }
        }

        UpdateTargetCommitBranchList(NeedUpdateCommitList, Commits);
    }

    public void RemoveGenerateCommitId(string commitId)
    {
        Transform Branches = transform.parent;
        Transform TargetCommitHistory = Branches.parent;

        TargetCommitHistory.GetComponent<CommitTool>().RemoveGenerateCommitId(commitId);
    }
}
