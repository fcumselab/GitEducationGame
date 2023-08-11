using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    public void LoadPlayerData()
    {
        saveData = JsonUtility.FromJson<SaveData>(saveJson);
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
    public List<StageLeaderBoardData> stageLeaderboard = new();
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


