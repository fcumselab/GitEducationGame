using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GitCommandController : MonoBehaviour
{

    List<string> gitCommandsDictionary = new List<string>{
        "git", "good", "cd"
    };
    List<string> gitCommandsDictionary2 = new List<string>{
        "git add", "git am", "git aply", "git askme", "git askyou",
        "git commit", "git reset",
        "git init",
        "git push",
        "git remote"
    };
    List<string> gitCommandsDictionary3 = new List<string>{
        "git remote add"
    };


    [Header("GitCommands")]
    [SerializeField] GameObject gitCommands;

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

    /*將指令分成多個string，另外去除指令頭尾的空格和將字串中同時出現的多個空格去成一個*/
    List<string> ShortedCommand(string command)
    {
        List<string> commandList = new List<string>();
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
    public void RunCommand(string command)
    {
        List<string> commandList = ShortedCommand(command);
        List<string> findList = new List<string>();
        
        if(commandList[0] == "cd") gitCommands.GetComponent<FileCommand>().RunCommand(commandList);
        else
        {
            if (commandList.Count > 1) findList = gitCommandsDictionary2.FindAll(command => command.Contains(commandList[0] + " " + commandList[1]));
            
            if (findList.Count == 0 && commandList.Count > 1) CommandInputField.Instance.AddFieldHistoryCommand("\'" + commandList[1] + "\' is not a git command.");
            else if (findList.Count == 1)
            {
                if (GitFile.Instance.GetInitial())
                {
                    if (commandList[1] == "init") CommandInputField.Instance.AddFieldHistoryCommand("Already have existing Git repository.\n");
                    else if (commandList[1] == "add" || commandList[1] == "reset") gitCommands.GetComponent<AddCommand>().RunCommand(commandList);
                    else if (commandList[1] == "commit") gitCommands.GetComponent<CommitCommand>().RunCommand(commandList);
                }
                else
                {
                    if (commandList[1] == "init") GitFile.Instance.SetInitial(true);
                    else CommandInputField.Instance.AddFieldHistoryCommand("You don\'t have Git repository. Please create one.\n");
                }
            }
        }


        FileManager.Instance.UpdateFileSystemUI();
        MissionManager.Instance.CheckPoint();
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
                CommandInputField.Instance.AddFieldHistoryCommand(list);
                return list;
            }
        }
        else
        {   // Find File Keywords
            if (commandList[0] == "cd")
            {
                if(commandList.Count == 1) findList = FileManager.Instance.FindFile("cd", "");
                else findList = FileManager.Instance.FindFile("cd", commandList[1]);

                if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
                else CleanDictionaryCommand(findList, commandList, commandList[0]);
            }
            else if(commandList[0] == "git" && (commandList[1] == "add" || commandList[1] == "reset"))
            {
                if (commandList.Count == 2) findList = FileManager.Instance.FindFile(commandList[1], "");
                else findList = FileManager.Instance.FindFile(commandList[1], commandList[2]);

                if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
                else CleanDictionaryCommand(findList, commandList, "git " + commandList[1]);
            }
        }

        return "";
    }

    public void FindCommandTest(string command)
    {
    }
    public string FindCommand(string command)
    {
        List<string> commandList = ShortedCommand(command);
        List<string> findList = new List<string>();
        string result = "";
        //有空格和沒有空格的需要做判斷
        switch (commandList.Count)
        {
            case 0: //Find empty text
                findList = gitCommandsDictionary;
                if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
                else result = CleanDictionaryCommand(findList, commandList);
                break;
            case 1:
                //Find ex: gi
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary.FindAll(command => command.StartsWith(commandList[0]));
                    if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
                    else result = CleanDictionaryCommand(findList, commandList);

                }
                else //Find ex: git_
                {
                    findList = gitCommandsDictionary2.FindAll(command => command.StartsWith(commandList[0] + " "));
                    if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
                    else result = CleanDictionaryCommand(findList, commandList, commandList[0]);
                }

                break;
            case 2:
                //Find ex: git_a
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary2.FindAll(command => command.StartsWith(commandList[0] + " " + commandList[1]));
                    if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
                    else result = CleanDictionaryCommand(findList, commandList, commandList[0] + " ");
                }
                else //Find ex: git_add_
                {
                    findList = gitCommandsDictionary3.FindAll(command => command.StartsWith(commandList[0] + " " + commandList[1] + " "));
                    if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
                    else result = CleanDictionaryCommand(findList, commandList, commandList[0] + " " + commandList[1]);
                }
                break;
            case 3: //Find ex:git_remote_ad
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary3.FindAll(command => command.StartsWith(commandList[0] + " " + commandList[1]));
                    if (findList.Count == 1) CommandInputField.Instance.AutoCompleteCommand(findList);
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




