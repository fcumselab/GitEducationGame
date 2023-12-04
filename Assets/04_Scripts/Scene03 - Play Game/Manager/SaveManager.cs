using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.Networking;

public class SaveManager : SerializedMonoBehaviour
{
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
        if (playerSaveData.stageData.Count == 0)
        {
            Debug.Log("using testingPlayerSaveData");
            return testingPlayerSaveData.stageData;
        }
        else
        {
            return playerSaveData.stageData;
        }
    }

    public List<GameManualData> GetGameManualDataListFromPlayerData()
    {
        if (playerSaveData.stageData.Count == 0)
        {
            Debug.Log("using testingPlayerSaveData");
            return testingPlayerSaveData.gameManualData;
        }
        else
        {
            return playerSaveData.gameManualData;
        }
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

    public void SaveStageLeaderBoardData(string stageName, string[] nameList, int[] scoreList, int[] starList, int[] timeList)
    {
        StageData findStage = playerSaveData.stageData.Find((stage) => stage.stageName == stageName);

        for (int i = 0; i < nameList.Length; i++)
        {
            findStage.stageLeaderboardData[i].playerName = nameList[i];
            findStage.stageLeaderboardData[i].playerScore = scoreList[i];
            findStage.stageLeaderboardData[i].playerStar = starList[i];
            findStage.stageLeaderboardData[i].playerClearTime = timeList[i];
        }
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
    public void SendRequestToServer(string type, WWWForm form)
    {
        if(type == "POST")
        {
            StartCoroutine(PostWebData(form));

        }
        else if (type == "GET"){
            StartCoroutine(GetWebData());
        }
    }*/

    
    IEnumerator GetWebData()
    {
        UnityWebRequest www = UnityWebRequest.Get("localhost:5050/getData");
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

