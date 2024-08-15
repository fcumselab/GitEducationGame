using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilter_010_AutoMerging_Practice : SerializedMonoBehaviour
{
    [Header("Need Give Values")]
    [SerializeField]
    Dictionary<string, List<int>> actionTagDict = new()
    {
        { "File/FileFunctionSelection", new List<int>() },
        { "FileContentWindow/AddButtonSelection", new List<int>() { 2 } },
        { "FileContentWindow/RenameButtonSelection", new List<int>() },
        { "FileContentWindow/ModifyButtonSelection", new List<int>() },
        { "FileContentWindow/DeleteButtonSelection", new List<int>() },
    };

    [SerializeField]
    //These action need to be restrict. (ex: git status don't need to, but git init need.)
    Dictionary<string, List<int>> commandActionDict = new()
    {
        { "branch", new() { 2, 6 } },
        { "checkout", new() { 2, 3, 4, 5, 6 } },
        { "merge", new() {4, 5 } }
    };

    [Header("status")]
    [SerializeField] bool isMergeConflict = false;

    [Header("i18n Text -> Get from Filter FSM")]
    [SerializeField] List<string> i18nTranslateList;

    [Header("Reference")]
    [SerializeField] GameObject CommandInputField;
    PlayMakerFSM CommandEnterFunction;

    [SerializeField] QuestFilterManager questFilterManager;

    void Initializei18nList()
    {
        if (i18nTranslateList.Count == 0)
        {
            questFilterManager = transform.parent.GetComponent<QuestFilterManager>();
            CommandInputField = questFilterManager.CommandInputField;
            CommandEnterFunction = MyPlayMakerScriptHelper.GetFsmByName(CommandInputField, "Enter Function");

            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Quest Filter");
            foreach (string text in fsm.FsmVariables.GetFsmArray("i18nTranslateList/Valider").Values)
            {
                i18nTranslateList.Add(text);
            }
        }
    }

    public string StartQuestFilter(GameObject Sender, string SenderFSMName, int currentQuestNum)
    {
        Debug.Log("Start run");
        Initializei18nList();

        //Run Git Command
        if (Sender.CompareTag("Window/CommandLineWindow/InputField"))
        {
            string allCommand = CommandEnterFunction.FsmVariables.GetFsmString("command").Value;
            string commandType = CommandEnterFunction.FsmVariables.GetFsmString("commandType").Value;
            string[] splitList = allCommand.Split(" ");
            if (commandActionDict.ContainsKey(commandType))
            {
                string resultText = "";

                List<int> commandTypeNumList = commandActionDict[commandType];
                int foundIndex = commandTypeNumList.FindIndex((num) => num == currentQuestNum);
                switch (commandType)
                {
                    case "branch":
                        // Debug.Log("branch action: \n command len: " + splitList.Length + "\ntarget branch:" + splitList[2]);
                        switch (splitList.Length)
                        {
                            case 3:
                                //-r or --remote
                                if (splitList[2] == "-r" || splitList[2] == "--remote")
                                {
                                    return "Continue";
                                }
                                else
                                {
                                    return "Git Commands/common/FollowQuest(Warning)";
                                }
                            case 4:
                                //if action is delete branch (git branch -d 'branchName')
                                if (splitList[2] == "-d" || splitList[2] == "--delete")
                                {
                                    switch (currentQuestNum)
                                    {
                                        case 6:
                                            resultText = questFilterManager.DetectAction_GitDeleteLocalBranch(splitList[3], "ui-design");
                                            if (resultText != "Continue")
                                            {
                                                return questFilterManager.DetectAction_GitDeleteLocalBranch(splitList[3], "java-activity");
                                            }
                                            else
                                            {
                                                return resultText;
                                            }
                                        default:
                                            return "Git Commands/common/FollowQuest(Warning)";
                                    }
                                }
                                return "Continue";
                        }
                        return "Continue";
                    case "checkout":
                        Debug.Log("checkout foundIndex: " + foundIndex + "\ncurrentQuestNum: " + currentQuestNum);
                        if (foundIndex != -1 && currentQuestNum == 3) //Give warning (use 'git log' first).
                        {
                            return questFilterManager.DetectAction_GitCheckout_InModifyContentQuest(5, isMergeConflict);
                        }
                        else if (foundIndex != -1) //Not 4 
                        {
                            return "Continue";
                        }
                        else
                        {
                            return "Git Commands/common/FollowQuest(Warning)";
                        }
                    case "merge":
                        //Todo
                        if (splitList.Length == 3 && foundIndex != -1 && (currentQuestNum == 4 || currentQuestNum == 5)) //Give warning (use 'git log' first).
                        {
                            switch (currentQuestNum)
                            {
                                case 4:
                                    //Fast Forward
                                    resultText = questFilterManager.DetectAction_GitMerge(splitList[2],"master", "ui-design", false, isMergeConflict);
                                    break;
                                case 5:
                                    //Auto Merge
                                    resultText = questFilterManager.DetectAction_GitMerge(splitList[2], "master", "java-activity", false, isMergeConflict);
                                    break;
                            }

                            if (resultText.Contains("(Merge Conflict)"))
                            {
                                isMergeConflict = true;
                                return "Continue";
                            }
                            else
                            {
                                return resultText;
                            }
                        }
                        else
                        {
                            return (currentQuestNum == 4 || currentQuestNum == 5) ? "Continue" : "Git Commands/common/FollowQuest(Warning)";
                        }
                    default:
                        return "Continue";
                }
            }
            else
            {
                return "Continue";
            }
        }
        else //Other action in File Manager/Content Window
        {
            List<int> actionTagList = actionTagDict[Sender.tag];
            if (actionTagList.Count == 0 || (actionTagList.FindIndex((num) => num == currentQuestNum) == -1))
            {
                return $"{Sender.tag}/Wrong Quest";
            }
            else
            {
                switch (currentQuestNum)
                {
                    case 2:
                        if (Sender.CompareTag("FileContentWindow/AddButtonSelection"))
                        {
                            return questFilterManager.DetectAction_AddContentFile(i18nTranslateList[0], "java-activity");
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                    default:
                        return $"{Sender.tag}/Wrong Quest";
                }
            }
        }
    }
}