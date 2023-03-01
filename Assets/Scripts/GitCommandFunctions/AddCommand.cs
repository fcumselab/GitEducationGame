using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCommand : MonoBehaviour
{
    void Start()
    {
        
    }

    public void RunCommand(List<string> commandList)
    {
        Debug.Log("run git add");
        FileManager.Instance.FindFile(commandList[2]);
    }
}
