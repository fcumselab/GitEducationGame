using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class GameDataManager : SerializedMonoBehaviour
{
    [Header("GameData")]
    [SerializeField] string stageName;
    [SerializeField] int lastQuestCompletedTime;
    [SerializeField] int QuestCountPerfect { get; set; }
    [SerializeField] int QuestCountGood;
    [SerializeField] int QuestCountHint;
    [SerializeField] int QuestCountAnswer;
    [SerializeField] int GameManualUsedTimes;
    [SerializeField] int CommandExecuteTime;

    [Header("StageSummaryWindow")]
    [SerializeField] List<string> completeQuestNameList;
    [SerializeField] List<int> completeQuestTimeList;
    [SerializeField] List<int> completeQuestUsedTimeList;
    [SerializeField] List<string> completeQuestTypeList;

    [Header("StageData")]
    [SerializeField] StageData selectedStageData;

    //Singleton instantation
    private static GameDataManager instance;
    public static GameDataManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameDataManager>();
            return instance;
        }
    }

    private void Start()
    {
        lastQuestCompletedTime = 0;
        QuestCountPerfect = 0;
        QuestCountGood = 0;
        QuestCountHint = 0;
        QuestCountAnswer = 0;
    }
    //type: self, hint, answer, perfect
    public void AddCompleteQuestData(string questName, float completedTime, string type)
    {
        int completedTimeInt = (int)completedTime;

        completeQuestNameList.Add(questName);
        completeQuestTimeList.Add(completedTimeInt);
        completeQuestUsedTimeList.Add(completedTimeInt - lastQuestCompletedTime);
        completeQuestTypeList.Add(type);

        switch (type)
        {
            case "Perfect":
                QuestCountPerfect++;
                break;
            case "Good":
                QuestCountGood++;
                break;
            case "Hint":
                QuestCountHint++;
                break;
            case "Answer":
                QuestCountAnswer++;
                break;
        }

        lastQuestCompletedTime = completedTimeInt;
    }


    public void GetSelectedStageData(string stageName)
    {
        List<StageData> allStageDataList = SaveManager.Instance.GetStageDataListFromPlayerData();
        selectedStageData = allStageDataList.Find((item) => item.stageName == stageName);
        this.stageName = stageName;
    }
    
    public void UpdateSelfLeaderBoardContent(GameObject ClearTimesContent, GameObject SelfLeaderBoardGroup, int highlightIndex = -1)
    {
        List<StageLeaderboardData> stageLeaderboardData = GetSelfLeaderBoardData();
        ClearTimesContent.GetComponent<Text>().text = selectedStageData.stageClearTimes.ToString();

        int index = 0;
        foreach (StageLeaderboardData item in stageLeaderboardData)
        {
            Transform selfLeaderBoardItem = SelfLeaderBoardGroup.transform.GetChild(index);
            bool hasRecord = (item.playerScore != 0);

            Text text = selfLeaderBoardItem.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
            text.text = $"{index + 1}";

            if (hasRecord)
            {
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text").GetComponent<Text>();
                text.gameObject.SetActive(true);
                text.text = item.playerName;
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text (None)").GetComponent<Text>();
                text.gameObject.SetActive(false);
            }
            else
            {
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text").GetComponent<Text>();
                text.gameObject.SetActive(false);
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text (None)").GetComponent<Text>();
                text.gameObject.SetActive(true);
            }

            Transform Stars = selfLeaderBoardItem.transform.Find("Stars");
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
            fsm.FsmVariables.GetFsmInt("getStarNum").Value = item.playerStar;
            fsm.enabled = true;

            text = selfLeaderBoardItem.transform.Find("Score & Time/Score/Score Text").GetComponent<Text>();
            text.text = $"{item.playerScore}";
            text = selfLeaderBoardItem.transform.Find("Score & Time/Time/Time Text").GetComponent<Text>();
            text.text = MyTimer.Instance.StopWatch(item.playerClearTime);

            if(highlightIndex != -1)
            {
                fsm = MyPlayMakerScriptHelper.GetFsmByName(selfLeaderBoardItem.gameObject, "Highlight TextBox");
                fsm.FsmVariables.GetFsmBool("needHighlight").Value = (index == highlightIndex - 1);
                fsm.enabled = true;
            }

            index++;
        }
    }

    public List<StageLeaderboardData> GetSelfLeaderBoardData()
    {
        return selectedStageData.stageLeaderboardData;
    }

    public void UpdateSelfLeaderBoardData(List<StageLeaderboardData> newData)
    {
        selectedStageData.stageLeaderboardData = newData;
    }

    public List<string> GetCompleteQuestName()
    {
        return completeQuestNameList;
    }

    public List<int> GetCompleteQuestTime()
    {
        return completeQuestTimeList;
    }

    public List<int> GetCompleteQuestUsedTime()
    {
        return completeQuestUsedTimeList;
    }

    public List<string> GetCompleteQuestType()
    {
        return completeQuestTypeList;
    }

    public int GetQuestCountPerfect()
    {
        return QuestCountPerfect;
    }

    public int GetQuestCountGood()
    {
        return QuestCountGood;
    }

    public int GetQuestCountHint()
    {
        return QuestCountHint;
    }

    public int GetQuestCountAnswer()
    {
        return QuestCountAnswer;
    }

    public int GetCommandExecuteTime()
    {
        return CommandExecuteTime;
    }

    public int GetGameManualUsedTimes()
    {
        return GameManualUsedTimes;
    }
    
}

