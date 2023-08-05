using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class BranchTool : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, GameObject> CommitDict = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddCommit(string commitId, GameObject Commit)
    {
        CommitDict.Add(commitId, Commit);
        return true;
    }

    public bool UpdateCommitsColor(string branchName, string targetCommit, Color selectedColor, Color grayColor)
    {
        Image image;
        Text text;
        foreach (var commit in CommitDict)
        {
            image = commit.Value.transform.Find("Image").GetComponent<Image>();
            image.color = grayColor;

            text = commit.Value.transform.Find("Text").GetComponent<Text>();
            text.color = selectedColor;
            text.text = branchName[0].ToString();
            //Debug.Log("set: " + commit.Key);
        }

        GameObject TargetCommit = CommitDict[targetCommit];
        image = TargetCommit.transform.Find("Image").GetComponent<Image>();
        image.color = selectedColor;

        text = TargetCommit.transform.Find("Text").GetComponent<Text>();
        text.color = grayColor;
        text.text = branchName[0].ToString();

        return true;
    }
}
