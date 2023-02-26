using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GitCommandController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] TextMeshProUGUI inputFieldText;
    public TMP_InputField tmpInputField;

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

    // Activate the main input field when the scene starts.
    // Start is called before the first frame update
    void Start()
    {
        tmpInputField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            MissionTarget.Instance.getCommand(inputFieldText.text);
            Debug.Log("hello" + inputFieldText.text);
        }
    }
}
