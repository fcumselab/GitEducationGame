using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLogger : MonoBehaviour
{
    public static WebLogger Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void LogToWebConsole(string message)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        // 1. 先把 C# 字串中的 \ 變成 \\，把 ' 變成 \'，防止破壞 JS 語法
        string safeMessage = message.Replace("\\", "\\\\").Replace("'", "\\'");

        // 2. 透過 String.raw 讓瀏覽器原封不動地印出所有特殊字元（包括 / 與 \）
        Application.ExternalEval("console.log('%c [unity-fsm] ' + String.raw`" + safeMessage + "`, 'color: #00ffff; font-weight: bold; font-size: 14px;');");
        #else
             Debug.Log("[unity-fsm] " + message);
        #endif
    }

}
