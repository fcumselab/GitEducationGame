using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine;

public class SaveManager : SerializedMonoBehaviour
{
    [Header("Default PlayerSaveData")]
    [SerializeField] PlayerSaveData defaultSaveData = new();

    [Header("Current PlayerSaveData")]
    [SerializeField] PlayerSaveData playerSaveData = new();
    [SerializeField] string saveJson;

    public void SavePlayerData()
    {
        saveJson = JsonUtility.ToJson(playerSaveData, true);
        Debug.Log(saveJson);
    }

    //LoadPlayerData 
    public void LoadPlayerData()
    {
        playerSaveData = JsonUtility.FromJson<PlayerSaveData>(saveJson);
    }

    //LoadStageData -> in "Stage Selection" scene -> Run function from "Stage Manager" GameObject
    public bool LoadStageData(GameObject stageObj)
    {
        List<StageData> stageData = playerSaveData.stageData;
        StageData findStage = stageData.Find((stage) => stage.stageName == stageObj.name);
        
        if (findStage == null)
        {
            Debug.Log("Cannot find Stage : " + stageObj.name);
            return true;
        }
        
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(stageObj, "Button Initial"); 
        fsm.FsmVariables.FindFsmInt("stageClearTimes").Value = findStage.stageClearTimes;
        fsm.FsmVariables.FindFsmBool("isStageUnlock").Value = findStage.isStageUnlock;
        if (findStage.isStageUnlock)
        {
            for (int i = 0; i < findStage.selfStageLeaderboardData.Count; i++)
            {
                fsm.FsmVariables.FindFsmArray("playerNameList").Set(i, findStage.selfStageLeaderboardData[i].playerName);
                fsm.FsmVariables.FindFsmArray("playerScoreList").Set(i, findStage.selfStageLeaderboardData[i].playerScore);
                fsm.FsmVariables.FindFsmArray("playerStarList").Set(i, findStage.selfStageLeaderboardData[i].playerStar);
                fsm.FsmVariables.FindFsmArray("playerClearTimeList").Set(i, findStage.selfStageLeaderboardData[i].playerClearTime);
            }
        }
        return true;
    }

    public void SaveStageLeaderBoardData(string stageName, string[] nameList, int[] scoreList, int[] starList, int[] timeList)
    {
        StageData findStage = playerSaveData.stageData.Find((stage) => stage.stageName == stageName);

        for (int i = 0; i < nameList.Length ; i++)
        {
            findStage.selfStageLeaderboardData[i].playerName = nameList[i];
            findStage.selfStageLeaderboardData[i].playerScore = scoreList[i];
            findStage.selfStageLeaderboardData[i].playerStar = starList[i];
            findStage.selfStageLeaderboardData[i].playerClearTime = timeList[i];
        }
    }

    public void UnlockNextStages(string stageName)
    {
        StageData targetStage = playerSaveData.stageData.Find((stage) => stage.stageName == stageName);
        targetStage.stageClearTimes++;
        for (int i = 0; i < targetStage.nextUnlockStageNameList.Count ; i++)
        {
            StageData unlockStage = playerSaveData.stageData.Find((stage) => stage.stageName == targetStage.nextUnlockStageNameList[i]);
            unlockStage.isStageUnlock = true;
            
            if (unlockStage == null) Debug.Log("Cannot find Stage : " + stageName);
        }
    }

    public void LoadGameManualData(GameObject MaunalWindow, string fsmName)
    {
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(MaunalWindow, fsmName);

        for (int i = 0; i < playerSaveData.gameManualData.Count; i++)
        {
            GameManualData manualData = playerSaveData.gameManualData[i];
            string type = manualData.manualType;
            //CommandNameList, RuleAndWindowNameList, VersionControlNameList...
            FsmArray nameList = fsm.FsmVariables.FindFsmArray(type + "NameList");
            FsmArray unlockProgressList = fsm.FsmVariables.FindFsmArray(type + "UnlockProgressList");

            for (int t = 0; t < manualData.items.Count; t++)
            {
                GameManualItem item = manualData.items[t];
                nameList.InsertItem(item.listName, t);
                unlockProgressList.InsertItem(item.listUnlockProgress, t);
            }
        }
    }

    public void SaveGameManualData(string[] typeList, string[] nameList, int[] unlockProgressList)
    {
        GameManualData CommandManual = playerSaveData.gameManualData[0];
        GameManualData RuleAndWindowManual = playerSaveData.gameManualData[1];
        GameManualData VersionControlManual = playerSaveData.gameManualData[2];

        for (int i = 0; i < typeList.Length; i++)
        {
            GameManualItem findItem;
            switch (typeList[i])
            {
                case "C":
                    findItem = CommandManual.items.Find((item) => item.listName == nameList[i]);
                    break;
                case "RAW":
                    findItem = RuleAndWindowManual.items.Find((item) => item.listName == nameList[i]);
                    break;
                case "VC":
                    findItem = VersionControlManual.items.Find((item) => item.listName == nameList[i]);
                    break;
                default:
                    Debug.Log("SaveGameManualData Error!");
                    return;
            }

            findItem.listUnlockProgress = unlockProgressList[i];
        }
    }
}


[Serializable]
public class PlayerSaveData
{
    private string playerName;
    private string password; 

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
    public int totalPlayTime;
    public int totalTimesStageClear;

    [Header("Help")]
    public int totalTimesUsedGameManual;

    [Header("Command")]
    public int totalRunCommandTimes;
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
    // isStageUnlock:           true
    // stageClearTimes:         1
    // selfStageLeaderboard:    [{A,3,1000,10:50},{B,2,500,08:22},{C,1,250,04:33}]

    public string stageName;
    public bool isStageUnlock;
    public int stageClearTimes;

    //Three player data in this leaderboard
    public List<SelfStageLeaderboardData> selfStageLeaderboardData = new(3);
    public List<string> nextUnlockStageNameList = new();
}

[Serializable]
public class SelfStageLeaderboardData
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


