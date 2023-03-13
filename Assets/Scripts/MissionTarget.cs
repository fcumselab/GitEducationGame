using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionTarget : MonoBehaviour
{
    private TextMeshProUGUI targetText;

    //Singleton instantation
    private static MissionTarget instance;
    public static MissionTarget Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<MissionTarget>();
            return instance;
        }
    }
    
    void Start()
    {
        targetText = transform.Find("TargetText").GetComponent<TextMeshProUGUI>();
        targetText.text = "Mission Target";
    }

    public void GetCommand(string command)
    {
        targetText.text = command;
    }
}
