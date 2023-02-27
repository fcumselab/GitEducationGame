using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using TMPro;

public class GitCommandController : MonoBehaviour
{
    TMP_InputField tmpInputField;
    [SerializeField] int historyIndex = -1;

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
        "git push"
    };

    List<string> gitCommandsDictionary3 = new List<string>{
        "git add remote",
    };

    [Header("Reference")]
    List<string> historyCommands = new List<string>();
    public TextMeshProUGUI fieldHistoryCommands;
    public Scrollbar fieldHistoryCommandsScrollbar;

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
            RunCommand(tmpInputField.text);

            AddFieldHistoryCommand(tmpInputField.text);
            
            tmpInputField.text = "";
            historyIndex = -1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("test: ");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            FindCommand(tmpInputField.text);

        }

        /* history commands system */
        if (Input.GetKeyDown(KeyCode.UpArrow) && historyIndex != 0 && historyCommands.Count != 0)
        {
            if(historyIndex == -1) historyIndex = historyCommands.Count;
            tmpInputField.text = historyCommands[--historyIndex];
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && historyIndex != -1 && historyIndex != historyCommands.Count)
        {
            historyIndex++;
            if (historyIndex == historyCommands.Count) tmpInputField.text = "";
            else tmpInputField.text = historyCommands[historyIndex];
        }
    }


    /*�N���O�����h��string�A�t�~�h�����O�Y�����Ů�M�N�r�ꤤ�P�ɥX�{���h�ӪŮ�h���@��*/
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


    /*��J���O��Ĳ�o���ƥ�*/
    void RunCommand(string command)
    {
        List<string> commandList = ShortedCommand(command);

        //gitCommandsDictionary[commandList[0]]

        foreach (var a in commandList)
        {
            Debug.Log(a);
        }        
    }

    /*�ΨӱN��J�����O�B��쪺���O����ܦb�O�����O���*/
    void AddFieldHistoryCommand(string text)
    {
        fieldHistoryCommands.text += (fieldHistoryCommands.text == "" ? (text) : ('\n' + text));
        fieldHistoryCommands.rectTransform.sizeDelta = new Vector2(fieldHistoryCommands.rectTransform.sizeDelta.x, fieldHistoryCommands.preferredHeight);
        fieldHistoryCommandsScrollbar.value = 0;
    }





    /* AutoComplete Function Start*/
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

        //���Ů�M�S���Ů檺�ݭn���P�_
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
        Debug.Log("Find");
    }
    /* AutoComplete Function End*/

}




