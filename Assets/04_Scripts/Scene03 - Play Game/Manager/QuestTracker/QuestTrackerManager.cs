using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestTrackerManager : SerializedMonoBehaviour
{
    string selectedStageName = "";
    [SerializeField] PlayMakerFSM LoadingQuestTrackerFsm;
    [SerializeField] Dictionary<string, GameObject> QuestTrackerDict = new();
    [SerializeField] GameObject selectedQuestTracker;
    [SerializeField] PlayMakerFSM QuestTrackerFsm;
    [SerializeField] PlayMakerFSM QuestValiderFsm;

    [FoldoutGroup("Web Connection")]
    [SerializeField] EventTrackerTrigger eventTrackerTrigger;

    //Singleton instantation
    private static QuestTrackerManager instance;
    public static QuestTrackerManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<QuestTrackerManager>();
            return instance;
        }
    }

    public void Initialize(string selectedStageName)
    {
        if (QuestTrackerDict.ContainsKey(selectedStageName))
        {
            selectedQuestTracker = Instantiate(QuestTrackerDict[selectedStageName]);
            selectedQuestTracker.transform.SetParent(transform);
            selectedQuestTracker.name = "QuestTracker";
            selectedQuestTracker.SetActive(true);
            QuestTrackerFsm = MyPlayMakerScriptHelper.GetFsmByName(selectedQuestTracker, "Quest Tracker");
            QuestValiderFsm = MyPlayMakerScriptHelper.GetFsmByName(selectedQuestTracker, "Quest Valider");

            LoadingQuestTrackerFsm.FsmVariables.GetFsmGameObject("selectedQuestTracker").Value = selectedQuestTracker;
            LoadingQuestTrackerFsm.FsmVariables.GetFsmString("selectStageName").Value = selectedStageName;
            LoadingQuestTrackerFsm.enabled = true;

            GetComponent<QuestFilterManager>().InitializeReference(selectedQuestTracker, selectedStageName);
        }
        else
        {
            Debug.LogError("Not found Target QuestTracker! Please add one.");
        }
    }

    public void AddNewQuest()
    {
        eventTrackerTrigger.SendEvent("Add New Quest", $"Quest{GetCurrentQuestNum()}");
    }
    

public int GetCurrentQuestNum()
    {
        return QuestTrackerFsm.FsmVariables.GetFsmInt("CurrentQuestNum").Value;
    }

    public void RunQuestValider(GameObject Sender, string senderFsmName)
    {
        Debug.Log("Run Valider...");
        QuestValiderFsm.FsmVariables.GetFsmGameObject("Sender").Value = Sender;
        QuestValiderFsm.FsmVariables.GetFsmString("senderName").Value = senderFsmName;
        QuestValiderFsm.enabled = true;
    }
}
