using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilter_011_MergeConflicts_Tutorial : SerializedMonoBehaviour
{
    [Header("Need Give Values")]
    [SerializeField]
    Dictionary<string, List<int>> actionTagDict = new()
    {
        { "File/FileFunctionSelection", new List<int>() },
        { "FileContentWindow/AddButtonSelection", new List<int>() { } },
        { "FileContentWindow/RenameButtonSelection", new List<int>() },
        { "FileContentWindow/ModifyButtonSelection", new List<int>() },
        { "FileContentWindow/DeleteButtonSelection", new List<int>() { 5, 6 } },
    };

    [SerializeField]
    //These action need to be restrict. (ex: git status don't need to, but git init need.)
    Dictionary<string, List<int>> commandActionDict = new()
    {
        { "add", new() { 3,4,5,6 } },
        { "reset", new() { 3, 4, 5, 6 } },
        { "commit", new() { 7 } },
        { "branch", new() { } },
        { "checkout", new() { 3, 4, 5, 6, 7 } },
        { "merge", new() { 2, 3 } }
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
                    case "add":
                    case "reset":
                        return (foundIndex != -1 && isMergeConflict) ? "Git Commands/git merge/MergeConflict(AddReset)(Warning)" : "Continue";
                    case "commit":
                        //Initialize wantedFileLocationList
                        List<string> wantedFileLocationList = new();
                        if(wantedFileLocationList.Count == 0) wantedFileLocationList = new() { i18nTranslateList[0], i18nTranslateList[1] };
                        Debug.Log("commit: " + foundIndex + "\nis conflict" + isMergeConflict);
                        if (foundIndex != -1 && isMergeConflict)
                        {
                            resultText = questFilterManager.DetectAction_GitCommit(wantedFileLocationList);
                            if (resultText.Contains("Resolved"))
                            {
                                isMergeConflict = false;
                                return "Continue";
                            }
                            else
                            {
                                return resultText;
                            }
                        }
                        else
                        {
                            return "Continue";
                        }
                    case "branch":
                        Debug.Log("branch action: \n command len: " + splitList.Length + "\ntarget branch:" + splitList[2]);
                        switch (splitList.Length)
                        {
                            case 4:
                                //if action is delete branch (git branch -d 'branchName')
                                if (splitList[2] == "-d" || splitList[2] == "--delete")
                                {
                                    return "Git Commands/common/FollowQuest(Warning)";
                                }
                                return "Continue";
                        }
                        return "Continue";
                    case "checkout":
                        Debug.Log("checkout foundIndex: " + foundIndex + "\ncurrentQuestNum: " + currentQuestNum);
                        if (foundIndex != -1 && currentQuestNum == 3) //Give warning (use 'git log' first).
                        {
                            return (isMergeConflict) ? questFilterManager.DetectAction_GitCheckout_InModifyContentQuest(4, isMergeConflict) : "Continue";
                        }
                        else if (foundIndex != -1)
                        {
                            // num 4 is when Quest 7, after resolve merge conflict.
                            return questFilterManager.DetectAction_GitCheckout_InModifyContentQuest(4, isMergeConflict);
                        }
                        else
                        {
                            return "Continue";
                        }
                    case "merge":
                        if (splitList.Length == 3 && foundIndex != -1 && (currentQuestNum == 2 || currentQuestNum == 3)) //Give warning (use 'git log' first).
                        {
                            switch (currentQuestNum)
                            {
                                case 2:
                                    //Fast Forward
                                    resultText = questFilterManager.DetectAction_GitMerge(splitList[2], "master", "add-score-feature", false, isMergeConflict);
                                    break;
                                case 3:
                                    //Merge Conflict
                                    resultText = questFilterManager.DetectAction_GitMerge(splitList[2], "master", "reset-score-feature", true, isMergeConflict);
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
                            return (currentQuestNum == 2 || currentQuestNum == 3) ? "Continue" : "Git Commands/common/FollowQuest(Warning)";
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
                    case 5:
                        if (Sender.CompareTag("FileContentWindow/DeleteButtonSelection"))
                        {
                            return questFilterManager.DetectAction_DeleteFile_ResolveMergeConflict(i18nTranslateList[0]);
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                    case 6:
                        if (Sender.CompareTag("FileContentWindow/DeleteButtonSelection"))
                        {
                            return questFilterManager.DetectAction_DeleteFile_ResolveMergeConflict(i18nTranslateList[1]);
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
