using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StageManager : SerializedMonoBehaviour
{
    //UsedGameManualInStage
    [SerializeField] Dictionary<string, int> UsedCommandDict = new();
    [SerializeField] Dictionary<string, int> UsedRuleAndWindowDict = new();
    [SerializeField] Dictionary<string, int> UsedVersionControlDict = new();

    public int FindMatchKey(string key, string targetName)
    {
        //key == gameobject's Tag
        if(key == "GameManualType/Command")
        {
            if (UsedCommandDict.ContainsKey(targetName))
            {
                return UsedCommandDict[targetName];
            }
        }
        else if(key == "GameManualType/RuleAndWindow")
        {
            if (UsedRuleAndWindowDict.ContainsKey(targetName)) return UsedRuleAndWindowDict[targetName];
        }
        else if(key == "GameManualType/VersionControl")
        {
            if (UsedVersionControlDict.ContainsKey(targetName)) return UsedVersionControlDict[targetName];
        }
        else
        {
            Debug.Log("找不到這個 Key！");
        }
        return 0;
    }
}
