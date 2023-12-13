using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

public class SaveManager : SerializedMonoBehaviour
{
    [SerializeField] bool isDebugMode = true;
    [SerializeField] bool isInitialFinish = false;

    [SerializeField] PlayerSaveData testingPlayerSaveData;
    [Header("Current PlayerSaveData")]
    public string userName;
    [SerializeField] PlayerSaveData playerSaveData;

    //[SerializeField] string saveJson;

    //Singleton instantation
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SaveManager>();
            return instance;
        }
    }

    private void Start()
    {
        isInitialFinish = false;
        if (isDebugMode)
        {
            Debug.Log("using testingPlayerSaveData");
            playerSaveData = testingPlayerSaveData;
            userName = "test player";
        }
        isInitialFinish = true;
    }
  
    //LoadPlayerData - from LoginManager.cs
    public void LoadPlayerSaveData(string username, PlayerSaveData playerSaveData)
    {
        userName = username;
        this.playerSaveData = playerSaveData;
    }

    public List<StageData> GetStageDataListFromPlayerData()
    {
        while (!isInitialFinish) { }
        return playerSaveData.stageData;
    }

    public List<GameManualData> GetGameManualDataListFromPlayerData()
    {
        while (!isInitialFinish) { }
        return playerSaveData.gameManualData;
    }

    public PlayerSaveData GetPlayerSaveData()
    {
        while (!isInitialFinish) { }
        return playerSaveData;
    }

    public void ClearTheStage(float playTime, string stageName, int playerPlace = 0, StageLeaderboardData newLeaderBoardData = null)
    {
        StageData targetStageData = playerSaveData.stageData.Find((item) => item.stageName == stageName);
        //unlock new stage if clear times = 0
        if (targetStageData.stageClearTimes == 0)
        {
            foreach (var unlockStageName in targetStageData.nextUnlockStageNameList)
            {
                StageData unlockStageData = playerSaveData.stageData.Find((item) => item.stageName == unlockStageName);
                unlockStageData.isStageUnlock = true;
            }
        }
        //clear times + 1
        targetStageData.stageClearTimes++;

        WWWForm form;
        if (playerPlace != 4)
        {
            targetStageData.stageLeaderboardData.Insert(playerPlace - 1, newLeaderBoardData);
            targetStageData.stageLeaderboardData.RemoveAt(3);

            UpdateGameRecordData(playTime);

            form = BuildGlobalLeaderBoardForm("PlayerSaveData");
            StartCoroutine(UploadPlayerSaveData(form));

            Debug.Log("Start POST GameProgress");
            form = BuildGlobalLeaderBoardForm("GameProgress");
            StartCoroutine(UpdateGlobalLeaderBoard(form));

            Debug.Log("Start POST ClearStageBestRecord");
            form = BuildGlobalLeaderBoardForm("ClearStageBestRecord", stageName, newLeaderBoardData);
            StartCoroutine(UpdateGlobalLeaderBoard(form));

            Debug.Log("Start POST TotalScore");
            form = BuildGlobalLeaderBoardForm("TotalScore");
            StartCoroutine(UpdateGlobalLeaderBoard(form));
        }
        else
        {
            UpdateGameRecordData(playTime);

            form = BuildGlobalLeaderBoardForm("PlayerSaveData");
            StartCoroutine(UploadPlayerSaveData(form));
        }
    }

    IEnumerator UploadPlayerSaveData(WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post("localhost:5050/UpdatePlayerData", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    IEnumerator UpdateGlobalLeaderBoard(WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post("localhost:5050/UpdateGlobalLeaderBoardData", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
    
    WWWForm BuildGlobalLeaderBoardForm(string type, string stageName = "", StageLeaderboardData newLeaderBoardData = null)
    {
        WWWForm form = new();
        form.AddField("leaderBoardType", type);
        switch (type)
        {
            case "GameProgress":
                form.AddField("playerName", userName);
                form.AddField("gameProgress", playerSaveData.gameRecordData.totalGameProgress);
                form.AddField("playTime", playerSaveData.gameRecordData.totalPlayTime);
                break;
            case "ClearStageBestRecord":
                form.AddField("stageName", stageName);
                form.AddField("playerName", userName);
                form.AddField("playerStar", newLeaderBoardData.playerStar);
                form.AddField("playerScore", newLeaderBoardData.playerScore);
                form.AddField("playerClearTime", newLeaderBoardData.playerClearTime);
                break;
            case "TotalScore":
                form.AddField("playerName", userName);
                form.AddField("totalScore", playerSaveData.gameRecordData.totalStageScore);
                form.AddField("playTime", playerSaveData.gameRecordData.totalPlayTime);
                break;
            case "PlayerSaveData":
                form.AddField("userName", userName);
                string saveJson = JsonUtility.ToJson(playerSaveData, true);
                form.AddField("saveData", saveJson);
                break;
        }
        return form;
    }

    void UpdateGameRecordData(float playTime)
    {
        GameDataManager gameDataManagerScript = GameObject.Find("Game Data Manager").GetComponent<GameDataManager>();
        playerSaveData.gameRecordData.totalCommandExecuteTimes += gameDataManagerScript.GetCommandExecuteTime();

        playerSaveData.gameRecordData.totalStageScore = 0;
        playerSaveData.gameRecordData.totalStarCount = 0;
        int clearStageCount = 0;
        foreach (StageData stageData in playerSaveData.stageData)
        {
            playerSaveData.gameRecordData.totalStageScore += stageData.stageLeaderboardData[0].playerScore;
            playerSaveData.gameRecordData.totalStarCount += stageData.stageLeaderboardData[0].playerStar;
            if(stageData.stageClearTimes > 0)
            {
                
                clearStageCount++;
            }
        }
        float progress = (clearStageCount * 100 / playerSaveData.stageData.Count);

        playerSaveData.gameRecordData.totalGameProgress = (int)progress;
        playerSaveData.gameRecordData.totalPlayTime += (int)playTime;

        playerSaveData.gameRecordData.totalTimesUsedGameManual += gameDataManagerScript.GetGameManualUsedTimes();
        playerSaveData.gameRecordData.totalTimesQuestClearPerfect += gameDataManagerScript.GetQuestCountPerfect();
        playerSaveData.gameRecordData.totalTimesQuestClearGood += gameDataManagerScript.GetQuestCountGood();
        playerSaveData.gameRecordData.totalTimesQuestClearHint += gameDataManagerScript.GetQuestCountHint();
        playerSaveData.gameRecordData.totalTimesQuestClearAnswer += gameDataManagerScript.GetQuestCountAnswer();
        playerSaveData.gameRecordData.totalTimesStageClear++;
    }
}

