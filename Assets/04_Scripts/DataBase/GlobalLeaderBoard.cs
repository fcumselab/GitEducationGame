using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine;

//GlobalGameProcessLeaderBoard - 遊戲總進度排行榜
[Serializable]
public class GlobalGameProcessLeaderBoard
{
    string leaderboardType = "GameProcess";
    List<GlobalGameProcessData> globalGameProcessDataList;
} 

[Serializable]
public class GlobalGameProcessData
{
    //使用者名稱: string
    //遊戲總進度: int
    //達成時的遊戲時長: int

    public string playerName;
    public int gameProcess;
    public int playTime;
}


//GlobalTotalScoreLeaderBoard - 遊戲總得分排行榜
[Serializable]
public class GlobalTotalScoreLeaderBoard
{
    string leaderboardType = "TotalScore";
    List<GlobalTotalScoreData> globalTotalScoreDataList;
}

[Serializable]
public class GlobalTotalScoreData
{
    //使用者名稱: string
    //遊戲總得分: int
    //達成時的遊戲時長: int

    public string playerName;
    public int totalScore;
    public int playTime;
}


//GlobalClearStageBestRecord - 通過關卡最佳記錄排行榜
[Serializable]
public class GlobalClearStageBestRecord
{   
    public string leaderboardType = "ClearStageBestRecord";
    public string stageName;
    public List<StageLeaderboardData> stageLeaderboardData;
}

