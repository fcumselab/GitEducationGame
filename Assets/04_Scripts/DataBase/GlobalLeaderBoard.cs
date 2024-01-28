﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Networking;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.UI;

public class GlobalLeaderBoard : SerializedMonoBehaviour
{
    #region Variable

    // http:/xxx.xxx.xxx.xxx:xxx/ 
    [SerializeField] string baseUrl;

    [FoldoutGroup("Process Popup")]
    [SerializeField] GameObject WebsiteLoadingPanel;
    [FoldoutGroup("Process Popup")]
    [SerializeField] GameObject OopsPage;

    [FoldoutGroup("Prefab")]
    [SerializeField] GameObject ClearStageItem;
    [FoldoutGroup("Prefab")]
    [SerializeField] GameObject ScoreItem;
    [FoldoutGroup("Prefab")]
    [SerializeField] GameObject GameProgressItem;

    [FoldoutGroup("PlayerPlace")]
    [SerializeField] GameObject PlayerClearStageItem;
    [FoldoutGroup("PlayerPlace")]
    [SerializeField] GameObject PlayerScoreItem;
    [FoldoutGroup("PlayerPlace")]
    [SerializeField] GameObject PlayerGameProgressItem;

    [FoldoutGroup("Generate Location")]
    [SerializeField] Transform ClearStageItems;
    [FoldoutGroup("Generate Location")]
    [SerializeField] Transform TotalScoreItems;
    [FoldoutGroup("Generate Location")]
    [SerializeField] Transform TotalGameProgressItems;

    [FoldoutGroup("Data")]
    [SerializeField] List<GameObject> ClearStageItemList;
    [FoldoutGroup("Data")]
    [SerializeField] List<GameObject> ScoreItemList;
    [FoldoutGroup("Data")]
    [SerializeField] List<GameObject> GameProgressItemList;
    #endregion 

