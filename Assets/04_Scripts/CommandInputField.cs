using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;

public class CommandInputField : SerializedMonoBehaviour
{
    //[SerializeField] TMP_InputField tmpInputField;
    [SerializeField] InputField inputField;

    [Header("HistoryCommands")]
    int historyIndex = -1;
    List<string> historyCommands = new List<string>();
    //[SerializeField] TextMeshProUGUI fieldHistoryCommands;
    [SerializeField] Text fieldHistoryCommands;
    [SerializeField] Scrollbar fieldHistoryCommandsScrollbar;

    //Singleton instantation
    private static CommandInputField instance;
    public static CommandInputField Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CommandInputField>();
            return instance;
        }
    }

    void Start()
    {
        inputField = GetComponent<InputField>();
        //if (tmpInputField) tmpInputField.ActivateInputField();
    }
    
    void Update()
    {
        if (false) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (inputField.text.Trim() != "")
                {
                    historyCommands.Add(inputField.text);
                    AddFieldHistoryCommand(FileManager.Instance.fileLocation + "> " + inputField.text);
                    GitCommandController.Instance.RunCommand(inputField.text);

                    fieldHistoryCommandsScrollbar.value = -1;
                    inputField.text = "";
                    historyIndex = -1;
                }
            }
            /* history commands system */

            if (Input.GetKeyDown(KeyCode.UpArrow) && historyIndex != 0 && historyCommands.Count != 0)
            {
                if (historyIndex == -1) historyIndex = historyCommands.Count;
                inputField.text = historyCommands[--historyIndex];
                inputField.caretPosition = inputField.text.Length;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && historyIndex != -1 && historyIndex != historyCommands.Count)
            {
                historyIndex++;
                if (historyIndex == historyCommands.Count) inputField.text = "";
                else inputField.text = historyCommands[historyIndex];
                inputField.caretPosition = inputField.text.Length;
            }
        }
    }

    /*用來將輸入的指令、找到的指令表顯示在記錄指令欄位*/
    public void AddFieldHistoryCommand(string text)
    {
        //WindowManager.Instance.CheckWindowOpen("FileWindow", true);
        fieldHistoryCommands.text += (fieldHistoryCommands.text == "" ? (text) : ('\n' + text));
        fieldHistoryCommands.rectTransform.sizeDelta = new Vector2(fieldHistoryCommands.rectTransform.sizeDelta.x, fieldHistoryCommands.preferredHeight);
        fieldHistoryCommandsScrollbar.value = -1;
    }

    public void AutoCompleteCommand(List<string> findList)
    {
        inputField.text = findList[0];
        inputField.caretPosition = inputField.text.Length;
    }

    /*This method use on Keyword Selection Function*/
    public void AutoCompleteCommand(string keyword)
    {
        string[] textSplit = inputField.text.Split(' ');
        string result = "";
        for(int i = 0; i < textSplit.Length - 1;i++)
        {
            result += textSplit[i] + " ";
        }

        if(result.Length == 0) inputField.text = keyword;
        else inputField.text = result + keyword;

        inputField.caretPosition = inputField.text.Length;
    }

    public void ValidInput()
    {
        if (inputField == null)
        {
            inputField = GetComponent<InputField>();
            Debug.Log(inputField);
        }
        Debug.Log(inputField.text);

        inputField.text = Regex.Replace(inputField.text, 
          @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./ ]", "");
    }

    public void DeselectAllText()
    {
        //inputField.MoveTextEnd();
    }
}
