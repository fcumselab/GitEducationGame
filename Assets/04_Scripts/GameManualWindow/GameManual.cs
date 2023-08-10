using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManual : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, bool> unlockCommandDict = new();

    public bool GetUnlockCommandDict(string keyword)
    {
        if (unlockCommandDict.ContainsKey(keyword))
        {
            return unlockCommandDict[keyword];
        }
        else
        {
            Debug.Log("Warning! GameManual GetUnlockCommandDict Cannot find keyword!");
            return false;
        }
    }
    public void UnlockUnlockCommandDict(string keyword,int id = 0)
    {
        string key;
        if (id != 0) key = keyword + "/" + id;
        else key = keyword;
        
        if (unlockCommandDict.ContainsKey(key))
        {
            unlockCommandDict[key] = true;
        }
        else
        {
            Debug.Log("Warning! GameManual UnlockUnlockCommandDict Cannot find keyword!");
        }
    }

    public int GetUnlockCommandSize(string keyword)
    {
        for (int i = 1; i < 10; i++)
        {
            string key = keyword + "/" + i;
            if (unlockCommandDict.ContainsKey(key))
            {
                if (!unlockCommandDict[key]) return i;
            }
            else return i;
        }
        Debug.Log("Warning! GameManual GetUnlockCommandSize");
        return -1;
    }
}