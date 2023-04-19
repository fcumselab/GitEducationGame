using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCommand : MonoBehaviour
{
    public void RunCommand(List<string> commandList)
    {
        if(commandList[1] == "add")
        {
            if (commandList.Count > 2) FileManager.Instance.FindFile(commandList[2], "add", FileManager.Instance.fileLocation);
            else CommandInputField.Instance.AddFieldHistoryCommand("Nothing specified, nothing added.\n");
        }
        else if(commandList[1] == "reset")
        {
            if (commandList.Count > 2) FileManager.Instance.FindFile(commandList[2], "reset", FileManager.Instance.fileLocation);
            else CommandInputField.Instance.AddFieldHistoryCommand("Not found.\n");
        }
            
    }
}
