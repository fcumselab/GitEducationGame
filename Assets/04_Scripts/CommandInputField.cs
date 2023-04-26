using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class CommandInputField : MonoBehaviour
{
    //[SerializeField] TMP_InputField tmpInputField;
    [SerializeField] InputField tmpInputField;

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
        //if (tmpInputField) tmpInputField.ActivateInputField();
    }
    
    void Update()
    {
        if (false) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (tmpInputField.text.Trim() != "")
                {
                    historyCommands.Add(tmpInputField.text);
                    AddFieldHistoryCommand(FileManager.Instance.fileLocation + "> " + tmpInputField.text);
                    GitCommandController.Instance.RunCommand(tmpInputField.text);

                    fieldHistoryCommandsScrollbar.value = -1;
                    tmpInputField.text = "";
                    historyIndex = -1;
                }
            }

            /* autocomplete function */
            if (Input.GetKeyDown(KeyCode.Tab)) GitCommandController.Instance.FindCommand(tmpInputField.text);

            /* history commands system */

            if (Input.GetKeyDown(KeyCode.UpArrow) && historyIndex != 0 && historyCommands.Count != 0)
            {
                if (historyIndex == -1) historyIndex = historyCommands.Count;
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
        tmpInputField.text = findList[0];
        tmpInputField.caretPosition = tmpInputField.text.Length;
    }

    /*This method use on Keyword Selection Function*/
    public void AutoCompleteCommand(string keyword)
    {
        string[] textSplit = tmpInputField.text.Split(' ');
        string result = "";
        for(int i = 0; i < textSplit.Length - 1;i++)
        {
            result += textSplit[i];
        }

        if(result.Length == 0) tmpInputField.text = keyword;
        else tmpInputField.text = result + ' ' + keyword;

        tmpInputField.caretPosition = tmpInputField.text.Length;
    }

    public void VaildInput()
    {
        tmpInputField.text = Regex.Replace(tmpInputField.text, 
          @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./ ]", "");
    }

    private void OnEnable()
    {
        //if (tmpInputField) tmpInputField.ActivateInputField();
    }
}
