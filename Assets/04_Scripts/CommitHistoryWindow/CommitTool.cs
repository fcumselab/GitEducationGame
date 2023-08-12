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

    public Vector2 SetCommitHistoryPanelSize(GameObject commitHistory)
    {
        Dictionary<string, int> branchCountDic = new();

        Transform Commits = commitHistory.transform.Find("Commits");
        for (int x = 0; x < Commits.childCount; x++)
        {
            Transform child = Commits.GetChild(x);

            if (child.gameObject.activeSelf)
            {
                PlayMakerFSM targetFsm = MyPlayMakerScriptHelper.GetFsmByName(child.gameObject, "Content");
                for (int y = 0; y < targetFsm.FsmVariables.GetFsmArray("branchList").Length; y++)
                {
                    string branchName = (string)targetFsm.FsmVariables.GetFsmArray("branchList").Get(y);
                    if (branchCountDic.ContainsKey(branchName))
                    {
                        branchCountDic[branchName]++;
                    }
                    else
                    {
                        branchCountDic.Add(branchName, 1);
                    }
                }
            }
        }
        int maxHeightLen = 0;
        int maxWidthLen = 0;
        foreach (var branch in branchCountDic)
        {
            if (maxHeightLen < branch.Value) maxHeightLen = branch.Value;
        }

        Transform Branches = commitHistory.transform.Find("Branches");
        for (int x = 0; x < Branches.childCount; x++)
        {
            Transform child = Branches.GetChild(x);
            if (child.gameObject.activeSelf)
            {
                PlayMakerFSM targetFsm = MyPlayMakerScriptHelper.GetFsmByName(child.gameObject, "Branch");

                int branchId = targetFsm.FsmVariables.GetFsmInt("branchId").ToInt();
                if (maxWidthLen < branchId) maxWidthLen = branchId;
            }
        }
        maxWidthLen = ((maxWidthLen + 1) * 175);
        maxHeightLen *= 175;
        Vector2 commitHistorySize = new(maxWidthLen, maxHeightLen);
        return commitHistorySize;
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

    public string[] DivideCheckoutCommand(string command)
    {
        List<string> resultList = new();
        int branchLen = -1;
        string keyword = "";
        for (int i = 0; i < command.Length; i++)
        {
            if (command[i] == '^' || command[i] == '~')
            {
                branchLen = i;
                break;
            }
            keyword += command[i];
        }

        if (keyword.Length != 0)
        {
            resultList.Add(keyword);
            keyword = "";
        }

        bool needDivide = false;
        for (int i = branchLen; i < command.Length; i++)
        {
            if ((command[i] == '^' || command[i] == '~') && !needDivide) needDivide = true;
            else if ((command[i] == '^' || command[i] == '~') && needDivide)
            {
                resultList.Add(keyword);
                keyword = "";
            }
            keyword += command[i];
        }

        if (keyword.Length != 0) resultList.Add(keyword);

        return resultList.ToArray();
    }

    public string[] ValidCheckoutCommand(string[] commandList)
    {
        List<string> resultList = new();
        bool isOk = true;
        for (int i = 1; i < commandList.Length; i++)
        {
            if (commandList[i].Length == 1)
            {
                commandList[i] += "1";
                continue;
            }

            string command = commandList[i][1..];
            if (IsDigitsOnly(command))
            {
                //Debug.Log(command + "-> is Ok");
            }
            else
            {
                //Debug.Log(command + "-> is not Ok");
                isOk = false;
                break;
            }
        }
        if (isOk)
        {
            resultList.Add("true");
            resultList.AddRange(commandList);
        }
        else
        {
            resultList.Add("false");
        }

        return resultList.ToArray();
    }

    bool IsDigitsOnly(string str)
    {
        foreach (char c in str) if (c < '0' || c > '9') return false;
        return true;
    }
}
