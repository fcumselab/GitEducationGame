using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.Networking;

public class SaveManager : SerializedMonoBehaviour
{
    [Header("Default PlayerSaveData")]
    //[SerializeField] PlayerSaveData defaultSaveData = new();

    [Header("Current PlayerSaveData")]
    [SerializeField] PlayerSaveData playerSaveData = new();
    [SerializeField] string saveJson;

    private void Start()
    {
        StartCoroutine(GetWebData());
    }

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

    public List<StageData> GetStageDataListFromPlayerData()
    {
        return playerSaveData.stageData;
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
    
    public void TestSendRequestToServer(string type)
    {
        if(type == "POST")
        {
            StartCoroutine(PostWebData());

        }
        else if (type == "GET"){
            StartCoroutine(GetWebData());
        }
    }

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

    IEnumerator PostWebData()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

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
    }
}

