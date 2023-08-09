using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class BranchTool : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, GameObject> CommitDict = new();

    public Dictionary<string, GameObject> GetCommitDict()
    {
        return CommitDict;
    }

    public bool AddCommit(string commitId, GameObject Commit)
    {
        CommitDict.Add(commitId, Commit);
        return true;
    }

    public bool UpdateCommitsColor(string branchName, string targetCommit, Color TextColor, Color ImageColor, Color grayColor)
    {
        Image image;
        Text text;
        foreach (var commit in CommitDict)
        {
            image = commit.Value.transform.Find("Image").GetComponent<Image>();
            image.color = grayColor;

            text = commit.Value.transform.Find("Text").GetComponent<Text>();
            text.color = TextColor;
            text.text = branchName[0].ToString();
            //Debug.Log("set: " + commit.Key);
        }

        GameObject TargetCommit = CommitDict[targetCommit];
        image = TargetCommit.transform.Find("Image").GetComponent<Image>();
        image.color = ImageColor;

        text = TargetCommit.transform.Find("Text").GetComponent<Text>();
        text.color = grayColor;
        text.text = branchName[0].ToString();

        return true;
    }

    public string FindMergeType(GameObject TargetB, string targetBCommit, string commit)
    {   // current master C8  keyword newF C2 (A > B)
        // current master C2  keyword newF C8 (A < B)
        // current master C8  keyword newF D2 (A < B)
        // current master C2  keyword newF D8 (A > B)

        Dictionary<string, GameObject> TargetDict = TargetB.GetComponent<BranchTool>().GetCommitDict();
        if(TargetDict.Count > CommitDict.Count)
        {
            if (TargetDict.ContainsKey(commit)) return "FastForward";
            else return "Merge";
        }
        else
        {
            if (CommitDict.ContainsKey(targetBCommit)) return "UpToDate";
            else return "Merge";
        }
    }
}
