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

    [Header("MessageForPlayMaker")]
    public bool isFinishInitialize;

    private void Start()
    {
        isFinishInitialize = false;

        InitializeQuestTracker();

        isFinishInitialize = true;

    }

    public void InitializeQuestTracker()
    {
        QuestTrackerParent = transform.parent.gameObject;
        GameManager = QuestTrackerParent.transform.parent.gameObject;
        StageManagerParent = GameManager.transform.Find("Stage Manager Parent").gameObject;

        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Quest Valider");
        fsm.FsmVariables.GetFsmGameObject("Quest Tracker Parent").Value = QuestTrackerParent;

        fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Quest Tracker");
        fsm.FsmVariables.GetFsmGameObject("Stage Manager Parent").Value = StageManagerParent;
    }
}
