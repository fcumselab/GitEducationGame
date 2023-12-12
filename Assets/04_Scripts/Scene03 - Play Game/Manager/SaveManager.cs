using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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
        }
        isInitialFinish = true;
    }
    /* ready to delete this.
    public void SavePlayerData()
    {
        saveJson = JsonUtility.ToJson(playerSaveData, true);
        Debug.Log(saveJson);
    }*/

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
        if (playerSaveData.stageData.Count == 0)
        {
            Debug.Log("using testingPlayerSaveData");
            return testingPlayerSaveData;
        }
        else
        {
            return playerSaveData;
        }
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
            for (int i = 0; i < findStage.stageLeaderboardData.Count; i++)
            {
                fsm.FsmVariables.FindFsmArray("playerNameList").Set(i, findStage.stageLeaderboardData[i].playerName);
                fsm.FsmVariables.FindFsmArray("playerScoreList").Set(i, findStage.stageLeaderboardData[i].playerScore);
                fsm.FsmVariables.FindFsmArray("playerStarList").Set(i, findStage.stageLeaderboardData[i].playerStar);
                fsm.FsmVariables.FindFsmArray("playerClearTimeList").Set(i, findStage.stageLeaderboardData[i].playerClearTime);
            }
        }
        return true;
    }

    //
    public void ClearTheStage(string stageName, int playerPlace = 0, StageLeaderboardData newLeaderBoardData = null)
    {
        //clear times + 1
        StageData targetStageData = playerSaveData.stageData.Find((item) => item.stageName == stageName);
        targetStageData.stageClearTimes++;

        //unlock new stage
        foreach (var unlockStageName in targetStageData.nextUnlockStageNameList)
        {
            StageData unlockStageData = playerSaveData.stageData.Find((item) => item.stageName == unlockStageName);
            unlockStageData.isStageUnlock = true;
        }

        //send POST request to local (save player data)
        if (playerPlace != 4)
        {
            targetStageData.stageLeaderboardData.Insert(playerPlace-1, newLeaderBoardData);
            targetStageData.stageLeaderboardData.RemoveAt(3);
        }

        //send POST request to global (if player get new record)
    }

    //
    public void SaveStageLeaderBoardData(string stageName, int playerPlace, StageLeaderboardData newLeaderBoardData = null)
    {
        
    }

    public void UnlockNextStages(string stageName)
    {
        StageData targetStage = playerSaveData.stageData.Find((stage) => stage.stageName == stageName);
        targetStage.stageClearTimes++;
        for (int i = 0; i < targetStage.nextUnlockStageNameList.Count; i++)
        {
            StageData unlockStage = playerSaveData.stageData.Find((stage) => stage.stageName == targetStage.nextUnlockStageNameList[i]);
            unlockStage.isStageUnlock = true;

            if (unlockStage == null) Debug.Log("Cannot find Stage : " + stageName);
        }
    }

    /*
    IEnumerator PostWebData(WWWForm form)
    {
        
        form.AddField("playerSaveData", saveJson);

        UnityWebRequest www = UnityWebRequest.Post("localhost:5050/postData", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }*/
}

