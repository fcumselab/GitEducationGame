using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;
using Lean.Gui;

public class TutorialPopup : SerializedMonoBehaviour
{
    [SerializeField] bool isDialogOpen = false;
    [SerializeField] Dictionary<string, GameObject> WindowDict = new();
    [SerializeField] Transform WindowLayer;

    [SerializeField] Dictionary<Transform, Transform> highlightObjDict = new();

    [SerializeField] PlayMakerFSM tutorialPopupAnimationFsm;


    [SerializeField] GameObject ImageWindow;
    [SerializeField] Image ImageIcon;
    [SerializeField] PlayMakerFSM ImageWindowAnimationFsm;

    [SerializeField] GameObject HighlightPos1;
    [SerializeField] GameObject HighlightPos2;

    [SerializeField] List<GameObject> saveWindowObjList;
    [SerializeField] List<PlayMakerFSM> saveWindowScriptList;


    [SerializeField] GameObject BlackPanel;
    [SerializeField] PlayMakerFSM BlackPanelFsm;

    Transform TutorialLayer;

    private void Start()
    {
        TutorialLayer = transform.parent;
    }

    public void RegisterFunction()
    {
        Lua.RegisterFunction("HighlightWindow", this, SymbolExtensions.GetMethodInfo(() => HighLightWindow(string.Empty, string.Empty)));
        Lua.RegisterFunction("ShowImage", this, SymbolExtensions.GetMethodInfo(() => ShowImage(string.Empty)));
        Lua.RegisterFunction("CloseImageWindow", this, SymbolExtensions.GetMethodInfo(() => CloseImageWindow()));
        Lua.RegisterFunction("BlackPanelControl", this, SymbolExtensions.GetMethodInfo(() => BlackPanelControl(false)));
        Lua.RegisterFunction("HighLightObj", this, SymbolExtensions.GetMethodInfo(() => HighLightObj(string.Empty)));
    }

    public void UnregisterFunction()
    {
        Lua.UnregisterFunction("HighlightWindow");
        Lua.UnregisterFunction("ShowImage");
        Lua.UnregisterFunction("CloseImageWindow");
        Lua.UnregisterFunction("BlackPanelControl");
        Lua.UnregisterFunction("HighLightObj");
    }

    public void ShowImage(string imageKey)
    {
        ResetWindowLayer();
        //Debug.Log("Show Image");
        if (ImageWindow.GetComponent<CanvasGroup>().alpha != 1)
        {
            ImageWindowAnimationFsm.FsmVariables.GetFsmString("runType").Value = "open";
            ImageWindowAnimationFsm.enabled = true;
        }
        ImageIcon.sprite = ImageManager.Instance.GetTutorialImage(imageKey);
    }

    public void CloseImageWindow()
    {
        ImageWindowAnimationFsm.FsmVariables.GetFsmString("runType").Value = "close";
        ImageWindowAnimationFsm.enabled = true;
    }

    public void BlackPanelControl(bool open)
    {
        if (open)
        {
            BlackPanelFsm.FsmVariables.GetFsmString("runType").Value = "open";
        }
        else
        {
            BlackPanelFsm.FsmVariables.GetFsmString("runType").Value = "close";
        }
        BlackPanelFsm.enabled = true;
    }

    public void HighLightWindow(string windowName1, string windowName2)
    {
        CloseImageWindow();
        ResetWindowLayer();

        GameObject TargetWindow;
        if (WindowDict.ContainsKey(windowName1))
        {
            TargetWindow = WindowDict[windowName1];
        }
        else
        {
            TargetWindow = GameObject.Find(windowName1);
            WindowDict.Add(windowName1, TargetWindow);
        }

        PlayMakerFSM WindowFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetWindow, "Window");
        WindowFsm.SendEvent("Common/Window/Show Window");
        PlayMakerFSM WindowDisplayFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetWindow, "Window Display");
        saveWindowObjList.Add(TargetWindow);
        saveWindowScriptList.Add(WindowDisplayFsm);
        WindowDisplayFsm.SendEvent("Highlight Panel/Highlight Panel");

        TargetWindow.transform.SetParent(HighlightPos1.transform);
        TargetWindow.transform.position = HighlightPos1.transform.position;
        if (windowName2 == "")
        {
            HighlightPos2.SetActive(false);
            return;
        }
        else
        {
            HighlightPos2.SetActive(true);

            if (WindowDict.ContainsKey(windowName2))
            {
                TargetWindow = WindowDict[windowName2];
            }
            else
            {
                TargetWindow = GameObject.Find(windowName2);
                WindowDict.Add(windowName2, TargetWindow);
            }
            TargetWindow.GetComponent<LeanConstrainToParent>().enabled = false;
            WindowFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetWindow, "Window");
            WindowFsm.SendEvent("Common/Window/Show Window");
            WindowDisplayFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetWindow, "Window Display");
            WindowDisplayFsm.SendEvent("Highlight Panel/Highlight Panel");
            saveWindowObjList.Add(TargetWindow);
            saveWindowScriptList.Add(WindowDisplayFsm);
            TargetWindow.transform.SetParent(HighlightPos2.transform);
            TargetWindow.transform.position = HighlightPos2.transform.position;
        }
    }

    public void HighLightObj(string objName)
    {
        Transform obj = GameObject.Find(objName).transform;
        if (highlightObjDict.ContainsKey(obj))
        {
            obj.transform.SetParent(highlightObjDict[obj]);
            highlightObjDict.Remove(obj);
        }
        else
        {
            highlightObjDict.Add(obj, obj.transform.parent);
            obj.transform.SetParent(TutorialLayer);
        }
    }

    public void ResetHighLightObj()
    {
        foreach(var item in highlightObjDict)
        {
            item.Key.SetParent(item.Value); 
        }
        highlightObjDict.Clear();
    }

    public void ResetWindowLayer()
    {
        for (int i=0;i< saveWindowObjList.Count;i++)
        {
            Debug.Log("Clear");
            saveWindowObjList[i].transform.SetParent(WindowLayer);
            saveWindowScriptList[i].SendEvent("Highlight Panel/Reset");
        }
        saveWindowObjList.Clear();
        saveWindowScriptList.Clear();
    }
}
