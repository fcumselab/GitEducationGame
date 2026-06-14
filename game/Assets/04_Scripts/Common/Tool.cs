using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tool : MonoBehaviour
{
    public string[] SplitStringIntoArray(string word, string keyword)
    {
        return word.Split(new string[] { keyword }, StringSplitOptions.None);
    }

    public string TrimString(string word, bool isTrimFront, bool isTrimBack)
    {
        if (isTrimFront) word = word.TrimStart();
        if (isTrimBack) word = word.TrimEnd();
        return word;
    }
    
    public string JoinArray(string[] contentList)
    {
        string result = string.Join("\\n", contentList);
        return result;
    }

    public string GetGameVersion()
    {
        return Application.version;
    }

    public void ScrollRectLockTargetContent(GameObject Target, GameObject Scroll, GameObject ContentPanel, string mode)
    {
        RectTransform targetRect = Target.GetComponent<RectTransform>();
        ScrollRect scrollRect = Scroll.GetComponent<ScrollRect>();
        RectTransform contentPanelRect = ContentPanel.GetComponent<RectTransform>();    

        Canvas.ForceUpdateCanvases();

        switch (mode) {
            case "x":
                float x = ((Vector2)scrollRect.transform.InverseTransformPoint(contentPanelRect.position) - (Vector2)scrollRect.transform.InverseTransformPoint(targetRect.position)).x;
                contentPanelRect.anchoredPosition = new(x, contentPanelRect.anchoredPosition.y);
                break;
            case "y":
                float y = ((Vector2)scrollRect.transform.InverseTransformPoint(contentPanelRect.position) - (Vector2)scrollRect.transform.InverseTransformPoint(targetRect.position)).y;
                contentPanelRect.anchoredPosition = new(contentPanelRect.anchoredPosition.x, y);
                break;
            case "xy":
                contentPanelRect.anchoredPosition = 
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanelRect.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(targetRect.position);
                break;
        }
    }

    
}
