using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameDataManager : SerializedMonoBehaviour
{
    [SerializeField] List<string> completeQuestNameList;
    [SerializeField] List<string> completeQuestTimeList;
    [SerializeField] List<int> completeQuestUsedTimeList;
    [SerializeField] List<string> completeQuestTypeList;

    //type: self, hint, answer, perfect
    public void AddCompleteQuestData(string questName, string time, string type, string lastCompletedTime)
    {
        int completedTime = MyTimer.Instance.ChangeTimeToSec(time);
        int lastCompletedTimeNum = MyTimer.Instance.ChangeTimeToSec(lastCompletedTime);

        completeQuestNameList.Add(questName);
        completeQuestTimeList.Add(time);
        completeQuestUsedTimeList.Add(completedTime - lastCompletedTimeNum);
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

    public int[] GetCompleteQuestUsedTime()
    {
        return completeQuestUsedTimeList.ToArray();
    }

    public string[] GetCompleteQuestType()
    {
        return completeQuestTypeList.ToArray();
    }
}
