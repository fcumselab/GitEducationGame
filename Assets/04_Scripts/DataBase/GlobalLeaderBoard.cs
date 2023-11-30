using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine;

//GlobalGameProcessLeaderBoard - �C���`�i�ױƦ�]
[Serializable]
public class GlobalGameProcessLeaderBoard
{
    string leaderboardType = "GameProcess";
    List<GlobalGameProcessData> globalGameProcessDataList;
} 

[Serializable]
public class GlobalGameProcessData
{
    //�ϥΪ̦W��: string
    //�C���`�i��: int
    //�F���ɪ��C���ɪ�: int

    public string playerName;
    public int gameProcess;
    public int playTime;
}


//GlobalTotalScoreLeaderBoard - �C���`�o���Ʀ�]
[Serializable]
public class GlobalTotalScoreLeaderBoard
{
    string leaderboardType = "TotalScore";
    List<GlobalTotalScoreData> globalTotalScoreDataList;
}

[Serializable]
public class GlobalTotalScoreData
{
    //�ϥΪ̦W��: string
    //�C���`�o��: int
    //�F���ɪ��C���ɪ�: int

    public string playerName;
    public int totalScore;
    public int playTime;
}


//GlobalClearStageBestRecord - �q�L���d�̨ΰO���Ʀ�]
[Serializable]
public class GlobalClearStageBestRecord
{   
    public string leaderboardType = "ClearStageBestRecord";
    public string stageName;
    public List<StageLeaderboardData> stageLeaderboardData;
}

