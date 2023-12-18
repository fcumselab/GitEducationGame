using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilterManager : SerializedMonoBehaviour
{
    [SerializeField] string selectStageName;
    [SerializeField] GameObject QuestTracker;

    [SerializeField] GameObject FileContentWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string StartQuestFilter()
    {
        string runResult = "";
        if (!QuestTracker)
        {
            QuestTracker = transform.Find("Quest Tracker").gameObject;
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Loading Quest Tracker");
            selectStageName = fsm.FsmVariables.GetFsmString("selectStageName").Value;
        }

        switch (selectStageName) {
            case "Game Introduction (Tutorial)":
                QuestFilter_001_GameIntroduction_Tutorial script = QuestTracker.GetComponent<QuestFilter_001_GameIntroduction_Tutorial>();
                runResult = script.StartQuestFilter();
                break;
            default:
                Debug.Log("Cannot found target Quest Tracker Object !\n" + selectStageName);
                break;
        }

        return runResult;
    }

    public void DetectAction_Files(GameObject Sender)
    {
        if (Sender.CompareTag("File/FileFunctionSelection"))
        {

        }
    }

    public string DetectAction_RenameFile(string copyText)
    {
        Debug.Log("Rename");

        if (!FileContentWindow) { FileContentWindow = GameObject.Find("FileContentWindow"); }
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(FileContentWindow, "File Content Window");
        string currentFileName = fsm.FsmVariables.GetFsmString("fileName").Value;

        return (currentFileName.Contains(copyText)) ? "Success" : "FileContentWindow/RenameButtonSelection/Wrong FileName";
    }

    public string DetectAction_CopyFile(GameObject Sender, string SenderFSMName, string wantedVersion)
    {
        Debug.Log("Copy");

        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Sender, SenderFSMName);
        string fileName = fsm.FsmVariables.GetFsmGameObject("ClickedFile").Value.name;

        return (fileName.Contains(wantedVersion)) ? "Success" : "File/FileFunctionSelection/Wrong FileName";
    }

    public string DetectAction_ModifyFile(GameObject Sender, string SenderFSMName, string actionType, string wantedFileName)
    {
        if (!FileContentWindow) { FileContentWindow = GameObject.Find("FileContentWindow"); }
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(FileContentWindow, "File Content Window");
        string currentFileName = fsm.FsmVariables.GetFsmString("fileName").Value;

        fsm = MyPlayMakerScriptHelper.GetFsmByName(Sender, SenderFSMName);
        string fileContent = fsm.FsmVariables.GetFsmString("text").Value;
        Debug.Log("modify actionType: " + actionType + "\nFileName: " + wantedFileName + "\ncurrentFileName: " + currentFileName + "\nfileContent: " + fileContent);

        if (currentFileName == wantedFileName)
        {
            if (fileContent.Contains(actionType)){
                return "Success";
            }
            else
            {
                return "FileContentWindow/ModifyButtonSelection/Wrong Content";
            }
        }
        else
        {
            return "FileContentWindow/ModifyButtonSelection/Wrong FileName";
        }
    }

    public string DetectAction_DeleteFile(GameObject Sender, string SenderFSMName, string actionType, string wantedFileName)
    {

        if (!FileContentWindow) { FileContentWindow = GameObject.Find("FileContentWindow"); }
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(FileContentWindow, "File Content Window");
        string currentFileName = fsm.FsmVariables.GetFsmString("fileName").Value;

        fsm = MyPlayMakerScriptHelper.GetFsmByName(Sender, SenderFSMName);
        string fileContent = fsm.FsmVariables.GetFsmString("text").Value;
        Debug.Log("Modify actionType: " + actionType + "\nFileName: " + wantedFileName + "\ncurrentFileName: " + currentFileName + "\nfileContent: " + fileContent);


        if (currentFileName == wantedFileName)
        {
            if (fileContent.Contains(actionType))
            {
                return "Success";
            }
            else
            {
                return "FileContentWindow/DeleteButtonSelection/Wrong Content";
            }
        }
        else
        {
            return "FileContentWindow/DeleteButtonSelection/Wrong FileName";
        }
    }
}
