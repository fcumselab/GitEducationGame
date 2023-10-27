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
        foreach(string commitID in CommitList)
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

    public bool UpdateCommitsColor(string branchName, Color TextColor, Color ImageColor)
    {
        /*
        Image image;
        Text text;
        
        foreach (var commit in CommitList)
        {
            image = commit.Value.transform.Find("Image").GetComponent<Image>();
            image.color = ImageColor;

            text = commit.Value.transform.Find("Text").GetComponent<Text>();
            text.color = TextColor;
            text.text = branchName[0].ToString();
        }*/

        return true;
    }

    public string FindMergeType(GameObject TargetB, string targetBCommit, string commit)
    {   // current master C8  keyword newF C2 (A > B)
        // current master C2  keyword newF C8 (A < B)
        // current master C8  keyword newF D2 (A < B)
        // current master C2  keyword newF D8 (A > B)

        List<string> TargetList = TargetB.GetComponent<BranchTool>().GetCommitList();
        if(TargetList.Count > CommitList.Count)
        {
            if (TargetList.Contains(commit)) return "FastForward";
            else return "Merge";
        }
        else
        {
            if (CommitList.Contains(targetBCommit)) return "UpToDate";
            else return "Merge";
        }
    }
}
