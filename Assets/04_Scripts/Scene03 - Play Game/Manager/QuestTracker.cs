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

        InitializeQuestTracker();

        isFinishInitialize = true;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeQuestTracker()
    {
        QuestTrackerParent = transform.parent.gameObject;
        GameManager = QuestTrackerParent.transform.parent.gameObject;
        StageManagerParent = GameManager.transform.Find("Stage Manager Parent").gameObject;

        AlertMsgFileContentWindow = GameObject.Find("AlertMsgFileContentWindow");
        AlertMsgFileManagerWindow = GameObject.Find("AlertMsgFileManagerWindow");


        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Quest Valider");
        fsm.FsmVariables.GetFsmGameObject("Quest Tracker Parent").Value = QuestTrackerParent;

        fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Quest Filter");
        fsm.FsmVariables.GetFsmGameObject("alertFileContentWindow").Value = AlertMsgFileContentWindow;
        fsm.FsmVariables.GetFsmGameObject("alertFileManagerWindow").Value = AlertMsgFileManagerWindow;

        fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Quest Tracker");
        fsm.FsmVariables.GetFsmGameObject("Stage Manager Parent").Value = StageManagerParent;
    }
}
