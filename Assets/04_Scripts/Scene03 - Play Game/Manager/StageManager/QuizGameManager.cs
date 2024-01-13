using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class QuizGameManager : SerializedMonoBehaviour
{
    //Manual count how many question in this quiz dialog
    [SerializeField] int maxQuizNum;

    //How many questions player need to answer.
    [SerializeField] int needAnswerQuizNum;

    [Header("Counting...")]
    [SerializeField] string currentStageKey;
    [SerializeField] int currentQuizNum = 0;
    [SerializeField] List<int> usedNumList = new();
    [SerializeField] bool waitForEndDialog;

    GameObject QuestTrackerParent;
    PlayMakerFSM questTrackerFsm;
    #region instance
    //Singleton instantation
    private static QuizGameManager instance;
    public static QuizGameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<QuizGameManager>();
            return instance;
        }
    }
    #endregion

    public void StartQuizGame(string selectStageKey)
    {
        QuestTrackerParent = GameObject.Find("Quest Tracker Parent");
        questTrackerFsm = MyPlayMakerScriptHelper.GetFsmByName(QuestTrackerParent, "Quest Tracker");
        DialogueSystemFeatureManager.Instance.AddNewRegisterFunctionKey("LoadNewQuiz");
        Lua.RegisterFunction("LoadNewQuiz", this, SymbolExtensions.GetMethodInfo(() => LoadNewQuiz()));
        currentStageKey = selectStageKey;
        currentQuizNum = 0;
        LoadNewQuiz();
    }

    //Get by dialog
    public void LoadNewQuiz()
    {
        Debug.Log("Load New Quiz!");
        currentQuizNum++;
        if (needAnswerQuizNum < currentQuizNum)
        {
            Debug.Log("The Ebd QUize");
            waitForEndDialog = true;
            return;
        }


        DialogueLua.SetVariable("Counter", currentQuizNum);
        int nowQuizNum = RandomQuiz();
        DialogueLua.SetVariable("tag", nowQuizNum.ToString());
        Debug.Log("New Quiz: " + nowQuizNum);

        questTrackerFsm.SendEvent("Quest Tracker/Add New Quest");
        DialogueManager.StopConversation();
        DialogueManager.StartConversation(currentStageKey + "Quiz");
    }

    //Get by DialogueSystemManager
    public void AnswerQuiz(string answerResult)
    {
        switch (answerResult)
        {
            case "x":
                PlayMakerFSM.BroadcastEvent("Score Panel/Quiz/Miss");
                //use Answer
                break;
            case "v":
                PlayMakerFSM.BroadcastEvent("Score Panel/Quiz/Success");
                PlayMakerFSM.BroadcastEvent("Quest Tracker/Complete Quest");
                //get score
                break;
        }
    }

    int RandomQuiz()
    {
        int newNum = 0;
        while (true)
        {
            newNum = Random.Range(1, maxQuizNum + 1);
            if (!usedNumList.Contains(newNum))
            {
                usedNumList.Add(newNum);
                return newNum;
            }
        }
    }

    private void Update()
    {
        if (waitForEndDialog)
        {
            Debug.Log("Wait for finish...");
            if (!DialogueManager.IsConversationActive)
            {
                Debug.Log("Wfinish!!!");

                waitForEndDialog = false;
                PlayMakerFSM.BroadcastEvent("Stage Manager/Start Count Result");
            }
        }
    }
}
