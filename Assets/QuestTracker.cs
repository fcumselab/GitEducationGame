using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    [Header("Reference/ExistGameObject")]
    public GameObject StageManagerParent;
    public GameObject QuestTrackerParent;
    public GameObject GameManager;

    [Header("Reference/PrefabGameObject")]
    public GameObject AlertMsgFileContentWindow;
    public GameObject AlertMsgFileManagerWindow;

    [Header("MessageForPlayMaker")]
    public bool isFinishInitialize;

    private void Start()
    {
        isFinishInitialize = false;

        QuestTrackerParent = transform.parent.gameObject;
        GameManager = QuestTrackerParent.transform.parent.gameObject;
        StageManagerParent = GameManager.transform.Find("Stage Manager Parent").gameObject;

        AlertMsgFileContentWindow = GameObject.Find("AlertMsgFileContentWindow");
        AlertMsgFileManagerWindow = GameObject.Find("AlertMsgFileManagerWindow");

        isFinishInitialize = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeQuestTracker()
    {
        /*
        GameObject targetWindow = FsmVariables.GlobalVariables.GetFsmGameObject("Window/FileManagerWindow").Value;
        while (targetWindow == null)
        {
            targetWindow = FsmVariables.GlobalVariables.GetFsmGameObject("Window/FileManagerWindow").Value;
        }
        Transform alertMsg = targetWindow.transform.Find("Alert Message");
        
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Initial");
        fsm.FsmVariables.GetFsmGameObject("alertFileManagerWindow").Value = alertMsg.gameObject;

        targetWindow = FsmVariables.GlobalVariables.GetFsmGameObject("Window/FileContentWindow").Value;
        while (targetWindow == null)
        {
            targetWindow = FsmVariables.GlobalVariables.GetFsmGameObject("Window/FileContentWindow").Value;
        }

        alertMsg = targetWindow.transform.Find("Alert Message");
        fsm.FsmVariables.GetFsmGameObject("alertFileContentWindow").Value = alertMsg.gameObject;*/
    }
}
