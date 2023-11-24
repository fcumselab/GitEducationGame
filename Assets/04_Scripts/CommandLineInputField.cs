using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;

public class CommandLineInputField : SerializedMonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] InputField inputField;
    void Start()
    {
        
    }

    //Singleton instantation
    private static CommandLineInputField instance;
    public static CommandLineInputField Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CommandLineInputField>();
            return instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        for (int i = 0; i < textSplit.Length - 1; i++)
        {
            result += textSplit[i] + " ";
        }

        if (result.Length == 0) inputField.text = keyword;
        else inputField.text = result + keyword;

        inputField.caretPosition = inputField.text.Length;
    }

    public void ValidText()
    {
        inputField.text = Regex.Replace(inputField.text,
          @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./ ]", "");
    }

    public void ValidTextOnlyLettersAndNumbers()
    {
        inputField.text = Regex.Replace(inputField.text,
          @"[^a-zA-Z0-9]", "");
    }

    public void DeselectAllText()
    {
        // Activate and select the chatInputField to allow the player to just continue typing.
        inputField.ActivateInputField();
        inputField.Select();

        // Start a coroutine to deselect text and move caret to end. 
        // This can't be done now, must be done in the next frame.
        StartCoroutine(MoveTextEnd_NextFrame());
    }

    IEnumerator MoveTextEnd_NextFrame()
    {
        yield return 0; // Skip the first frame in which this is called.
        inputField.MoveTextEnd(false); // Do this during the next frame.
    }
}
