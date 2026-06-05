using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;
using System;
using System.Linq;

public class I18nTranslatorManager : SerializedMonoBehaviour
{
    [Header("Data")]
    //Enter i18n keys before run game.
    [SerializeField] Dictionary<string, string> i18nSourceTextDict = new();

    [SerializeField] bool needLog;
    bool isWaitingForResult = false;

   

    [SerializeField] Text text;
    [SerializeField] LeanLocalizedText leanLocalizeScript;

    DateTime startTime;

    //Singleton instantation
    private static I18nTranslatorManager instance;
    public static I18nTranslatorManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<I18nTranslatorManager>();
            return instance;
        }
    }

    void Start()
    {
        Debug.Log("Run I18nTranslatorManager script");
        if(!text || !leanLocalizeScript)
        {
            text = transform.Find("i18nTextTranslator").GetComponent<Text>();
            leanLocalizeScript = text.GetComponent<LeanLocalizedText>();
        }
        
        StartCoroutine(TranslateText());
    }

    IEnumerator TranslateText()
    {
        foreach(string keyword in i18nSourceTextDict.Keys.ToList())
        {
            Debug.Log("New tranlate keyword: " + keyword);

            isWaitingForResult = true;
            startTime = DateTime.Now;
            leanLocalizeScript.TranslationName = keyword;

            while (isWaitingForResult)
            {
                Debug.Log("Waiting...");

                if (text.text != "Waiting...")
                {
                    i18nSourceTextDict[keyword] = text.text;
                    isWaitingForResult = false;
                }else if((DateTime.Now - startTime).TotalSeconds > 1.5f)
                {
                    Debug.LogError("i18n Translator Time out!\n" + "Keyword: " + keyword);
                    i18nSourceTextDict[keyword] = "Failed to Load i18n text!";
                    isWaitingForResult = false;
                }
                yield return null;
            }
        }
    }

    public string GetTranslatedText(string keyword)
    {
        if (i18nSourceTextDict.ContainsKey(keyword))
        {
            return i18nSourceTextDict[keyword];
        }
        else
        {
            return "Failed to Load i18n text!";
        }
    }
}
