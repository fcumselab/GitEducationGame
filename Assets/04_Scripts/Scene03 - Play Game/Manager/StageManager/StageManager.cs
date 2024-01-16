using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StageManager : SerializedMonoBehaviour
{
    enum StageType {Action, Quiz, Summary};
    [SerializeField] StageType stageType;
    [SerializeField] string stageName;

    
    #region Initial Variable
    [FoldoutGroup("Window")]
    [SerializeField] List<string> renderWindowList = new();
    [FoldoutGroup("Window")]
    [SerializeField] Dictionary<string, GameObject> AllWindowDict = new();

    //UsedGameManualInStage -> Enable Star/Command can use or not
    [FoldoutGroup("GameManual Give Star")]
    [SerializeField] Dictionary<string, int> UsedCommandDict = new();
    [FoldoutGroup("GameManual Give Star")]
    [SerializeField] Dictionary<string, int> UsedRuleAndWindowDict = new();
    [FoldoutGroup("GameManual Give Star")]
    [SerializeField] Dictionary<string, int> UsedVersionControlDict = new();
    #endregion

    [SerializeField] GameObject DefaultData;
    [SerializeField] PlayMakerFSM defaultCommitDataFsm;
    [SerializeField] PlayMakerFSM defaultBranchNameDataFsm;
    [FoldoutGroup("Reference")]
    public GameObject GameManager;
    [SerializeField] GameObject RenderWindowLayer;
    [FoldoutGroup("Reference")]
    [SerializeField] GameObject GameScreen;
    [FoldoutGroup("Reference")]
    public GameObject StageManagerParent;
    [FoldoutGroup("Reference")]
    public GameObject QuestTrackerParent;
    [FoldoutGroup("Reference/SelectionPopup")]
    [SerializeField] SelectionPopup BranchNamingPopup;
    [FoldoutGroup("Reference/SelectionPopup")]
    [SerializeField] SelectionPopup CommitMessagePopup;

    [Header("MessageForPlayMaker")]
    public bool isFinishInitialize;

    [Header("Stage Summary")]
    // 4 -> 2 star score line
    public List<int> getStarScoreLine = new(3);

    #region instance
    //Singleton instantation
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<StageManager>();
            return instance;
        }
    }
    #endregion


    #region Initialize
    public void Initialize(string stageName)
    {
        isFinishInitialize = false;
        this.stageName = stageName;
        StageManagerParent = transform.parent.gameObject;
        GameManager = StageManagerParent.transform.parent.gameObject;
        QuestTrackerParent = GameManager.transform.Find("Quest Tracker Parent").gameObject;

        if(stageType == StageType.Action) { 
            InitializeSelectionPopup();
        }

        RenderWindowLayer = GameObject.Find("Layer 2");
        GameScreen = GameObject.Find("GameScreen");

        RenderTargetWindow();
    }

    void InitializeSelectionPopup()
    {
        DefaultData = transform.Find("DefaultData").gameObject;
        defaultCommitDataFsm = MyPlayMakerScriptHelper.GetFsmByName(DefaultData, "Commit data");
        defaultBranchNameDataFsm = MyPlayMakerScriptHelper.GetFsmByName(DefaultData, "BranchName data");

        BranchNamingPopup = GameObject.Find("BranchNamingPopup").GetComponent<SelectionPopup>();
        CommitMessagePopup = GameObject.Find("CommitMessagePopup").GetComponent<SelectionPopup>();

        BranchNamingPopup.InitializePopup(defaultBranchNameDataFsm);
        CommitMessagePopup.InitializePopup(defaultCommitDataFsm);
    }

    public void RenderTargetWindow()
    {
        foreach (string windowName in renderWindowList)
        {
            GameObject cloneWindow = Instantiate(AllWindowDict[windowName], GameScreen.transform);
            cloneWindow.SetActive(true);
            cloneWindow.name = windowName;
            cloneWindow.transform.SetParent(RenderWindowLayer.transform, true);
        }

        isFinishInitialize = true;
    }

    #endregion

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

    public Dictionary<string, int> GetUsedCommandDict()
    {
        return UsedCommandDict;
    }

    #region GameManual
    public bool CheckGameManualListItemUseInStage(string key, string categoryType)
    {
        switch (categoryType)
        {
            case "Command":
                return UsedCommandDict.ContainsKey(key);
            case "RuleAndWindow":
                return UsedRuleAndWindowDict.ContainsKey(key);
            case "VersionControl":
                return UsedVersionControlDict.ContainsKey(key);
            default:
                Debug.LogError("Please use correct categoryType!");
                return false;
        }
    }

    public bool CheckGameManualContentItemUseInStage(string key, int commandPageNum)
    {
        if (UsedCommandDict.ContainsKey(key))
        {
            return (UsedCommandDict[key] >= commandPageNum);
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Dialog System Manager
    public string GetStageType()
    {
        return stageType.ToString();
    }
    #endregion
}
