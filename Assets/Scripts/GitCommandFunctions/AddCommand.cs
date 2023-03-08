using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCommand : MonoBehaviour
{
    public void RunCommand(List<string> commandList)
    {
        if(commandList.Count > 2)
        {
            FileManager.Instance.FindFile(commandList[2]);
        }
        else
        {
            GitCommandController.Instance.AddFieldHistoryCommand("Nothing specified, nothing added.\n");
        }
    }
}
