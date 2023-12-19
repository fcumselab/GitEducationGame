using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilterManager : SerializedMonoBehaviour
{
    [SerializeField] string selectStageName;
    [SerializeField] GameObject QuestTracker;

    [Header("Reference")]
    [SerializeField] GameObject FileContentWindow;
    public GameObject CommandInputField;
    public GameObject CurrentFolderPanel;

    [Header("Get from Sender")]
    public GameObject Sender;
    public string SenderFSMName;
    public int currentQuestNum;

    public string StartQuestFilter()
    {
        string runResult = "";
        if (!QuestTracker)
        {
            CommandInputField = GameObject.Find("CommandInputField");
            QuestTracker = transform.Find("Quest Tracker").gameObject;
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Loading Quest Tracker");
            selectStageName = fsm.FsmVariables.GetFsmString("selectStageName").Value;
        }

        switch (selectStageName)
        {
            case "Game Introduction (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_001_GameIntroduction_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Create Local Repository (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_003_CreateLocalRepository_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Basic Staging Area (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_004_BasicStagingArea_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Advanced Staging Area (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_005_AdvancedStagingArea_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Creating First Version (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_006_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Switching Project Versions (Tutorial)":
                //runResult = QuestTracker.GetComponent<QuestFilter_007_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Git Branching Basics (Tutorial)":
                //runResult = QuestTracker.GetComponent<QuestFilter_008_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Fast-Forward Merging (Tutorial)":
                //runResult = QuestTracker.GetComponent<QuestFilter_009_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Auto Merging (Tutorial)":
                //runResult = QuestTracker.GetComponent<QuestFilter_010_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Merge Conflicts (Tutorial)":
                //runResult = QuestTracker.GetComponent<QuestFilter_011_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Create Remote Repository (Tutorial)":
                //runResult = QuestTracker.GetComponent<QuestFilter_012_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Push to Remote Branches (Tutorial)":
                //runResult = QuestTracker.GetComponent<QuestFilter_013_CreatingFirstVersion_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
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

        return (currentFileName.Contains(copyText)) ? "Continue" : "FileContentWindow/RenameButtonSelection/Wrong FileName";
    }

    public string DetectAction_CopyFile(string wantedVersion)
    {
        Debug.Log("Copy");

        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Sender, SenderFSMName);
        string fileName = fsm.FsmVariables.GetFsmGameObject("ClickedFile").Value.name;

        return (fileName.Contains(wantedVersion)) ? "Continue" : "File/FileFunctionSelection/Wrong FileName";
    }

    public string DetectAction_ModifyFile(string actionType, string wantedFileName)
    {
        if (!FileContentWindow) { FileContentWindow = GameObject.Find("FileContentWindow"); }
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(FileContentWindow, "File Content Window");
        string currentFileName = fsm.FsmVariables.GetFsmString("fileName").Value;

        fsm = MyPlayMakerScriptHelper.GetFsmByName(Sender, SenderFSMName);
        string fileContent = fsm.FsmVariables.GetFsmString("text").Value;
        Debug.Log("modify actionType: " + actionType + "\nFileName: " + wantedFileName + "\ncurrentFileName: " + currentFileName + "\nfileContent: " + fileContent);

        if (currentFileName == wantedFileName)
        {
            if (fileContent.Contains(actionType))
            {
                return "Continue";
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

    public string DetectAction_DeleteFile(string actionType, string wantedFileName)
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
                return "Continue";
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

    public string DetectAction_AddContentFile(string wantedFileName)
    {
        if (!FileContentWindow) { FileContentWindow = GameObject.Find("FileContentWindow"); }
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(FileContentWindow, "File Content Window");
        string currentFileName = fsm.FsmVariables.GetFsmString("fileName").Value;

        fsm = MyPlayMakerScriptHelper.GetFsmByName(Sender, SenderFSMName);
        string fileContent = fsm.FsmVariables.GetFsmString("text").Value;
        Debug.Log("add actionType: " + "\nFileName: " + wantedFileName + "\ncurrentFileName: " + currentFileName + "\nfileContent: " + fileContent);

        return (currentFileName == wantedFileName) ? "Continue" : "FileContentWindow/AddButtonSelection/Wrong FileName";
    }

    public string DetectAction_GitInit(string wantedFolderLocation)
    {
        if (!CurrentFolderPanel)
        {
            CurrentFolderPanel = GameObject.Find("Current Folder Panel");
        }
        string currentFolderLocation = CurrentFolderPanel.transform.parent.name;
        Debug.Log("currentFolderLocation: " + currentFolderLocation + "\nwantedFolderLocation: " + wantedFolderLocation);

        return (currentFolderLocation == wantedFolderLocation) ? "Continue" : "Git Commands/git init/Wrong Location";

    }
}
