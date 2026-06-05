using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilter_003_CreateLocalRepository_Practice : SerializedMonoBehaviour
{
    [Header("Need Give Values")]
    [SerializeField]
    Dictionary<string, List<int>> actionTagDict = new()
    {
        { "File/FileFunctionSelection", new List<int>() },
        { "FileContentWindow/AddButtonSelection", new List<int>() },
        { "FileContentWindow/RenameButtonSelection", new List<int>() },
        { "FileContentWindow/ModifyButtonSelection", new List<int>() },
        { "FileContentWindow/DeleteButtonSelection", new List<int>() },
    };

    [SerializeField]
    //These action need to be restrict. (ex: git status don't need to, but git init need.)
    Dictionary<string, List<int>> commandActionDict = new()
    {
        { "init", new List<int>() }
    };

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

        string runResult = "";
        //Run Git Command
        if (Sender.CompareTag("Window/CommandLineWindow/InputField"))
        {
            string allCommand = CommandEnterFunction.FsmVariables.GetFsmString("command").Value;
            string commandType = CommandEnterFunction.FsmVariables.GetFsmString("commandType").Value;
            if(commandActionDict.ContainsKey(commandType))
            {
                List<int> commandTypeNumList = commandActionDict[commandType];
                if (commandTypeNumList.FindIndex((num) => num == currentQuestNum) != -1)
                {
                    switch (currentQuestNum)
                    {
                        case 1:
                        case 2:
                            return "Continue";
                        case 3:
                            if (commandType == "init")
                            {
                                return questFilterManager.DetectAction_GitInit(i18nTranslateList[0]);
                            }
                            else
                            {
                                return "Continue";
                            }
                    }
                }
                else
                {
                    return $"Git Commands/git {commandType}/Wrong Quest(Failed)";
                }
            }
            else
            {
                return "Continue";
            }
        }
        else //Other action in File Manager/Content Window
        {
            Debug.Log("other move");

            return $"{Sender.tag}/Wrong Quest";
        }

        return runResult;
    }


}
