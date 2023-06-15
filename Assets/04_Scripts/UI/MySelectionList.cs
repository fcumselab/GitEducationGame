using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySelectionList : MonoBehaviour
{
    [SerializeField] RectTransform GameScreenRectTransform;
    [SerializeField] RectTransform TooltipRectTransform;
    [SerializeField] RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowSelectionList()
    {
        float x = (float)Screen.width / 3840;
        float y = (float)Screen.height / 2160;

        Vector2 ScreenSize = new Vector2(x, y);
        Vector2 anchoredPos = Input.mousePosition / ScreenSize;

        if (anchoredPos.x + rectTransform.rect.width > GameScreenRectTransform.rect.width && anchoredPos.y + rectTransform.rect.height > GameScreenRectTransform.rect.height)
        {
            topRight();
            rectTransform.localPosition = new Vector2(-5, -5);
        }
        else if (anchoredPos.x + rectTransform.rect.width > GameScreenRectTransform.rect.width)
        {
            bottomRight();
            rectTransform.localPosition = new Vector2(-5, 5);
        }
        else if (anchoredPos.y + rectTransform.rect.height > GameScreenRectTransform.rect.height)
        {
            topLeft();
            rectTransform.localPosition = new Vector2(5, -5);
        }
        else
        {
            bottomLeft();
            rectTransform.localPosition = new Vector2(5, 5);
        }
        TooltipRectTransform.anchoredPosition = anchoredPos;
    }


    void topLeft()
    {
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
    }

    void topRight()
    {
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
    }

    void bottomLeft()
    {
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
    }

    void bottomRight()
    {
        rectTransform.anchorMin = new Vector2(1, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(1, 0);
    }
}

