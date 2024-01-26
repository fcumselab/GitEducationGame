using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;
using UnityEngine.EventSystems;

public class MouseTooltipManager : SerializedMonoBehaviour
{
    [FoldoutGroup("Game Screen")]
    [SerializeField] RectTransform GameScreenRectTransform;
    [FoldoutGroup("Game Screen")]
    [SerializeField] Canvas gameScreenCanvas;

    [SerializeField] float alphaTime = 0.15f;
    [SerializeField] bool isActivate = false;
    [SerializeField] bool isShow = false;
    [SerializeField] LeanLocalizedText i18nText;
    [SerializeField] Text DefaultText;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    RectTransform tooltipRectTransform;

    //Singleton instantation
    private static MouseTooltipManager instance;
    public static MouseTooltipManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<MouseTooltipManager>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        tooltipRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            UpdateTooltipAlpha();
            Vector2 anchoredPos = Input.mousePosition / gameScreenCanvas.scaleFactor;
            float x = (float)Screen.width / 1920;
            float y = (float)Screen.height / 1080;

            if (anchoredPos.x + (tooltipRectTransform.rect.width * gameScreenCanvas.scaleFactor) > GameScreenRectTransform.rect.width)
            {
                anchoredPos.x = GameScreenRectTransform.rect.width - (tooltipRectTransform.rect.width * gameScreenCanvas.scaleFactor);
            }

            if (anchoredPos.y + (tooltipRectTransform.rect.height * gameScreenCanvas.scaleFactor) > GameScreenRectTransform.rect.height)
            {
                anchoredPos.y = GameScreenRectTransform.rect.height - (tooltipRectTransform.rect.height * gameScreenCanvas.scaleFactor);
            }

            rectTransform.anchoredPosition = anchoredPos;
        }
    }

    void UpdateTooltipAlpha()
    {
        //Show animation
        if (isShow)
        {
            if(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * alphaTime;
            }
        }
        else //Hide Animation
        {
            if(canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaTime;
            }else
            {
                isActivate = false;
            }
        }
    }

    public void ShowTooltip(string tooltipMessage, bool isI18nKey)
    {
        if (isI18nKey)
        {
            if (!i18nText.gameObject.activeSelf)
            {
                i18nText.gameObject.SetActive(true);
                DefaultText.gameObject.SetActive(false);
            }
            i18nText.TranslationName = tooltipMessage;
        }
        else
        {
            if (!DefaultText.gameObject.activeSelf)
            {
                i18nText.gameObject.SetActive(false);
                DefaultText.gameObject.SetActive(true);
            }
            DefaultText.text = tooltipMessage;
        }
        isActivate = true;
        isShow = true;
    }

    public void HideTooltip()
    {
        isShow = false;
    }
}
