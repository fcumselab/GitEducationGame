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

