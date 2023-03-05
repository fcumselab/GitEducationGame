using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCommand : MonoBehaviour
{
    [SerializeField] GameObject fieldCommit;
    void Start()
    {
        
    }

    public void RunCommand(List<string> commandList)
    {
        if(commandList[1] == "init")
        {
            fieldCommit.SetActive(true);
        }
    }
}