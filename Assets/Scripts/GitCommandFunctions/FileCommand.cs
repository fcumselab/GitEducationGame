using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileCommand : MonoBehaviour
{
    public void RunCommand(List<string> commandList)
    {
        if (commandList[1] == "..") FileManager.Instance.PageButtonUp.ClickButton();
        else
        {
            if (commandList.Count == 2)
            {
                FileManager.Instance.FindFile(commandList[1], "cd", FileManager.Instance.fileLocation);
            }
            else GitCommandController.Instance.AddFieldHistoryCommand("Nothing specified, nothing added.\n");
        }
        
    }
}
