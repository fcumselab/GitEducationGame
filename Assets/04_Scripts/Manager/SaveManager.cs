using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine;

public class SaveManager : SerializedMonoBehaviour
{
    [Header("Default SaveData")]
    [SerializeField] SaveData defaultSaveData = new();

    [Header("Player Current SaveData")]
    [SerializeField] SaveData saveData = new();
    [SerializeField] string saveJson;

    public void SavePlayerData()
    {
        saveJson = JsonUtility.ToJson(saveData,true);
        Debug.Log(saveJson);
    }

    //LoadPlayerData 
    public void LoadPlayerData()
    {
        saveData = JsonUtility.FromJson<SaveData>(saveJson);
    }

    //LoadStageData -> in "Stage Selection" scene -> Run function from "Stage Manager" GameObject
    public bool LoadStageData(GameObject stageObj)
    {
        List<StageData> stageData = saveData.stageData;
        StageData findStage = stageData.Find((stage) => stage.stageName == stageObj.name);
        
        if (findStage == null)
        {
            Debug.Log("Cannot find Stage : " + stageObj.name);
            return true;
        }
        
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(stageObj, "Button Initial"); 
        fsm.FsmVariables.FindFsmString("stageName").Value = findStage.stageName;
        fsm.FsmVariables.FindFsmBool("isStageClear").Value = findStage.isStageClear;
        fsm.FsmVariables.FindFsmBool("isStageUnlock").Value = findStage.isStageUnlock;
        if (findStage.isStageUnlock)
        {
            for (int i = 0; i < 3; i++)
            {
                fsm.FsmVariables.FindFsmArray("playerNameList").Set(i, findStage.stageLeaderboard.playerName[i]);
                fsm.FsmVariables.FindFsmArray("playerScoreList").Set(i, findStage.stageLeaderboard.playerScore[i]);
                fsm.FsmVariables.FindFsmArray("playerStarList").Set(i, findStage.stageLeaderboard.playerStar[i]);
                fsm.FsmVariables.FindFsmArray("playerClearTimeList").Set(i, findStage.stageLeaderboard.playerClearTime[i]);
            }
        }
        return true;
    }

    public void SaveStageLeaderBoardData(string stageName, string[] nameList, int[] scoreList, int[] starList, string[] timeList)
    {
        StageData findStage = saveData.stageData.Find((stage) => stage.stageName == stageName);

        for (int i = 0; i < nameList.Length ; i++)
        {
            findStage.stageLeaderboard.playerName[i] = nameList[i];
            findStage.stageLeaderboard.playerScore[i] = scoreList[i];
            findStage.stageLeaderboard.playerStar[i] = starList[i];
            findStage.stageLeaderboard.playerClearTime[i] = timeList[i];
        }
    }

}


[Serializable]
public class SaveData
{
    public int level;
    public float timeElapsed;
    public string playerName;
    public List<StageData> stageData = new();
    public List<GameManualData> gameManualData = new();
     
}

[Serializable]
public class StageData
{
    public string stageName;
    public bool isStageClear;
    public bool isStageUnlock;
    public StageLeaderBoardData stageLeaderboard = new();
    public List<string> nextStageNameList;
}

[Serializable]
public class StageLeaderBoardData
{
    public List<string> playerName;
    public List<int> playerScore;
    public List<int> playerStar;
    public List<string> playerClearTime;
}

[Serializable]
public class GameManualData
{
    public string manualType;
    public List<GameManualItem> manualListData = new();
}

[Serializable]
public class GameManualItem
{
    public string listName;
    public int listUnlockProgress;
    public bool isUnlock;
}


