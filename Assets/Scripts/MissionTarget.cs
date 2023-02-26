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

    // Start is called before the first frame update
    void Start()
    {
        targetText = transform.Find("TargetText").GetComponent<TextMeshProUGUI>();
        targetText.text = "HI";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getCommand(string command)
    {
        targetText.text = command;
    }
}
