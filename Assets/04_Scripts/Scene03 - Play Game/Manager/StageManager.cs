using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StageManager : SerializedMonoBehaviour
{
    [SerializeField] List<string> renderWindowList = new();

    [SerializeField] Dictionary<string, GameObject> AllWindowDict = new();
    //UsedGameManualInStage
    [SerializeField] Dictionary<string, int> UsedCommandDict = new();
    [SerializeField] Dictionary<string, int> UsedRuleAndWindowDict = new();
    [SerializeField] Dictionary<string, int> UsedVersionControlDict = new();

    [SerializeField] GameObject RenderWindowLayer;
    [SerializeField] GameObject GameScreen;

    private void Start()
    {
        RenderWindowLayer = GameObject.Find("Layer 2");
        GameScreen = GameObject.Find("GameScreen");
        RenderTargetWindow();
    }

    public void RenderTargetWindow()
    {
        foreach (string windowName in renderWindowList)
        {
            Debug.Log("want to render: " + windowName);
            GameObject cloneWindow = Instantiate(AllWindowDict[windowName], GameScreen.transform);
            cloneWindow.SetActive(true);
            cloneWindow.name = windowName;
            cloneWindow.transform.SetParent(RenderWindowLayer.transform, true);
        }
    }

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
            Debug.Log("�䤣��o�� Key�I");
        }
        return 0;
    }
}
