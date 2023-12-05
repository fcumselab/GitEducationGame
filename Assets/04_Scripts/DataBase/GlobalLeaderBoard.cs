using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Networking;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.UI;

public class GlobalLeaderBoard : SerializedMonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject ClearStageItem;
    [SerializeField] GameObject ScoreItem;
    [SerializeField] GameObject GameProcessItem;

    [Header("generate location")]
    [SerializeField] Transform ClearStageItems;
    [SerializeField] Transform TotalScoreItems;
    [SerializeField] Transform TotalGameProcessItems;

    [Header("Save")]
    [SerializeField] List<GameObject> ClearStageItemList;
    [SerializeField] List<GameObject> ScoreItemList;
    [SerializeField] List<GameObject> GameProcessItemList;

    public void GetLeaderBoardData(string leaderBoardType, string stageName = "")
    {
        StartCoroutine(GetWebData(leaderBoardType, stageName, (result) =>
        {
            switch (leaderBoardType)
            {
                case "GameProcess":
                    {
                        ReturnGlobalLeaderBoardData<GlobalTotalGameProcessData> returnData = JsonUtility.FromJson<ReturnGlobalLeaderBoardData<GlobalTotalGameProcessData>>(result);
                        int saveListSize = GameProcessItemList.Count;
                        GameObject Item;

                        foreach (GameObject item in GameProcessItemList)
                        {
                            item.SetActive(false);
                        }

                        for (int i = 0; i < returnData.returnLeaderBoardData.Count; i++)
                        {
                            if (saveListSize < i + 1)
                            {
                                Item = Instantiate(GameProcessItem, TotalGameProcessItems);
                                GameProcessItemList.Add(Item);
                            }
                            else
                            {
                                Item = GameProcessItemList[i];
                            }

                            Item.SetActive(true);

                            Text text = Item.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
                            text.text = $"{i + 1}";
                            text = Item.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                            text.text = returnData.returnLeaderBoardData[i].playerName;
                            text = Item.transform.Find("PlayerDetailed/ProcessPanel/Process Text").GetComponent<Text>();
                            text.text = $"{returnData.returnLeaderBoardData[i].gameProcess} %";
                            text = Item.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                            text.text = MyTimer.Instance.StopWatch(returnData.returnLeaderBoardData[i].playTime);
                        }
                        break;
                    }
                case "ClearStageBestRecord":
                    {
                        ReturnGlobalLeaderBoardData<StageLeaderboardData> returnData = JsonUtility.FromJson<ReturnGlobalLeaderBoardData<StageLeaderboardData>>(result);
                        int saveListSize = ClearStageItemList.Count;
                        GameObject Item;

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
                            text.text = $"{i + 1}";
                            text = Item.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                            text.text = returnData.returnLeaderBoardData[i].playerName;

                            Transform Stars = Item.transform.Find("PlayerDetailed/Stars");
                            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
                            fsm.FsmVariables.GetFsmInt("getStarNum").Value = returnData.returnLeaderBoardData[i].playerStar;
                            fsm.enabled = true;

                            text = Item.transform.Find("PlayerDetailed/Score/Score Text").GetComponent<Text>();
                            text.text = $"{returnData.returnLeaderBoardData[i].playerScore}";
                            text = Item.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                            text.text = MyTimer.Instance.StopWatch(returnData.returnLeaderBoardData[i].playerClearTime);
                        }
                        break;
                    }
                case "TotalScore":
                    {
                        ReturnGlobalLeaderBoardData<GlobalTotalScoreData> returnData = JsonUtility.FromJson<ReturnGlobalLeaderBoardData<GlobalTotalScoreData>>(result);
                        int saveListSize = ScoreItemList.Count;
                        GameObject Item;

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
                            text.text = $"{i + 1}";
                            text = Item.transform.Find("PlayerDetailed/PlayerNamePanel/TextPanel/Name Text").GetComponent<Text>();
                            text.text = returnData.returnLeaderBoardData[i].playerName;
                            text = Item.transform.Find("PlayerDetailed/Score/Score Text").GetComponent<Text>();
                            text.text = $"{returnData.returnLeaderBoardData[i].totalScore}";
                            text = Item.transform.Find("PlayerDetailed/Time/Time Text").GetComponent<Text>();
                            text.text = MyTimer.Instance.StopWatch(returnData.returnLeaderBoardData[i].playTime);
                        }
                        break;
                    }
            }
        }));

    }

    IEnumerator GetWebData(string leaderBoardType, string stageName, Action<string> callback)
    {
        string url;

        if (stageName == "")
        {
            url = $"localhost:5050/getGlobalLeaderBoardData?type={leaderBoardType}";
        }
        else
        {
            url = $"localhost:5050/getGlobalLeaderBoardData?type={leaderBoardType}&stageName={stageName}";
        }

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            callback(www.downloadHandler.text);
        }
    }
}

[Serializable]
public class ReturnGlobalLeaderBoardData<T>
{
    public string status;
    public List<T> returnLeaderBoardData;
}



//GlobalTotalGameProcessLeaderBoard - 遊戲總進度排行榜
[Serializable]
public class GlobalTotalGameProcessLeaderBoard
{
    public string leaderBoardType = "GameProcess";
    public List<GlobalTotalGameProcessData> globalGameProcessDataList;
}

[Serializable]
public class GlobalTotalGameProcessData
{
    //使用者名稱: string
    //遊戲總進度: int
    //達成時的遊戲時長: int

    public string playerName;
    public int gameProcess;
    public int playTime;
}


//GlobalTotalScoreLeaderBoard - 遊戲總得分排行榜
[Serializable]
public class GlobalTotalScoreLeaderBoard
{
    public string leaderBoardType = "TotalScore";
    public List<GlobalTotalScoreData> globalTotalScoreDataList;
}

[Serializable]
public class GlobalTotalScoreData
{
    //使用者名稱: string
    //遊戲總得分: int
    //達成時的遊戲時長: int

    public string playerName;
    public int totalScore;
    public int playTime;
}


//GlobalClearStageBestRecord - 通過關卡最佳記錄排行榜
[Serializable]
public class GlobalClearStageBestRecord
{
    public string leaderBoardType = "ClearStageBestRecord";
    public string stageName;
    public List<StageLeaderboardData> stageLeaderboardData;
}

