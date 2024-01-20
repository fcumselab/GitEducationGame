using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class DialogueSystemFeatureManager : SerializedMonoBehaviour
{
    [SerializeField] TutorialPopup tutorialPopup;

    [SerializeField] GameManual gameManual;
    
    [FoldoutGroup("Hint")]
    [SerializeField] List<PlayMakerFSM> enableHintList = new();
    [FoldoutGroup("Hint")]
    [SerializeField] Dictionary<string, PlayMakerFSM> hintDict = new();

    [Header("For other scripts to Register/Unregister")]
    [SerializeField] List<string> registerFunctionList = new();

    #region instance
    //Singleton instantation
    private static DialogueSystemFeatureManager instance;
    public static DialogueSystemFeatureManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<DialogueSystemFeatureManager>();
            return instance;
        }
    }
    #endregion

    public void RegisterFunction()
    {
        Debug.Log("RegisterFunction");
        Lua.RegisterFunction("HintController", this, SymbolExtensions.GetMethodInfo(() => HintController(string.Empty, false)));
        Lua.RegisterFunction("ResetAllTutorialObj", this, SymbolExtensions.GetMethodInfo(() => ResetAllTutorialObj()));
        Lua.RegisterFunction("UpdateButtonStatus", this, SymbolExtensions.GetMethodInfo(() => UpdateButtonStatus(string.Empty, false)));
        
        tutorialPopup.RegisterFunction();
    }

    public void UnregisterFunction()
    {
        Lua.UnregisterFunction("HintController");
        Lua.UnregisterFunction("ResetAllTutorialObj");

        tutorialPopup.UnregisterFunction();
        gameManual.RegisterFunction(false);

        //GameManual
        Lua.UnregisterFunction("UnlockGameManualItem");

        foreach(string key in registerFunctionList)
        {
            Lua.UnregisterFunction(key);
        }
    }

    public void RegisterFunctionGameManual(GameManual gameManual)
    {
        this.gameManual = gameManual;
        this.gameManual.RegisterFunction(true);
    }

    // For other scripts to Register/Unregister
    public void AddNewRegisterFunctionKey(string key)
    {
        registerFunctionList.Add(key);
    }

    //Hint_xxxxx  key = xxxxx
    public void HintController(string key, bool enable)
    {
        PlayMakerFSM TargetHint;
        if (hintDict.ContainsKey(key))
        {
            TargetHint = hintDict[key];
        }
        else
        {
            TargetHint = GameObject.Find("Hint_" + key).GetComponent<PlayMakerFSM>();
            hintDict.Add(key, TargetHint);
        }

        if (enable)
        {
            enableHintList.Add(TargetHint);
            TargetHint.enabled = true;
        }
        else
        {
            enableHintList.Remove(TargetHint);
            TargetHint.SendEvent("Hint/Tutorial Highlight/close highlight");
        }
    }
    
    public void UpdateButtonStatus(string key, bool enable)
    {
        string eventKey;
        switch (key)
        {
            case "All":
                eventKey = (enable) ? "DialogueFeature/FileButton/Enable" : "DialogueFeature/FileButton/Disable";
                PlayMakerFSM.BroadcastEvent(eventKey);
                break;
            case "FileButton":
                eventKey = (enable) ? "DialogueFeature/FileButton/Enable" : "DialogueFeature/FileButton/Disable";
                PlayMakerFSM.BroadcastEvent(eventKey);
                break;
        }
    }

    public void RegisterHintDict(Dictionary<string, PlayMakerFSM> newDict)
    {
        Debug.Log("RegisterHintDict");
        foreach (var item in newDict)
        {
            hintDict.Add(item.Key, item.Value);
        }
    }

    public void ResetHintStatus()
    {
        foreach(var item in enableHintList)
        {
            item.SendEvent("Hint/Tutorial Highlight/close highlight");
        }
        enableHintList.Clear();
    }

    public void ResetAllTutorialObj()
    {
        tutorialPopup.CloseImageWindow();
        tutorialPopup.ResetWindowLayer();
        tutorialPopup.BlackPanelControl(false);
        tutorialPopup.ResetHighLightObj();
        PlayMakerFSM.BroadcastEvent("Hint/Particle/Close Particle");
        DialogueLua.SetVariable("isReplay", false);
        ResetHintStatus();
        UpdateButtonStatus("All", true);
    }
}
