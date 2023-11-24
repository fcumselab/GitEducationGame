using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class BasicInputField : MonoBehaviour
{
    [SerializeField] InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void ShowPassword(bool needShow)
    {
        if (needShow)
        {
            inputField.inputType = InputField.InputType.Standard;
        }
        else
        {
            inputField.inputType = InputField.InputType.Password;
        }
        inputField.ForceLabelUpdate();
    }
}
