using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] List<string> completeQuestNameList;
    [SerializeField] List<string> completeQuestTimeList;
    [SerializeField] List<string> completeQuestTypeList;

    public string[] SplitStringIntoArray(string word, string keyword)
    {
        return word.Split(new string[] { keyword }, StringSplitOptions.None);
    }

    public string TrimString(string word, bool isTrimFront, bool isTrimBack)
    {
        if (isTrimFront) word = word.TrimStart();
        if (isTrimBack) word = word.TrimEnd();
        return word;
    }

    public void AddCompleteQuestData(string questName, string time, string type)
    {
        completeQuestNameList.Add(questName);
        completeQuestTimeList.Add(time);
        completeQuestTypeList.Add(type);
    }

    public string[] GetCompleteQuestName()
    {
        return completeQuestNameList.ToArray();
    }

    public string[] GetCompleteQuestTime()
    {
        return completeQuestTimeList.ToArray();
    }

    public string[] GetCompleteQuestType()
    {
        return completeQuestTypeList.ToArray();
    }

    public int CompareLeaderboard(GameObject ResultWindow ,int[] scoreList, string[] timeList, int score, int star, string time)
    {
        for (int i=0; i < scoreList.Length; i++)
        {
            if(score > scoreList[i])
            {
                PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(ResultWindow, "Window");
                NewRecordLeaderboard(fsm, score, star, time, i);
                return i + 1;
            }
            else if(score == scoreList[i])
            {
                int playerTime = MyTimer.Instance.ChangeTimeToSec(time);
                int leaderboardTime = MyTimer.Instance.ChangeTimeToSec(timeList[i]);
                if(playerTime < leaderboardTime)
                {
                    PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(ResultWindow, "Window");
                    NewRecordLeaderboard(fsm, score, star, time, i);
                    return i + 1;
                }
            }
        }
        return 0;
    }

    void NewRecordLeaderboard(PlayMakerFSM fsm, int score, int star, string time, int spot)
    {
        fsm.FsmVariables.FindFsmArray("playerNameList").InsertItem("±z", spot);
        fsm.FsmVariables.FindFsmArray("playerNameList").Resize(3);
        fsm.FsmVariables.FindFsmArray("playerScoreList").InsertItem(score, spot);
        fsm.FsmVariables.FindFsmArray("playerScoreList").Resize(3);
        fsm.FsmVariables.FindFsmArray("playerStarList").InsertItem(star, spot);
        fsm.FsmVariables.FindFsmArray("playerStarList").Resize(3);
        fsm.FsmVariables.FindFsmArray("playerClearTimeList").InsertItem(time, spot);
        fsm.FsmVariables.FindFsmArray("playerClearTimeList").Resize(3);
    }

}
