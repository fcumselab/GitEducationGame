using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class GitCommandController : SerializedMonoBehaviour
{
    [Header("GitCommands")]
    [SerializeField] List<string> gitCommandsDictionary = new List<string>{
        "git", "good"
    };

    [SerializeField] List<string> gitCommandsDictionary2 = new List<string>{
        "git add", "git am", "git aply", "git askme", "git askyou",
        "git commit", "git reset",
        "git init",
        "git push",
        "git remote"
    };

    [SerializeField] List<string> gitCommandsDictionary3 = new List<string>{
        "git remote add"
    };

    //Singleton instantation
    private static GitCommandController instance;
    public static GitCommandController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GitCommandController>();
            return instance;
        }
    }

    /*Let commands divide into multiple command, then trim space at the front and back*/
    List<string> ShortedCommand(string command)
    {
        List<string> commandList = new();
        string cleanCommand = command.Trim();
        string saveCommand = "";
        foreach (char ch in cleanCommand)
        {
            if (ch == ' ' && saveCommand != "")
            {
                commandList.Add(saveCommand);
                saveCommand = "";
            }
            else if (ch != ' ') saveCommand += ch;
        }
        if (saveCommand != "") commandList.Add(saveCommand);

        return commandList;
    }

    /*輸入指令時觸發的事件*/
    public string[] RunCommand(string command)
    {

        List<string> commandList = ShortedCommand(command);
        List<string> findList = new();
        List<string> resultList = new();

        if (commandList.Count > 1)
        {
            findList = gitCommandsDictionary2.FindAll(command => command.Equals(commandList[0] + " " + commandList[1]));
        }

        if (findList.Count == 1)
        {
            resultList.Add("findOne");
           

        }else if(findList.Count > 1)
        {
            Debug.Log("Show many result!");
        }else if(findList.Count == 0)
        {
            if (commandList[0] == "git")
            {
                if (commandList.Count == 1) resultList.Add("findZero");
                else
                {
                    if (commandList[1] == "--version" || commandList[1] == "-v") resultList.Add("findOne");
                    else resultList.Add("findZero");
                }
                
            } 
            else
            {
                resultList.Add("findZero");
            }
        }
        resultList.AddRange(commandList);
        return resultList.ToArray();
    }

    string CleanDictionaryCommand(List<string> findList, List<string> commandList, string keyword = "")
    {
        if (findList.Count != 0)
        {   // Find Commands Keywords
            if (keyword != "")
            {
                for (int i = 0; i < findList.Count; i++)
                {
                    findList[i] = findList[i].Replace(keyword, "");
                    findList[i] = findList[i].Trim();
                }
            }

            string list = "";

            foreach (var c in findList) list += (c + " ");

            if (list.Length != 0)
            {
                return list;
            }
        }
        else
        {   
            // Find File Keywords
            /*if (commandList[0] == "cd")
            {
                
            }*/
            if(commandList[0] == "git" && (commandList[1] == "add" || commandList[1] == "reset"))
            {
                return "RunFindFile";
            }
        }

        return "";
    }

    //Pressing tab button or search button
    public string FindCommand(string command)
    {
        List<string> commandList = ShortedCommand(command);
        List<string> findList = new();
        string result = "";
        //有空格和沒有空格的需要做判斷
        switch (commandList.Count)
        {
            case 0: //Find empty text
                findList = gitCommandsDictionary;
                if (findList.Count == 1)
                {
                    CommandLineInputField.Instance.AutoCompleteCommand(findList);
                    result = "AutoCompleted";
                }
                else result = CleanDictionaryCommand(findList, commandList);
                break;
            case 1:
                //Find ex: gi
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary.FindAll(command => command.StartsWith(commandList[0]));
                    if (findList.Count == 1)
                    {
                        CommandLineInputField.Instance.AutoCompleteCommand(findList);
                        result = "AutoCompleted";
                    }
                    else result = CleanDictionaryCommand(findList, commandList);

                }
                else //Find ex: git_
                {
                    findList = gitCommandsDictionary2.FindAll(command => command.StartsWith(commandList[0] + " "));
                    if (findList.Count == 1)
                    {
                        CommandLineInputField.Instance.AutoCompleteCommand(findList);
                        result = "AutoCompleted";
                    }
                    else result = CleanDictionaryCommand(findList, commandList, commandList[0]);
                }

                break;
            case 2:
                //Find ex: git_a
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary2.FindAll(command => command.StartsWith(commandList[0] + " " + commandList[1]));
                    if (findList.Count == 1)
                    {
                        CommandLineInputField.Instance.AutoCompleteCommand(findList);
                        result = "AutoCompleted";
                    }
                    else result = CleanDictionaryCommand(findList, commandList, commandList[0] + " ");
                }
                else //Find ex: git_add_
                {
                    findList = gitCommandsDictionary3.FindAll(command => command.StartsWith(commandList[0] + " " + commandList[1] + " "));
                    if (findList.Count == 1)
                    {
                        CommandLineInputField.Instance.AutoCompleteCommand(findList);
                        result = "AutoCompleted";
                    }
                    else result = CleanDictionaryCommand(findList, commandList, commandList[0] + " " + commandList[1]);
                }
                break;
            case 3: //Find ex:git_remote_ad
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary3.FindAll(command => command.StartsWith(commandList[0] + " " + commandList[1]));
                    if (findList.Count == 1)
                    {
                        CommandLineInputField.Instance.AutoCompleteCommand(findList);
                        result = "AutoCompleted";
                    }
                    else result = CleanDictionaryCommand(findList, commandList, commandList[0] + " ");
                }
                else //Find ex: git_remote_add_
                {
                    //gitCommandsDictionary4
                }
                break;
            case 4:
                break;
        }
        return result;
    }

}




