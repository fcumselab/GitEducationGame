using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTooltip : MonoBehaviour
{
    [SerializeField] bool isShow;
    [SerializeField] RectTransform GameScreenRectTransform;
    [SerializeField] RectTransform TooltipRectTransform;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow) { 
            Vector2 anchoredPos = Input.mousePosition / GameScreenRectTransform.localScale.x;

            if(anchoredPos.x + rectTransform.rect.width > GameScreenRectTransform.rect.width)
            {
                anchoredPos.x = GameScreenRectTransform.rect.width - rectTransform.rect.width;
            }

            if (anchoredPos.y + rectTransform.rect.height > GameScreenRectTransform.rect.height)
            {
                anchoredPos.y = GameScreenRectTransform.rect.height - rectTransform.rect.height;
            }

            TooltipRectTransform.anchoredPosition = anchoredPos;
        }
    }

    public void SetIsShow(bool isShow)
    {
        this.isShow = isShow;
    }
    
}
