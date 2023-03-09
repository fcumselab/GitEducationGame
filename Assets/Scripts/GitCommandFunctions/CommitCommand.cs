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
                if(commandList.Count == 3)
                {
                    GitCommandController.Instance.AddFieldHistoryCommand("Please add a comment\n");
                }
                else if(commandList.Count == 4)
                {
                    if(StageFileManager.Instance.stagedFileLists.Count != 0)
                    {
                        
                    }
                    else GitCommandController.Instance.AddFieldHistoryCommand("No changes added to commit\n");
                }
            }
        }
        else
        {
            GitCommandController.Instance.AddFieldHistoryCommand("Using -m or --message Commit\n");
        }
    }
}
