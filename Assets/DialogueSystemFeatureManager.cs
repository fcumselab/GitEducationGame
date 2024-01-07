using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class DialogueSystemFeatureManager : SerializedMonoBehaviour
{
    [SerializeField] TutorialPopup tutorialPopup;
    [SerializeField] Dictionary<string, PlayMakerFSM> hintDict = new();
    [SerializeField] List<PlayMakerFSM> enableHintList = new();

    [SerializeField] GameObject GameManualWindow;

    public void RegisterFunction()
    {
        Lua.RegisterFunction("HintController", this, SymbolExtensions.GetMethodInfo(() => HintController(string.Empty, false)));
        Lua.RegisterFunction("ResetAllTutorialObj", this, SymbolExtensions.GetMethodInfo(() => ResetAllTutorialObj()));

        tutorialPopup.RegisterFunction();
        
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
        ResetHintStatus();
    }
}
