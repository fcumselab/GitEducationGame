using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCommand : MonoBehaviour
{
    public void RunCommand(List<string> commandList)
    {
        if(commandList[1] == "add")
        {
            if (commandList.Count > 2) FileManager.Instance.FindFile(commandList[2], "add");
            else GitCommandController.Instance.AddFieldHistoryCommand("Nothing specified, nothing added.\n");
        }
        else if(commandList[1] == "reset")
        {
            if (commandList.Count > 2) FileManager.Instance.FindFile(commandList[2], "reset");
            else GitCommandController.Instance.AddFieldHistoryCommand("Not found.\n");
        }
            
    }
}
