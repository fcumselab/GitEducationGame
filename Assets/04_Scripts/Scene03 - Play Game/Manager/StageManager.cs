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

    [Header("Reference/ExistGameObject")]
    [SerializeField] GameObject RenderWindowLayer;
    [SerializeField] GameObject GameScreen;
    public GameObject StageManagerParent;
    public GameObject QuestTrackerParent;
    public GameObject GameManager;

    [Header("MessageForPlayMaker")]
    public bool isFinishInitialize;

    [Header("BackgroundStory")]
    public string backgroundStoryText = "";

    private void Start()
    {
        isFinishInitialize = false;

        StageManagerParent = transform.parent.gameObject;
        GameManager = StageManagerParent.transform.parent.gameObject;
        QuestTrackerParent = GameManager.transform.Find("Quest Tracker Parent").gameObject;

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

        isFinishInitialize = true;
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
            Debug.Log("找不到這個 Key！");
        }
        return 0;
    }
}
