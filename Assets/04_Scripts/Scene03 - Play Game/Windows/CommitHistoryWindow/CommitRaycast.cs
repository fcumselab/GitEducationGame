using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CommitRaycast : SerializedMonoBehaviour
{

    public void HitCommit(GameObject HitCommit, GameObject Commits, GameObject Branches, GameObject CommitHistoryData)
    {
        PlayMakerFSM CommitContentFsm = MyPlayMakerScriptHelper.GetFsmByName(HitCommit, "Content");
        string firstBranchName = CommitContentFsm.FsmVariables.GetFsmArray("branchList").Get(0).ToString();

        Transform Branch = Branches.transform.Find(firstBranchName);
        PlayMakerFSM BranchFsm = MyPlayMakerScriptHelper.GetFsmByName(Branch.gameObject, "Branch");
        string LatestCommitName = BranchFsm.FsmVariables.GetFsmString("LatestCommit").ToString();

        Transform TargetCommit = Commits.transform.Find(LatestCommitName);
        int branchColumnIndex = CommitHistoryData.GetComponent<CommitTool>().GetBranchColumn(firstBranchName);

        //Move Commit
        TargetCommit.GetComponent<CommitRaycast>().MoveCommit(HitCommit, Commits, branchColumnIndex);
    }

    public void MoveCommit(GameObject GoalMoveCommit, GameObject Commits, int branchColumnIndex)
    {
        if (GoalMoveCommit != gameObject)
        {
            PlayMakerFSM CommitContentFsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Content");
            object[] commitParentList = CommitContentFsm.FsmVariables.GetFsmArray("commitParentList").Values;
            foreach(object commit in commitParentList)
            {
                Transform NextMoveCommit = Commits.transform.Find(commit.ToString());
                if(NextMoveCommit != null)
                {
                    NextMoveCommit.GetComponent<CommitRaycast>().MoveCommit(GoalMoveCommit, Commits, branchColumnIndex);
                }
                else
                {
                    Debug.LogWarning("Error!! Commit Parent ID Can not found.");
                }
            }
        }

        RectTransform rect = GetComponent<RectTransform>();
        float xIndex = branchColumnIndex * -150;

        rect.localPosition = new Vector2(xIndex, rect.localPosition.y);

        PlayMakerFSM LineFsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Line Generator");
        LineFsm.FsmVariables.GetFsmString("runType").Value = "position";
        LineFsm.enabled = true;
    }
}
