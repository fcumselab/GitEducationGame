using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class DialogueSystemManager : SerializedMonoBehaviour
{
    #region Variable
    [SerializeField] bool isFirstDialog = true;
    [SerializeField] string stageType;

    [SerializeField] string selectStageKey;
    [SerializeField] string lastDialogKey;

    [FoldoutGroup("Dialogue")]
    [SerializeField] GameObject responseButtonPanel;
    PlayMakerFSM panelControllerFsm;
    PlayMakerFSM answerButtonDetectorFsm;
    [FoldoutGroup("Dialogue/Button")]
    [SerializeField] Button DialogueButton;
    PlayMakerFSM dialogueButtonFsm;
    PlayMakerFSM highlightParticleFsm;

    [Header("Reference")]
    [SerializeField] PlayMakerFSM StarIcon;
    [SerializeField] DialogueSystemFeatureManager dialogueSystemFeatureManager;
    [SerializeField] PlayMakerFSM ScoreFsm;

    [FoldoutGroup("Web Connection")]
    [SerializeField] EventTrackerTrigger eventTrackerTrigger;
    #endregion

    #region Initialize
    public void InitializeReference(string selectedStageName)
    {
        selectStageKey = selectedStageName;
        selectStageKey = BuildDialogKey();
        InitializeButton();
    }

    public void InitializeButton()
    {
        isFirstDialog = true;
        stageType = StageManager.Instance.GetStageType();
        highlightParticleFsm = MyPlayMakerScriptHelper.GetFsmByName(DialogueButton.gameObject, "Highlight Particle");
        dialogueButtonFsm = MyPlayMakerScriptHelper.GetFsmByName(DialogueButton.gameObject, "Button");
        panelControllerFsm = MyPlayMakerScriptHelper.GetFsmByName(responseButtonPanel.gameObject, "Panel Controller");
        answerButtonDetectorFsm = MyPlayMakerScriptHelper.GetFsmByName(responseButtonPanel.gameObject, "Answer Button Detector");
        DialogueButton.onClick.AddListener(() => ClickButtonActionDialogue());
        DialogueButton.interactable = true;
    }

    string BuildDialogKey()
    {
        string[] splitArr;
        string buildKey = "";
        if (selectStageKey.Contains("Tutorial"))
        {
            splitArr = selectStageKey.Split("(Tutorial)");
            splitArr[0] = splitArr[0].Trim();
            splitArr[0] = splitArr[0].Insert(splitArr[0].Length, "/Tutorial/");
            buildKey = splitArr[0];
        }
        else if (selectStageKey.Contains("Practice"))
        {
            splitArr = selectStageKey.Split("(Practice)");
            splitArr[0] = splitArr[0].Trim();
            splitArr[0] = splitArr[0].Insert(splitArr[0].Length, "/Practice/");
            buildKey = splitArr[0];
        }
        return buildKey;
    }
    #endregion


    public void EnableDialog(string runType)
    {
        if (stageType == "Quiz")
        {
            switch (runType)
            {
                case "Help(Quiz)":
                    DialogueManager.StartConversation(selectStageKey + "Help Selection");
                    break;
                case "StartQuiz":
                    Debug.Log("Let's Start Quiz!!!");
                    QuizGameManager.Instance.StartQuizGame(selectStageKey);
                    break;
                case "First":
                    DialogueManager.StartConversation(selectStageKey + "Start game");
                    SetLastConversationKey(QuestTrackerManager.Instance.GetCurrentQuestNum());
                    break;
                case "End":
                    DialogueManager.StartConversation(selectStageKey + "End");
                    break;
            }
        }
        else if (stageType == "Action")
        {
            switch (runType)
            {
                case "Help":
                    DialogueManager.StartConversation("Common/Help Selection");
                    break;
                case "First":
                    DialogueManager.StartConversation(selectStageKey + "Start game");
                    SetLastConversationKey(QuestTrackerManager.Instance.GetCurrentQuestNum());
                    break;
                case "Quest":
                    int currentQuestNum = QuestTrackerManager.Instance.GetCurrentQuestNum();
                    DialogueManager.StartConversation(selectStageKey + "Quest " + currentQuestNum);
                    SetLastConversationKey(currentQuestNum);
                    break;
                case "End":
                    DialogueManager.StartConversation(selectStageKey + "End");
                    break;
                default:
                    Debug.Log("Not found HelpDialogueSystem Key : " + runType);
                    break;
            }
        }
    }

    //If panelControllerFsm enable -> click reply button will trigger this
    public string HelpDialogAction(string runType)
    {
        if (stageType == "Quiz")
        {
            switch (runType)
            {
                case "Replay":
                    DialogueManager.StopConversation();
                    DialogueManager.StartConversation(selectStageKey + "Start game");
                    DialogueLua.SetVariable("isReplay", true);
                    break;
                case "next":
                    DialogueManager.StopConversation();
                    DialogueManager.StartConversation(selectStageKey + "Start game");
                    panelControllerFsm.FsmVariables.GetFsmString("runType").Value = "Show";
                    panelControllerFsm.enabled = true;
                    DialogueLua.SetVariable("isReplay", false);
                    break;
                case "x":
                case "v":
                    QuizGameManager.Instance.AnswerQuiz(runType);
                    break;
                case "LoadNewQuiz":
                    QuizGameManager.Instance.LoadNewQuiz();
                    break;
                case "Bye":
                    return "End";
            }
        }
        else if(stageType == "Action")
        {
            switch (runType)
            {
                case "Bye":
                    return "End";
                case "Hint":
                case "Answer":
                    PlayMakerFSM.BroadcastEvent("Help Dialogue System/Help Message Popup/Open Popup");
                    break;
                case "Hint(Action)":
                    eventTrackerTrigger.SendEvent("Use Hint Feature", $"{SaveManager.Instance.GetSelectedStageName()}: Quest{QuestTrackerManager.Instance.GetCurrentQuestNum()}");
                    DialogueManager.StopConversation();
                    DialogueManager.StartConversation(selectStageKey + "Hint");
                    StarIcon.FsmVariables.GetFsmString("runType").Value = "usedHint";
                    StarIcon.enabled = true;
                    ScoreFsm.FsmVariables.GetFsmBool("usedHint").Value = true;
                    break;
                case "Answer(Action)":
                    eventTrackerTrigger.SendEvent("Use Answer Feature", $"{SaveManager.Instance.GetSelectedStageName()}: Quest{QuestTrackerManager.Instance.GetCurrentQuestNum()}");
                    DialogueManager.StopConversation();
                    DialogueManager.StartConversation(selectStageKey + "Answer");
                    StarIcon.FsmVariables.GetFsmString("runType").Value = "usedAnswer";
                    StarIcon.enabled = true;
                    ScoreFsm.FsmVariables.GetFsmBool("usedAnswer").Value = true;
                    break;
                case "Current":
                    eventTrackerTrigger.SendEvent("Last Conversation", $"{SaveManager.Instance.GetSelectedStageName()}: Quest{QuestTrackerManager.Instance.GetCurrentQuestNum()}");
                    lastDialogKey = DialogueLua.GetVariable("LastConversationKey").asString;
                    DialogueLua.SetVariable("isReplay", true);
                    DialogueManager.StopConversation();
                    DialogueManager.StartConversation(lastDialogKey);
                    break;
                default:
                    Debug.Log("Not found HelpDialogueSystem Key : " + runType);
                    break;
            }
        }

        return "Continue";
    }



    public void SetLastConversationKey(int currentQuestNum)
    {
        if (currentQuestNum == 1)
        {
            lastDialogKey = (selectStageKey + "Start game");
            DialogueLua.SetVariable("LastConversationKey", lastDialogKey);
        }
        else
        {
            var database = DialogueManager.MasterDatabase;
            lastDialogKey = (selectStageKey + "Quest " + currentQuestNum);
            var conversation = database.GetConversation(lastDialogKey);

            if (conversation != null)
            {
                DialogueLua.SetVariable("LastConversationKey", lastDialogKey);
            }
        }
    }


    #region Button Action
    void ClickButtonActionDialogue()
    {
        //Debug.Log("Click Dialogue");
        if (!DialogueManager.isConversationActive)
        {
            isFirstDialog = dialogueButtonFsm.FsmVariables.GetFsmBool("pressFirstTime").Value;
            if (isFirstDialog)
            {
                switch (stageType)
                {
                    case "Action":
                        Debug.Log("ActionFirst Dialogue");
                        EnableDialog("First");
                        break;
                    case "Quiz":
                        Debug.Log("QuizFirst Dialogue");
                        panelControllerFsm.FsmVariables.GetFsmString("runType").Value = "Show";
                        panelControllerFsm.enabled = true;
                        EnableDialog("First");
                        dialogueButtonFsm.FsmVariables.GetFsmBool("pressFirstTime").Value = false;
                        break;
                }

            }
            else
            {
                Debug.Log("Help Dialogue");
                switch (stageType)
                {
                    case "Action":
                        panelControllerFsm.FsmVariables.GetFsmString("runType").Value = "Show";
                        panelControllerFsm.enabled = true;
                        answerButtonDetectorFsm.SendEvent("Help Dialogue System/Answer/Count Points");
                        EnableDialog("Help");
                        break;
                    case "Quiz":
                        panelControllerFsm.FsmVariables.GetFsmString("runType").Value = "Show";
                        panelControllerFsm.enabled = true;
                        EnableDialog("Help(Quiz)");
                        break;
                }
                
            }
            highlightParticleFsm.SendEvent("Hint/Particle/Close Particle");
        }
    }
    #endregion
}