    public void GetLeaderBoardData(string leaderBoardType, string stageName = "")
    {
        OopsPage.SetActive(false);
        WebsiteLoadingPanel.SetActive(true);

        Text text;
        PlayMakerFSM fsm;
        StartCoroutine(GetWebData(leaderBoardType, stageName, (result, runResult) =>
        {
            switch (runResult)
            {
                case "Failed":
                    OopsPage.SetActive(true);
                    break;
                case "Success":
                    switch (leaderBoardType)
                    {
                        case "GameProgress":
                            {
                                ReturnGlobalLeaderBoardData<GlobalTotalGameProgressData> returnData = JsonUtility.FromJson<ReturnGlobalLeaderBoardData<GlobalTotalGameProgressData>>(result);

                                int saveListSize = GameProgressItemList.Count;
                                GameObject Item;

                                //Count Player own place index
                                int foundPlayerDataIndex = returnData.returnLeaderBoardData.FindIndex((item) => { return item.playerName == SaveManager.Instance.userName; });
                                GlobalTotalGameProgressData playerData = null;
                                if (foundPlayerDataIndex != -1) playerData = returnData.returnLeaderBoardData[foundPlayerDataIndex];
                                bool isFoundPlayerData = (playerData != null);

                                //Deactivate all gameObject.
                                foreach (GameObject item in GameProgressItemList)
                                {
                                    item.SetActive(false);
                                }

                                for (int i = 0; i < returnData.returnLeaderBoardData.Count; i++)
                                {
                                    if (saveListSize < i + 1)
                                    {
                                        Item = Instantiate(GameProgressItem, TotalGameProgressItems);
                                        GameProgressItemList.Add(Item);
                                    }
                                    else
                                    {
                                        Item = GameProgressItemList[i];
                                    }

                                    Item.SetActive(true);

                                    text = Item.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
                                    text.text = $"{returnData.placeList[i]}";
                                    text = Item.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                                    text.text = returnData.returnLeaderBoardData[i].playerName;
                                    text = Item.transform.Find("PlayerDetailed/ProgressPanel/Progress Text").GetComponent<Text>();
                                    text.text = $"{returnData.returnLeaderBoardData[i].gameProgress} %";
                                    text = Item.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                                    text.text = MyTimer.Instance.StopWatch(returnData.returnLeaderBoardData[i].playTime);

                                    if (foundPlayerDataIndex == i)
                                    {
                                        fsm = MyPlayMakerScriptHelper.GetFsmByName(Item, "Highlight TextBox");
                                        fsm.FsmVariables.GetFsmBool("needHighlight").Value = true;
                                        fsm.enabled = true;
                                    }
                                }

                                //Update player own place textBox
                                text = PlayerGameProgressItem.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? $"{returnData.placeList[foundPlayerDataIndex]}" : "0";
                                text = PlayerGameProgressItem.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                                text.text = SaveManager.Instance.userName;
                                text = PlayerGameProgressItem.transform.Find("PlayerDetailed/ProgressPanel/Progress Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? $"{playerData.gameProgress} %" : "0 %";
                                text = PlayerGameProgressItem.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? MyTimer.Instance.StopWatch(playerData.playTime) : "00:00";
                                break;
                            }
                        case "ClearStageBestRecord":
                            {
                                Transform Stars;

                                ReturnGlobalLeaderBoardData<StageLeaderboardData> returnData = JsonUtility.FromJson<ReturnGlobalLeaderBoardData<StageLeaderboardData>>(result);
                                int saveListSize = ClearStageItemList.Count;
                                GameObject Item;

                                //Count Player own place index
                                int foundPlayerDataIndex = returnData.returnLeaderBoardData.FindIndex((item) => { return item.playerName == SaveManager.Instance.userName; });
                                StageLeaderboardData playerData = null;
                                if (foundPlayerDataIndex != -1) playerData = returnData.returnLeaderBoardData[foundPlayerDataIndex];
                                bool isFoundPlayerData = (playerData != null);

                                foreach (GameObject item in ClearStageItemList)
                                {
                                    item.SetActive(false);
                                }

                                for (int i = 0; i < returnData.returnLeaderBoardData.Count; i++)
                                {
                                    if (saveListSize < i + 1)
                                    {
                                        Item = Instantiate(ClearStageItem, ClearStageItems);
                                        ClearStageItemList.Add(Item);
                                    }
                                    else
                                    {
                                        Item = ClearStageItemList[i];
                                    }

                                    Item.SetActive(true);

                                    Text text = Item.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
                                    text.text = $"{returnData.placeList[i]}";
                                    text = Item.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                                    text.text = returnData.returnLeaderBoardData[i].playerName;

                                    Stars = Item.transform.Find("PlayerDetailed/Stars");
                                    fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
                                    fsm.FsmVariables.GetFsmInt("getStarNum").Value = returnData.returnLeaderBoardData[i].playerStar;
                                    fsm.enabled = true;

                                    text = Item.transform.Find("PlayerDetailed/Score/Score Text").GetComponent<Text>();
                                    text.text = $"{returnData.returnLeaderBoardData[i].playerScore}";
                                    text = Item.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                                    text.text = MyTimer.Instance.StopWatch(returnData.returnLeaderBoardData[i].playerClearTime);

                                    if (foundPlayerDataIndex == i)
                                    {
                                        fsm = MyPlayMakerScriptHelper.GetFsmByName(Item, "Highlight TextBox");
                                        fsm.FsmVariables.GetFsmBool("needHighlight").Value = true;
                                        fsm.enabled = true;
                                    }
                                }

                                text = PlayerClearStageItem.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? $"{returnData.placeList[foundPlayerDataIndex]}" : "0";
                                text = PlayerClearStageItem.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                                text.text = SaveManager.Instance.userName;

                                Stars = PlayerClearStageItem.transform.Find("PlayerDetailed/Stars");
                                fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
                                fsm.FsmVariables.GetFsmInt("getStarNum").Value = isFoundPlayerData ? playerData.playerStar : 0;
                                fsm.enabled = true;

                                text = PlayerClearStageItem.transform.Find("PlayerDetailed/Score/Score Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? $"{playerData.playerScore}" : "0";
                                text = PlayerClearStageItem.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? MyTimer.Instance.StopWatch(playerData.playerClearTime) : "00:00";
                                break;
                            }
                        case "TotalScore":
                            {
                                ReturnGlobalLeaderBoardData<GlobalTotalScoreData> returnData = JsonUtility.FromJson<ReturnGlobalLeaderBoardData<GlobalTotalScoreData>>(result);
                                int saveListSize = ScoreItemList.Count;
                                GameObject Item;

                                //Player own place item
                                int foundPlayerDataIndex = returnData.returnLeaderBoardData.FindIndex((item) => { return item.playerName == SaveManager.Instance.userName; });
                                GlobalTotalScoreData playerData = null;
                                if (foundPlayerDataIndex != -1) playerData = returnData.returnLeaderBoardData[foundPlayerDataIndex];
                                bool isFoundPlayerData = (playerData != null);

                                foreach (GameObject item in ScoreItemList)
                                {
                                    item.SetActive(false);
                                }

                                for (int i = 0; i < returnData.returnLeaderBoardData.Count; i++)
                                {
                                    if (saveListSize < i + 1)
                                    {
                                        Item = Instantiate(ScoreItem, TotalScoreItems);
                                        ScoreItemList.Add(Item);
                                    }
                                    else
                                    {
                                        Item = ScoreItemList[i];
                                    }

                                    Item.SetActive(true);

                                    Text text = Item.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
                                    text.text = $"{returnData.placeList[i]}";
                                    text = Item.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                                    text.text = returnData.returnLeaderBoardData[i].playerName;
                                    text = Item.transform.Find("PlayerDetailed/Score/Score Text").GetComponent<Text>();
                                    text.text = $"{returnData.returnLeaderBoardData[i].totalScore}";
                                    text = Item.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                                    text.text = MyTimer.Instance.StopWatch(returnData.returnLeaderBoardData[i].playTime);

                                    if (foundPlayerDataIndex == i)
                                    {
                                        fsm = MyPlayMakerScriptHelper.GetFsmByName(Item, "Highlight TextBox");
                                        fsm.FsmVariables.GetFsmBool("needHighlight").Value = true;
                                        fsm.enabled = true;
                                    }
                                }

                                text = PlayerScoreItem.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? $"{returnData.placeList[foundPlayerDataIndex]}" : "0";
                                text = PlayerScoreItem.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                                text.text = SaveManager.Instance.userName;
                                text = PlayerScoreItem.transform.Find("PlayerDetailed/Score/Score Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? $"{playerData.totalScore}" : "0";
                                text = PlayerScoreItem.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                                text.text = isFoundPlayerData ? MyTimer.Instance.StopWatch(playerData.playTime) : "00:00";

                                break;
                            }
                    }
                    break;
            }

            WebsiteLoadingPanel.SetActive(false);
        }));


    }

    IEnumerator GetWebData(string leaderBoardType, string stageName, Action<string, string> callback)
    {
        string url;

        if (stageName == "")
        {
            url = $"{baseUrl}getGlobalLeaderBoardData?type={leaderBoardType}";
        }
        else
        {
            url = $"{baseUrl}getGlobalLeaderBoardData?type={leaderBoardType}&stageName={stageName}";
        }

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            callback(www.downloadHandler.text, "Failed");
        }
        else
        {
            callback(www.downloadHandler.text, "Success");
        }
    }
}

#region Variables
[Serializable]
public class ReturnGlobalLeaderBoardData<T>
{
    public List<int> placeList;
    public List<T> returnLeaderBoardData;
    public StageLeaderboardData playerLeaderBoardData;
}

//GlobalTotalGameProgressLeaderBoard
[Serializable]
public class GlobalTotalGameProgressLeaderBoard
{
    public string leaderBoardType = "GameProgress";
    public List<GlobalTotalGameProgressData> globalGameProgressDataList;
}

[Serializable]
public class GlobalTotalGameProgressData
{
    //�ϥΪ̦W��: string
    //�C���`�i��: int
    //�F���ɪ��C���ɪ�: int

    public string playerName;
    public int gameProgress;
    public int playTime;
}


//GlobalTotalScoreLeaderBoard
[Serializable]
public class GlobalTotalScoreLeaderBoard
{
    public string leaderBoardType = "TotalScore";
    public List<GlobalTotalScoreData> globalTotalScoreDataList;
}

[Serializable]
public class GlobalTotalScoreData
{
    public string playerName;   
    public int totalScore;
    public int playTime;
}

//GlobalClearStageBestRecord
[Serializable]
public class GlobalClearStageBestRecord
{
    public string leaderBoardType = "ClearStageBestRecord";
    public string stageName;
    public List<StageLeaderboardData> stageLeaderboardData;
}
#endregion
