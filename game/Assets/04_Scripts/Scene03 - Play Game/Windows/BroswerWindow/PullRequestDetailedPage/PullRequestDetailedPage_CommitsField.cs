using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PullRequestDetailedPage_CommitsField : SerializedMonoBehaviour
{
    [Header("Children")]
    [SerializeField] Text FieldSelectionNumText;

    [Header("Prefab")]
    [SerializeField] GameObject CommitsMsgPrefab;

    [Header("Field")]
    [SerializeField] GameObject TextMessageGroup_Commits;

    [Header("Data")]
    [SerializeField] List<GameObject> ExistCommitsList;

    [Header("Reference")]
    GameObject CommitHistoryWindow;
    GameObject RemoteBranches;
    GameObject RemoteCommits;
    GameObject BaseBranch;
    GameObject CompareBranch;

    string baseBranchName;
    string compareBranchName;


    public void SetPRTargetBranches(string[] branchList)
    {
        baseBranchName = branchList[0];
        compareBranchName = branchList[1];
        InitializeGameObject();
    }

    void InitializeGameObject()
    {
        CommitHistoryWindow = GameObject.Find("CommitHistoryWindow");
        PlayMakerFSM Fsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "Commit History Manager");
        RemoteBranches = Fsm.FsmVariables.GetFsmGameObject("Remote/Branches").Value;
        RemoteCommits = Fsm.FsmVariables.GetFsmGameObject("Remote/Commits").Value;
        BaseBranch = RemoteBranches.transform.Find(baseBranchName).gameObject;
        CompareBranch = RemoteBranches.transform.Find(compareBranchName).gameObject;
    }

    public void UpdateCommitsField()
    {
        string[] resultList = BaseBranch.GetComponent<BranchTool>().CompareTwoCommitList(BaseBranch, CompareBranch);

        if (resultList.Length != ExistCommitsList.Count)
        {
            for (int i = 0; i < resultList.Length; i++)
            {
                if (ExistCommitsList.Count - 1 < i)
                {
                    GameObject cloneObj = Instantiate(CommitsMsgPrefab);
                    cloneObj.name = "CommitMsg";
                    cloneObj.transform.SetParent(TextMessageGroup_Commits.transform);
                    cloneObj.transform.localScale = new(1, 1, 1);

                    Transform Commit = RemoteCommits.transform.Find(resultList[i]);
                    PlayMakerFSM Fsm = MyPlayMakerScriptHelper.GetFsmByName(Commit.gameObject, "Content");
                    Text text = cloneObj.transform.Find("TextBox/Title/TextPanel/AuthorText").GetComponent<Text>();
                    text.text = Fsm.FsmVariables.GetFsmString("commitAuthor").Value;
                    text = cloneObj.transform.Find("TextBox/Title/TextPanel/CommitText").GetComponent<Text>();
                    text.text = Fsm.FsmVariables.GetFsmString("commitId").Value;
                    text = cloneObj.transform.Find("TextBox/Content/CommitMessageText").GetComponent<Text>();
                    text.text = Fsm.FsmVariables.GetFsmString("commitMessage").Value;
                    text = cloneObj.transform.Find("TextBox/Content/TimeText").GetComponent<Text>();
                    text.text = Fsm.FsmVariables.GetFsmString("commitTime").Value;

                    ExistCommitsList.Add(cloneObj);
                }
                else
                {
                    continue;
                }
            }
        }

        //Commits Field Button Switcher NumText
        FieldSelectionNumText.text = $"{resultList.Length}";
    }
}

