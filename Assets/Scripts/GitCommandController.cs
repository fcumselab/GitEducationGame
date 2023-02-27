using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GitCommandController : MonoBehaviour
{
    TMP_InputField tmpInputField;
    [SerializeField] int historyIndex = -1;
    //Dictionary<string,>
    [Header("Reference")]
    List<string> historyCommands = new List<string>();
    public TextMeshProUGUI fieldHistoryCommands;

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
            //MissionTarget.Instance.getCommand(tmpInputField.text);

            fieldHistoryCommands.text += (fieldHistoryCommands.text == "" ? (tmpInputField.text) : ('\n' + tmpInputField.text));
            historyCommands.Add(tmpInputField.text);
            fieldHistoryCommands.rectTransform.sizeDelta = new Vector2(fieldHistoryCommands.rectTransform.sizeDelta.x, fieldHistoryCommands.preferredHeight);

            tmpInputField.text = "";
            historyIndex = -1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Lines count: " + fieldHistoryCommands.preferredHeight);

        }

        /* history commands system */
        if (Input.GetKeyDown(KeyCode.UpArrow) && historyIndex != 0 && historyCommands.Count != 0)
        {
            if(historyIndex == -1)
            {
                historyIndex = historyCommands.Count;
            }
            tmpInputField.text = historyCommands[--historyIndex];
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && historyIndex != -1 && historyIndex != historyCommands.Count)
        {
            historyIndex++;

            if (historyIndex == historyCommands.Count)
            {
                tmpInputField.text = "";
            }
            else
            {
                tmpInputField.text = historyCommands[historyIndex];
            }
        }
    }
}
