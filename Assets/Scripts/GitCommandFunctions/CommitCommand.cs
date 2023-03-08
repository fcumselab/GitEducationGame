using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitCommand : MonoBehaviour
{
    public void RunCommand(List<string> commandList)
    {
        Debug.Log("run git commit");
        if (commandList.Count > 2)
        {
            if(commandList[2] == "-m" || commandList[2] == "--message")
            {
                Debug.Log(commandList[2]);
            }
        }
        else
        {
            GitCommandController.Instance.AddFieldHistoryCommand("Using -m or --message Commit\n");
        }
    }
}
