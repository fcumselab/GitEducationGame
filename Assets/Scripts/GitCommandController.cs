using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class GitCommandController : MonoBehaviour
{
    TMP_InputField tmpInputField;
    int historyIndex = -1;

    List<string> gitCommandsDictionary = new List<string>{
        "git",
        "good"
    };

    List<string> gitCommandsDictionary2 = new List<string>{
        "git add",
        "git am",
        "git aply",
        "git askme",
        "git askyou",
        "git commit",
        "git init",
        "git push",
        "git remote"

    };

    List<string> gitCommandsDictionary3 = new List<string>{
        "git remote add",
    };


    [Header("HistoryCommands")]
    List<string> historyCommands = new List<string>();
    [SerializeField] TextMeshProUGUI fieldHistoryCommands;
    [SerializeField] Scrollbar fieldHistoryCommandsScrollbar;

    [Header("GitCommands")]
    [SerializeField] InitCommand initCommand;
    [SerializeField] AddCommand addCommand;

    
    //Singleton instantation
    private static GitCommandController instance;
    public static GitCommandController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GitCommandController>();
            return instance;
        }
    }

    void Start()
    {
        tmpInputField = transform.GetComponent<TMP_InputField>();
        tmpInputField.ActivateInputField();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //MissionTarget.Instance.GetCommand(tmpInputField.text);
            historyCommands.Add(tmpInputField.text);
            AddFieldHistoryCommand(FileManager.Instance.fileLocation + "> " + tmpInputField.text);
            RunCommand(tmpInputField.text);

            fieldHistoryCommandsScrollbar.value = -1;
            tmpInputField.text = "";
            historyIndex = -1;
        }

        /* autocomplete function */
        if (Input.GetKeyDown(KeyCode.Tab)) FindCommand(tmpInputField.text);

        /* history commands system */
        if (Input.GetKeyDown(KeyCode.UpArrow) && historyIndex != 0 && historyCommands.Count != 0)
        {
            if(historyIndex == -1) historyIndex = historyCommands.Count;
            tmpInputField.text = historyCommands[--historyIndex];
            tmpInputField.caretPosition = tmpInputField.text.Length;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && historyIndex != -1 && historyIndex != historyCommands.Count)
        {
            historyIndex++;
            if (historyIndex == historyCommands.Count) tmpInputField.text = "";
            else tmpInputField.text = historyCommands[historyIndex];
            tmpInputField.caretPosition = tmpInputField.text.Length;
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
            else if(ch != ' ') saveCommand += ch;
        }
        if (saveCommand != "") commandList.Add(saveCommand);
        return commandList;
    }


    /*輸入指令時觸發的事件*/
    void RunCommand(string command)
    {
        List<string> commandList = ShortedCommand(command);
        List<string> findList = new List<string>();
        if (commandList.Count > 1)
        {
            findList = gitCommandsDictionary2.FindAll(command => command.Contains(commandList[0] + " " + commandList[1]));
        }

        if (findList.Count == 0 && commandList.Count > 1) AddFieldHistoryCommand("\'" + commandList[1] + "\' is not a git command.");
        else if (findList.Count == 1)
        {
            if (commandList[1] == "init") initCommand.RunCommand(commandList);
            if (commandList[1] == "add") addCommand.RunCommand(commandList);
        }

    }

    /*用來將輸入的指令、找到的指令表顯示在記錄指令欄位*/
    public void AddFieldHistoryCommand(string text)
    {
        fieldHistoryCommands.text += (fieldHistoryCommands.text == "" ? (text) : ('\n' + text));
        fieldHistoryCommands.rectTransform.sizeDelta = new Vector2(fieldHistoryCommands.rectTransform.sizeDelta.x, fieldHistoryCommands.preferredHeight);
        fieldHistoryCommandsScrollbar.value = -1;
    }




    /* AutoComplete Functions Start */
    void AutoCompleteCommand(List<string> findList)
    {
        tmpInputField.text = findList[0];
        tmpInputField.caretPosition = tmpInputField.text.Length;
    }

    void CleanDictionaryCommand(List<string> findList, string keyword = "")
    {
        if (keyword != "")
        {
            for (int i = 0; i < findList.Count; i++)
            {
                findList[i] = findList[i].Replace(keyword, "");
                findList[i] = findList[i].Trim();
            }
        }

        string list = "";

        foreach (var c in findList)
        {
            list += (c + " ");
        }

        AddFieldHistoryCommand(list);
    }

    void FindCommand(string command)
    {
        List<string> commandList = ShortedCommand(command);
        List<string> findList = new List<string>();

        //有空格和沒有空格的需要做判斷
        switch (commandList.Count)
        {
            case 0: //Find empty text
                findList = gitCommandsDictionary;
                if (findList.Count == 1) AutoCompleteCommand(findList);
                else CleanDictionaryCommand(findList);
                break;
            case 1:
                //Find ex: gi
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary.FindAll(command => command.Contains(commandList[0]));
                    if (findList.Count == 1) AutoCompleteCommand(findList);
                    else CleanDictionaryCommand(findList);

                }
                else //Find ex: git_
                {
                    findList = gitCommandsDictionary2.FindAll(command => command.Contains(commandList[0] + " "));
                    if (findList.Count == 1) AutoCompleteCommand(findList);
                    else CleanDictionaryCommand(findList, commandList[0]);
                }

                break;
            case 2:
                //Find ex: git_a
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary2.FindAll(command => command.Contains(commandList[0] + " " + commandList[1]));
                    if (findList.Count == 1) AutoCompleteCommand(findList);
                    else CleanDictionaryCommand(findList, commandList[0] + " ");
                }
                else //Find ex: git_add_
                {
                    findList = gitCommandsDictionary3.FindAll(command => command.Contains(commandList[0] + " " + commandList[1] + " "));
                    if (findList.Count == 1) AutoCompleteCommand(findList);
                    else CleanDictionaryCommand(findList, commandList[0] + " " + commandList[1]);
                }
                break;
            case 3: //Find ex:git_add_rem
                if (command[command.Length - 1] != ' ')
                {
                    findList = gitCommandsDictionary3.FindAll(command => command.Contains(commandList[0] + " " + commandList[1]));
                    if (findList.Count == 1) AutoCompleteCommand(findList);
                    else CleanDictionaryCommand(findList, commandList[0] + " ");
                }
                else //Find ex: git_add_remote_
                {
                    //gitCommandsDictionary4
                }
                break;
            case 4:
                break;
        }
    }
    /* AutoComplete Functions End */

}




