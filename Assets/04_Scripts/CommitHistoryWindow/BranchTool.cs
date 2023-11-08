using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public int GetCommitListSize()
    {
        return CommitList.Count;
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

    List<string> GetCommitList(Transform TargetCommit, GameObject Commits)
    {
        List<string> resultCommitList = new();
        PlayMakerFSM targetFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetCommit.gameObject, "Content");
        resultCommitList.Add(TargetCommit.name);

        for (int y = 0; y < targetFsm.FsmVariables.GetFsmArray("commitParentList").Length; y++)
        {
            string commitID = (string)targetFsm.FsmVariables.GetFsmArray("commitParentList").Get(y);
            List<string> parentList = GetCommitList(Commits.transform.Find(commitID), Commits);
            resultCommitList.AddRange(parentList);
        }

        return resultCommitList;
    }

    public void CreateBranchByCommit(GameObject BaseBranch, GameObject BaseCommit, GameObject Commits)
    {
        List<string> resultCommitList = new();

        List<string> getCommitList = GetCommitList(BaseCommit.transform, Commits);
        List<string> BaseCommitList = BaseBranch.GetComponent<BranchTool>().GetCommitList();
        foreach(string commitID in BaseCommitList)
        {
            if (getCommitList.Contains(commitID) && !resultCommitList.Contains(commitID))
            {
                resultCommitList.Add(commitID);
            }
        }
        UpdateTargetCommitBranchList(resultCommitList, Commits);

        PlayMakerFSM targetBranchFsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Branch");
        targetBranchFsm.FsmVariables.GetFsmString("LatestCommit").Value = BaseCommit.name;

        foreach (string commitID in resultCommitList)
        {
            CommitList.Add(commitID);
        }
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
                if (foundCommit != null) //Commit has created (curB is newF but master has commits)
                {
                    NeedAddBranchList.Add(baseCommitID);
                }
                else
                {
                    Transform CopyCommit = BaseCommits.Find(baseCommitID);
                    CopyCommit.GetComponent<CircleCollider2D>().enabled = false;
                    targetFsm = MyPlayMakerScriptHelper.GetFsmByName(CopyCommit.gameObject, "LockController");
                    targetFsm.FsmVariables.GetFsmBool("isLock").Value = false;
                    targetFsm.enabled = true;

                    Transform NewCommit = Instantiate(CopyCommit, TargetCommits);
                    NewCommit.name = CopyCommit.name;
                    CopyCommit.GetComponent<CircleCollider2D>().enabled = true;

                    targetFsm = MyPlayMakerScriptHelper.GetFsmByName(NewCommit.gameObject, "Line Generator");
                    targetFsm.FsmVariables.GetFsmString("runType").Value = "position";
                    targetFsm.enabled = true;

                    TargetGenerateCommitList.Add(baseCommitID);
                }

                TargetCommitList.Add(baseCommitID);
            }
        }

        UpdateTargetCommitBranchList(NeedAddBranchList, TargetCommits.gameObject);

        PlayMakerFSM targetBranchFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetBranch, "Branch");
        string latestCommit = targetBranchFsm.FsmVariables.GetFsmString("LatestCommit").Value;

        //Set previous TargetBranch's HEAD Commit to false.
        Transform TargetHEADCommit = TargetCommits.Find(latestCommit);
        targetFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetHEADCommit.gameObject, "Content");
        targetFsm.FsmVariables.GetFsmBool("isLatestCommit").Value = false;

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
