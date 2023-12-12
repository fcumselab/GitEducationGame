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
        while (!isInitialFinish) { }
        return playerSaveData;
    }

    public void ClearTheStage(string stageName, int playerPlace = 0, StageLeaderboardData newLeaderBoardData = null)
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

        
        if (playerPlace != 4)
        {
            //Insert newPlayerData
            targetStageData.stageLeaderboardData.Insert(playerPlace - 1, newLeaderBoardData);
            targetStageData.stageLeaderboardData.RemoveAt(3);

            //send POST request to global (if player get new record)
            //Count Game Progress and send POST request
            //send POST request to update clear stage LB
            //Count Total Game Score and send POST request


            //send POST request to local (save player data)

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

