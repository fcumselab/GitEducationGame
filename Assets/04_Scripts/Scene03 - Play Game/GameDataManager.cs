using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class GameDataManager : SerializedMonoBehaviour
{
    [Header("ResultWindow")]
    [SerializeField] List<string> completeQuestNameList;
    [SerializeField] List<int> completeQuestTimeList;
    [SerializeField] List<int> completeQuestUsedTimeList;
    [SerializeField] List<string> completeQuestTypeList;

    [Header("StageData")]
    [SerializeField] StageData selectedStageData;
    //type: self, hint, answer, perfect
    public void AddCompleteQuestData(string questName, float time, string type, int lastCompletedTime)
    {
        int completedTime = (int)time;

        completeQuestNameList.Add(questName);
        completeQuestTimeList.Add(completedTime);
        completeQuestUsedTimeList.Add(completedTime - lastCompletedTime);
        completeQuestTypeList.Add(type);
    }

    public void GetSelectedStageData(string stageName)
    {
        List<StageData> allStageDataList = SaveManager.Instance.GetStageDataListFromPlayerData();
        selectedStageData = allStageDataList.Find((item) => item.stageName == stageName);
    }
    
    public void GetSelfLeaderBoardData(GameObject SelfLeaderBoardGroup)
    {
        List<StageLeaderboardData> stageLeaderboardData = selectedStageData.stageLeaderboardData;
        int index = 0;
        foreach(StageLeaderboardData item in stageLeaderboardData)
        {
            Transform selfLeaderBoardItem = SelfLeaderBoardGroup.transform.GetChild(index);
            bool hasRecord = (item.playerScore != 0);

            Text text = selfLeaderBoardItem.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
            text.text = $"{index + 1}";
            text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text").GetComponent<Text>();
            text.text = hasRecord  ? item.playerName : "�L" ;

            Transform Stars = selfLeaderBoardItem.transform.Find("Stars");
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
            fsm.FsmVariables.GetFsmInt("getStarNum").Value = item.playerStar;
            fsm.enabled = true;

            text = selfLeaderBoardItem.transform.Find("Score & Time/Score/Score Text").GetComponent<Text>();
            text.text = $"{item.playerScore}";
            text = selfLeaderBoardItem.transform.Find("Score & Time/Time/Time Text").GetComponent<Text>();
            text.text = MyTimer.Instance.StopWatch(item.playerClearTime);

            index++;
        }
    }

    public string[] GetCompleteQuestName()
    {
        return completeQuestNameList.ToArray();
    }

    public int[] GetCompleteQuestTime()
    {
        return completeQuestTimeList.ToArray();
    }

    public int[] GetCompleteQuestUsedTime()
    {
        return completeQuestUsedTimeList.ToArray();
    }

    public string[] GetCompleteQuestType()
    {
        return completeQuestTypeList.ToArray();
    }
}
