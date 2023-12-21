using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GitCommandValider : SerializedMonoBehaviour
{
    [SerializeField]
    List<string> AllCommandList = new()
    {
        "git",
        "git init",
        "git status",
        "git add",
        "git reset",
        "git commit",
        "git log",
        "git checkout",
        "git branch",
        "git merge",
        "git clone",
        "git remote",
        "git push",
        "git pull"
    };

    [SerializeField] Dictionary<string, int> PlayerUnlockDict = new();
    [SerializeField] Dictionary<string, int> CurrentStageUsableDict = new();


    [Header("Reference")]
    [SerializeField] SaveManager saveManager;
    [SerializeField] StageManager stageManager;

    public void InitializeUsableCommandList()
    {
        saveManager = GameObject.Find("Save Manager (Main)").GetComponent<SaveManager>();
        stageManager = GameObject.Find("Stage Manager").GetComponent<StageManager>();

        //Get UsableDict
        CurrentStageUsableDict = stageManager.GetUsedCommandDict();

        List<GameManualItem> commandGameManualDatas = saveManager.GetCommandDataListInGameManual();
        int index = 0;
        foreach(GameManualItem commandData in commandGameManualDatas)
        {
            int foundIndex = commandGameManualDatas.FindIndex((item) => item.listName == AllCommandList[index]);
            PlayerUnlockDict.Add(AllCommandList[index], commandGameManualDatas[foundIndex].listUnlockProgress);
            index++;
        }
    }

    public void UpdatePlayerUnlockDict()
    {
        List<GameManualItem> commandGameManualDatas = saveManager.GetCommandDataListInGameManual();
        for(int i = 0; i< PlayerUnlockDict.Count; i++)
        {
            int foundIndex = commandGameManualDatas.FindIndex((item) => item.listName == AllCommandList[i]);
            PlayerUnlockDict[AllCommandList[i]] = commandGameManualDatas[foundIndex].listUnlockProgress;
        }
    }

    public string CanTargetCommandRun(string targetCommandType, int targetCommandTypeNum)
    {
        //If targetNum = 0 -> command type (git/git add/git reset...)
        bool isCommandType = false;
        
        if(targetCommandTypeNum == 0)
        {
            isCommandType = true;
            targetCommandTypeNum = 1;
        }

        //If player unlock this command
        if (PlayerUnlockDict[targetCommandType] >= targetCommandTypeNum)
        {
            if (CurrentStageUsableDict.ContainsKey(targetCommandType))
            {
                //Can use?
                if(CurrentStageUsableDict[targetCommandType] >= targetCommandTypeNum)
                {
                    return "Success";
                }
                else
                {
                    if (isCommandType)
                    {
                        return $"GitCommandValider/CannotUsedCommandInStage/{targetCommandType}/0";
                    }
                    else
                    {
                        return $"GitCommandValider/CannotUsedCommandInStage/{targetCommandType}/{targetCommandTypeNum}";

                    }
                }
            }
            else
            {
                if (isCommandType)
                {
                    return $"GitCommandValider/CannotUsedCommandInStage/{targetCommandType}/0";
                }
                else
                {
                    return $"GitCommandValider/CannotUsedCommandInStage/{targetCommandType}/{targetCommandTypeNum}";

                }
            }
        }
        else
        {
            if (isCommandType)
            {
                return $"GitCommandValider/LockedCommand/{targetCommandType}/0";
            }
            else
            {
                return $"GitCommandValider/LockedCommand/{targetCommandType}/{targetCommandTypeNum}";
            }
        }
    }
}
