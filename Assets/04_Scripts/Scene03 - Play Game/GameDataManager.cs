using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameDataManager : SerializedMonoBehaviour
{
    [Header("ResultWindow")]
    [SerializeField] List<string> completeQuestNameList;
    [SerializeField] List<int> completeQuestTimeList;
    [SerializeField] List<int> completeQuestUsedTimeList;
    [SerializeField] List<string> completeQuestTypeList;

    //type: self, hint, answer, perfect
    public void AddCompleteQuestData(string questName, float time, string type, int lastCompletedTime)
    {
        int completedTime = (int)time;

        completeQuestNameList.Add(questName);
        completeQuestTimeList.Add(completedTime);
        completeQuestUsedTimeList.Add(completedTime - lastCompletedTime);
        completeQuestTypeList.Add(type);
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
