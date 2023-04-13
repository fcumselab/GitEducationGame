using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] string mission;

    [SerializeField] string[] allStep;
    [SerializeField] string nowStep;

    //Singleton instantation
    private static MissionManager instance;
    public static MissionManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<MissionManager>();
            return instance;
        }
    }

    void Start()
    {
        allStep = QuestLog.GetAllQuests(QuestState.Unassigned | QuestState.Active, true, mission);
    }

    public void CheckPoint()
    {
        //targetText.text = command;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("Quest:");

            foreach (var i in allStep)
            {
                Debug.Log(i);
            }
        }
    }
}
