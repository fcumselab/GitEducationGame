using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class GameDataManager : SerializedMonoBehaviour
{
    #region Game Data
    [FoldoutGroup("GameData")]
    [SerializeField] string stageName;
    [FoldoutGroup("GameData")]
    [SerializeField] int lastQuestCompletedTime;
    [FoldoutGroup("GameData")]
    [SerializeField] int QuestCountPerfect { get; set; }
    [FoldoutGroup("GameData")]
    [SerializeField] int QuestCountGood;
    [FoldoutGroup("GameData")]
    [SerializeField] int QuestCountHint;
    [FoldoutGroup("GameData")]
    [SerializeField] int QuestCountAnswer;
    [FoldoutGroup("GameData")]
    [SerializeField] int GameManualUsedTimes;
    [FoldoutGroup("GameData")]
    [SerializeField] int CommandExecuteTime;
    #endregion

    #region Stage Summary Window
    [FoldoutGroup("StageSummaryWindow")]
    [SerializeField] List<string> completeQuestNameList;
    [FoldoutGroup("StageSummaryWindow")]
    [SerializeField] List<int> completeQuestTimeList;
    [FoldoutGroup("StageSummaryWindow")]
    [SerializeField] List<int> completeQuestUsedTimeList;
    [FoldoutGroup("StageSummaryWindow")]
    [SerializeField] List<string> completeQuestTypeList;
    #endregion

    [FoldoutGroup("StageData")]
    [SerializeField] StageData selectedStageData;

    [FoldoutGroup("Web Connection")]
    [SerializeField] EventTrackerTrigger eventTrackerTrigger;

    //Singleton instantation
    private static GameDataManager instance;
    public static GameDataManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameDataManager>();
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

        eventTrackerTrigger.SendEvent($"Complete Quest ({type})", $"{SaveManager.Instance.GetSelectedStageName()}: Quest{QuestTrackerManager.Instance.GetCurrentQuestNum()}");
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

    public void AddGameManualUsedTimes()
    {
        GameManualUsedTimes++;
    }
}

