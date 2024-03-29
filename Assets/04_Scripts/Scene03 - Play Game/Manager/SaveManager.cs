using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;


public class SaveManager : SerializedMonoBehaviour
{
    // http:/xxx.xxx.xxx.xxx:xxx/ 
    [SerializeField] string baseUrl;

    [FoldoutGroup("Play Game")]
    [SerializeField] string selectedStageName;

    [FoldoutGroup("SaveManagerFsm")]
    [SerializeField] PlayMakerFSM switchSceneFsm;

    [SerializeField] bool isDebugMode = true;
    [SerializeField] bool updateTestingData = false;
    [SerializeField] bool isInitialFinish = false;

    [SerializeField] PlayerSaveData testingPlayerSaveData;
    [Header("Current PlayerSaveData")]
    public string userName;
    [SerializeField] PlayerSaveData playerSaveData;
    public string testSaveJson;
    #region instance
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
    #endregion

    #region Initial
    private void Start()
    {
        isInitialFinish = false;
        if (isDebugMode)
        {
            if (updateTestingData)
            {
                Debug.Log("please use register to get new account.");
            }
            else
            {
                Debug.Log("using testingPlayerSaveData");
                playerSaveData = testingPlayerSaveData;
                userName = "test player";
            }
        }
        isInitialFinish = true;
    }
    #endregion

    public void SaveToJson()
    {
        testSaveJson = JsonUtility.ToJson(playerSaveData, true);
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

    public List<GameManualItem> GetCommandDataListInGameManual()
    {
        while (!isInitialFinish) { }
        return playerSaveData.gameManualData[0].items;
    }

    public PlayerSaveData GetPlayerSaveData()
    {
        while (!isInitialFinish) { }
        return playerSaveData;
    }


    #region Stage Select
    public void AddGameManualUsedTimes()
    {
        playerSaveData.gameRecordData.totalTimesUsedGameManual++;
    }
    #endregion


    #region Play Game Scene

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
        }

        //Common -> update playData -> Upload
        UpdateGameRecordData(playTime);
        form = BuildGlobalLeaderBoardForm("PlayerSaveData");
        StartCoroutine(UploadPlayerSaveData(form));

        //Beat self record -> upload to GlobalLeaderBoard
        if (playerPlace == 1)
        {
            //Debug.Log("New GlobalLeaderBoard Record");
            form = BuildGlobalLeaderBoardForm("GameProgress");
            StartCoroutine(UpdateGlobalLeaderBoard(form));

            form = BuildGlobalLeaderBoardForm("ClearStageBestRecord", stageName, newLeaderBoardData);
            StartCoroutine(UpdateGlobalLeaderBoard(form));

            form = BuildGlobalLeaderBoardForm("TotalScore");
            StartCoroutine(UpdateGlobalLeaderBoard(form));
        }
    }

    IEnumerator UploadPlayerSaveData(WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}UpdatePlayerData", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.Log("Request Error");
            //Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
        }
        www.Dispose();
    }

    IEnumerator UpdateGlobalLeaderBoard(WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post($"{baseUrl}UpdateGlobalLeaderBoardData", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.Log("Request Error");
            //Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
        }
        www.Dispose();
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
            if (stageData.stageClearTimes > 0)
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

    #endregion

    #region Scene Switcher
    //Switch Scene Fsm
    public void InitializeGameManagerInScene(string currentSceneName, string lastSceneName)
    {
        switch (currentSceneName)
        {
            case "Stage Select":
                if (lastSceneName == "Play Game")
                {
                    GameObject.Find("Game Manager").GetComponent<GameManagerStageSelect>().InitializeScene(lastSceneName, selectedStageName);
                    selectedStageName = "";
                }
                else
                {
                    GameObject.Find("Game Manager").GetComponent<GameManagerStageSelect>().InitializeScene(lastSceneName, "");
                }
                break;
            case "Play Game":
                GameObject.Find("Game Manager").GetComponent<GameManagerPlayGame>().InitializeScene(lastSceneName);
                break;
            default:
                break;
        }
    }

    public void GoToStageSelectScene()
    {
        switchSceneFsm.FsmVariables.GetFsmString("targetSceneName").Value = "Stage Select";
        switchSceneFsm.enabled = true;
    }

    public void GoToPlayGameScene(string stageKey)
    {
        //stageKey isEmpty -> replay
        if (stageKey != "")
        {
            switchSceneFsm.FsmVariables.GetFsmString("selectedStageName").Value = stageKey;
            selectedStageName = stageKey;
        }
        switchSceneFsm.FsmVariables.GetFsmString("targetSceneName").Value = "Play Game";
        switchSceneFsm.enabled = true;
    }

    #endregion

    public StageData GetPlayingStageData()
    {
        return playerSaveData.stageData.Find((stageDataItem) => stageDataItem.stageName == selectedStageName);
    }

    public string GetSelectedStageName()
    {
        return selectedStageName;
    }
}

