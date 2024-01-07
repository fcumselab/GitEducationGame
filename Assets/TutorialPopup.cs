using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class TutorialPopup : SerializedMonoBehaviour
{
    [SerializeField] bool isDialogOpen = false;
    [SerializeField] Dictionary<string, GameObject> WindowDict = new();
    [SerializeField] Transform WindowLayer;

    [SerializeField] PlayMakerFSM tutorialPopupAnimationFsm;


    [SerializeField] GameObject ImageWindow;
    [SerializeField] Image ImageIcon;
    [SerializeField] PlayMakerFSM ImageWindowAnimationFsm;

    [SerializeField] GameObject HighlightPos1;
    [SerializeField] GameObject HighlightPos2;

    [SerializeField] GameObject BlackPanel;
    [SerializeField] PlayMakerFSM BlackPanelFsm;

    public void RegisterFunction()
    {
        Lua.RegisterFunction("HighlightWindow", this, SymbolExtensions.GetMethodInfo(() => HighLightWindow(string.Empty, string.Empty)));
        Lua.RegisterFunction("ShowImage", this, SymbolExtensions.GetMethodInfo(() => ShowImage(string.Empty)));
        Lua.RegisterFunction("CloseImageWindow", this, SymbolExtensions.GetMethodInfo(() => CloseImageWindow()));
        Lua.RegisterFunction("BlackPanelControl", this, SymbolExtensions.GetMethodInfo(() => BlackPanelControl(false)));
    }

    public void ShowImage(string imageKey)
    {
        ResetWindowLayer();
        Debug.Log("Show Image");
        if (ImageWindow.GetComponent<CanvasGroup>().alpha != 1)
        {
            ImageWindowAnimationFsm.FsmVariables.GetFsmString("runType").Value = "open";
            ImageWindowAnimationFsm.enabled = true;
        }
        ImageIcon.sprite = ImageManager.Instance.GetTutorialImage(imageKey);
    }

    public void CloseImageWindow()
    {
        Debug.Log("Close Image");
        ImageWindowAnimationFsm.FsmVariables.GetFsmString("runType").Value = "close";
        ImageWindowAnimationFsm.enabled = true;
    }

    public void BlackPanelControl(bool open)
    {
        Debug.Log("Test");
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
        
        HighlightPos1.GetComponent<RectTransform>().sizeDelta = TargetWindow.GetComponent<RectTransform>().sizeDelta;
        TargetWindow.transform.SetParent(HighlightPos1.transform);

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
            HighlightPos2.GetComponent<RectTransform>().sizeDelta = TargetWindow.GetComponent<RectTransform>().sizeDelta;
            TargetWindow.transform.SetParent(HighlightPos2.transform);
        }
    }

    public void ResetWindowLayer()
    {
        if (HighlightPos1.transform.childCount != 0)
        {
            HighlightPos1.transform.GetChild(0).SetParent(WindowLayer);
        }

        if (HighlightPos2.transform.childCount != 0)
        {
            HighlightPos2.transform.GetChild(0).SetParent(WindowLayer);
        }
    }

    
}
