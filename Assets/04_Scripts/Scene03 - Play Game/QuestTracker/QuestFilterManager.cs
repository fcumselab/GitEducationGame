using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilterManager : SerializedMonoBehaviour
{
    [SerializeField] string selectStageName;
    [SerializeField] GameObject QuestTracker;

    [Header("Return Value -> for apply token to Quest Filter Checker FSM")]
    public string token = "";

    [Header("Reference")]
    [SerializeField] GameObject FileContentWindow;
    [SerializeField] GameObject CommitHistoryWindow;
    [SerializeField] GameObject LocalBranches;
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
            CommitHistoryWindow = GameObject.Find("CommitHistoryWindow");
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
                runResult = QuestTracker.GetComponent<QuestFilter_007_SwitchingProjectVersions_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Git Branching Basics (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_008_GitBranchingBasics_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Fast-Forward Merging (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_009_FastForwardMerging_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
                break;
            case "Auto Merging (Tutorial)":
                runResult = QuestTracker.GetComponent<QuestFilter_010_AutoMerging_Tutorial>().StartQuestFilter(Sender, SenderFSMName, currentQuestNum);
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
        Debug.Log("Result : " + runResult);
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

    public string DetectAction_AddContentFile(string wantedFileName, string wantedBranchName = "")
    {
        PlayMakerFSM fsm;
        //If need to detect branchName
        if (wantedBranchName != "")
        {
            if (!CommitHistoryWindow) { CommitHistoryWindow = GameObject.Find("CommitHistoryWindow"); }
            fsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "Commit History");
            string currentBranchName = fsm.FsmVariables.GetFsmString("Local/currentBranch").Value;

            if (currentBranchName.Contains("HEAD"))
            {
                return "FileContentWindow/AddButtonSelection/Detached HEAD";
            }
            else if (currentBranchName != wantedBranchName)
            {
                return "FileContentWindow/AddButtonSelection/Wrong Branch";
            }
        }

        if (!FileContentWindow) { FileContentWindow = GameObject.Find("FileContentWindow"); }
        fsm = MyPlayMakerScriptHelper.GetFsmByName(FileContentWindow, "File Content Window");
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

        return (currentFolderLocation == wantedFolderLocation) ? "Continue" : "Git Commands/git init/Wrong Location(Failed)";
    }

    public string DetectAction_GitDeleteLocalBranch(string playerWantedDeleteBranchName, string wantedBranchName)
    {
        token = "";

        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "Commit History");
        string currentBranchName = fsm.FsmVariables.GetFsmString("Local/currentBranch").Value;
        if (!LocalBranches) { LocalBranches = fsm.FsmVariables.GetFsmGameObject("Local/Branches").Value; }

        //Debug.Log("Delet Local B: " + playerWantedDeleteBranchName + "\nwantedBranchName: " + wantedBranchName + "\ncurrentBranchName" + currentBranchName);
        //If player input can't not find in local branches.
        if (LocalBranches.transform.Find(playerWantedDeleteBranchName) == null)
        {
            return "Continue";
        }
        else
        {
            //input branchName = current branchName (let Branch Command Fsm do action)
            if (playerWantedDeleteBranchName == currentBranchName)
            {
                return "Continue";
            }
            else
            {
                if (playerWantedDeleteBranchName == wantedBranchName)
                {
                    return "Continue";
                }
                else
                {
                    token = playerWantedDeleteBranchName;
                    return "Git Commands/git branch/DoNotDeleteBranch(Failed)";
                }
            }
        }
    }

    public string DetectAction_GitCheckout_InModifyContentQuest(int HEADCreateNum, bool isMergeConflict = false)
    {
        if (!isMergeConflict)
        {
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "Commit History");
            GameObject HEAD = fsm.FsmVariables.GetFsmGameObject("Local/HEAD").Value;

            fsm = MyPlayMakerScriptHelper.GetFsmByName(HEAD, "Content");
            int createdCommitNum = fsm.FsmVariables.GetFsmInt("createdCommitNum").Value;
            if (createdCommitNum == HEADCreateNum)
            {
                return "Git Commands/git checkout/afterCreateCommit(Warning)";
            }
            else
            {
                return "Git Commands/git checkout/afterChangeFiles(Warning)";
            }
        }
        else
        {
            return "Git Commands/git merge/MergeConflict(During)(Warning)";
        }
    }

    public string DetectAction_GitMerge(string enterBranchName, string mergeLocation, string wantedBranchName, bool setMergeConflict)
    {
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "Commit History");
        string currentBranchName = fsm.FsmVariables.GetFsmString("Local/currentBranch").Value;
        if (!LocalBranches) { LocalBranches = fsm.FsmVariables.GetFsmGameObject("Local/Branches").Value; }

        //TODO, has issues.

        //player input "HEAD"
        if (enterBranchName == "HEAD")
        {
            return "Git Commands/git merge/HEADMerge(Warning)";
        }

        //If player enter branch = current branch 
        if (enterBranchName == currentBranchName)
        {
            Debug.Log("enterBranchName == currentBranchName ");

            return "Continue";
        }

        //if player target branch can find
        if (LocalBranches.transform.Find(enterBranchName))
        {
            if (currentBranchName.Contains("HEAD"))
            {
                //If player in HEAD branch
                return "Git Commands/git merge/HEADIsCurrentBranch(Failed)";
            }
            else if ((enterBranchName == wantedBranchName) && (currentBranchName == mergeLocation))
            {
                //Success, run Merge FSM. (IF want to start mergeConflict mode, set setMergeConflict true)
                return (setMergeConflict) ? "Continue(Merge Conflict)" : "Continue";
            }
            else
            {
                //Error
                return "Git Commands/git merge/MergeError(Failed)";
            }
        }
        else
        {
            //Failed, let Merge FSM do its things.
            return "Continue";
        }
    }

    public string DetectAction_GitCreateLocalBranch(string playerTargetBranchName, string wantedCreateLocation, string wantedCreateBranchName)
    {

        token = "";

        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(CommitHistoryWindow, "Commit History");
        string currentBranchName = fsm.FsmVariables.GetFsmString("Local/currentBranch").Value;
        if (!LocalBranches) { LocalBranches = fsm.FsmVariables.GetFsmGameObject("Local/Branches").Value; }

        Debug.Log("currentBranchName :" + currentBranchName + "\nCreate local branch action: " + playerTargetBranchName + "\n wanted create loc : " + wantedCreateLocation + "\n wanted CreateBranchName : " + wantedCreateBranchName);

        //Can find in current branch list. Or player enter the name not same as wanted branch name.
        if (LocalBranches.transform.Find(playerTargetBranchName) || (playerTargetBranchName != wantedCreateBranchName))
        {
            return "Continue";
        }

        //Create location != wanted location.
        if ((currentBranchName != wantedCreateLocation) && (playerTargetBranchName == wantedCreateBranchName))
        {
            token = playerTargetBranchName;
            return "Git Commands/git branch/WrongLocation(Create)(Failed)";
        }
        else
        {
            return "Continue";
        }
    }
}
