using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] List<string> completeQuestNameList;
    [SerializeField] List<string> completeQuestTimeList;
    [SerializeField] List<string> completeQuestTypeList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] SplitStringIntoArray(string word, string keyword)
    {
        return word.Split(new string[] { keyword }, StringSplitOptions.None);
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

    public int CompareLeaderboard(int[] scoreList, string[] timeList, int score, string time)
    {
        for (int i=0; i < scoreList.Length; i++)
        {
            if(score > scoreList[i])
            {
                return i + 1;
            }else if(score == scoreList[i])
            {
                int playerTime = MyTimer.Instance.ChangeTimeToSec(time);
                int leaderboardTime = MyTimer.Instance.ChangeTimeToSec(timeList[i]);
                if(playerTime < leaderboardTime)
                {
                    return i + 1;
                }
            }
        }
        return 0;
    }
}
