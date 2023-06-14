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
}
