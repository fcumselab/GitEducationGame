using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine;


[Serializable]
public class PlayerSaveData
{
    //遊玩遊戲總記錄
    public GameRecordData gameRecordData = new();
    //單個關卡資料
    public List<StageData> stageData = new();
    //手冊資料，C, RAW, VC
    public List<GameManualData> gameManualData = new(3);
}

[Serializable]
public class GameRecordData
{
    [Header("Stage & Score")]
    public int totalStarCount;
    public int totalStageScore;
    //totalStageProgress
    public int totalPlayTime;
    public int totalTimesStageClear;

    [Header("Help")]
    public int totalTimesUsedGameManual;

    [Header("Command")]
    public int totalRunCommandTimes;
    //totalTimesCompleteQuest = Perfect + Good + Hint + Answer
    public int totalTimesQuestClearPerfect;
    public int totalTimesQuestClearGood;
    public int totalTimesQuestClearHint;
    public int totalTimesQuestClearAnswer;
}

[Serializable]
public class StageData
{
    // StageData:
    // stageName:               Game Introduction
    // stageType:               Basic/Branch/Remote
    // isStageUnlock:           true
    // stageClearTimes:         1
    // selfStageLeaderboard:    [{A,3,1000,10:50},{B,2,500,08:22},{C,1,250,04:33}]

    public string stageName;
    public string stageType;
    public bool isStageUnlock;
    public int stageClearTimes;

    //Three player data in this leaderboard
    public List<StageLeaderboardData> stageLeaderboardData = new(3);
    public List<string> nextUnlockStageNameList = new();
}

[Serializable]
public class StageLeaderboardData
{
    public string playerName;
    public int playerStar;
    public int playerScore;
    public int playerClearTime;
}

[Serializable]
public class GameManualData
{
    //manualType:           C -> Command
    //List<GameManualItem>: [{git add, 0}, {git reset, 3}]
    //manualType:           RAW -> RuleAndWindow
    //                      [...]
    //manualType:           VC -> VersionControl
    //                      [...]
    public string manualType;
    public List<GameManualItem> items = new();
}

[Serializable]
public class GameManualItem
{
    // GameManualItem:
    //      listName: git add
    //      listUnlockProgress: 0
    public string listName;
    public int listUnlockProgress;
}

