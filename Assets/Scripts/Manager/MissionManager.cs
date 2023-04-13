using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] string mission;

    [SerializeField] string[] allStep;
    [SerializeField] string nowStepName;
    [SerializeField] int nowStepNum;

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
        nowStepNum = 0;
    }

    public void CheckPoint()
    {
        if (mission == "Level1") CheckPointLevel1();
    }

    public void CheckPointLevel1()
    {
        Debug.Log("Check Level1");
        if (nowStepNum == 0)
        {
            if (GitFile.Instance.GetInitial())
            {
                QuestLog.SetQuestState("Level1-1", QuestState.Success);
                StepComplete();
            }
        }else if(nowStepNum == 1)
        {
            if(StageFileManager.Instance.stagedFileLists.Find(file => file.GetName() == "1.txt"))
            {
                QuestLog.SetQuestState("Level1-2", QuestState.Success);
                StepComplete();
            }
        }
    }
    private void StepComplete()
    {
        nowStepNum++;
        Sequencer.Message("Success");
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
