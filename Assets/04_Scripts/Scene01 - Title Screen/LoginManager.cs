using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Newtonsoft.Json;

[Serializable]
public class LoginResultData
{
    public string status;
    public PlayerSaveData playerSaveData;
}

public class LoginManager : SerializedMonoBehaviour
{
    public string runResult;
    public string warningMessage;
    
    public void SignUpFunction(string username, string password)
    {
        runResult = "";
        warningMessage = "";
        WWWForm form = new();
        form.AddField("username", username);

        string encoderPassword = PasswordEncoder.GetMd5Hash(password);
        form.AddField("password", encoderPassword);

        StartCoroutine(SignUpRequest(form, (result) => {
            if (result.Contains("successful"))
            {
                LoginFunction(username, password);
            }
            else if (result.Contains("already sign up"))
            {
                runResult = "failed";
                warningMessage = "already sign up";
            }
            else if (result.Contains("Cannot connect to destination host"))
            {
                runResult = "failed";
                warningMessage = "Cannot connect";
            }
        }));
    }

    IEnumerator SignUpRequest(WWWForm form, Action<string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Post("localhost:5050/signUp", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            callback(www.error);
        }
        else
        {
            callback(www.downloadHandler.text);
        }
        www.Dispose();
    }

    public void LoginFunction(string username, string password)
    {
        runResult = "";
        warningMessage = "";

        WWWForm form = new();
        form.AddField("username", username);

        string encoderPassword = PasswordEncoder.GetMd5Hash(password);
        form.AddField("password", encoderPassword);
        Debug.Log("Login Start");

        StartCoroutine(LoginRequest(form, (result) => {

            Debug.Log("LogResult \n" + result);
            if (result.Contains("successful"))
            {
                LoginResultData loginResultData = JsonUtility.FromJson<LoginResultData>(result);
                SaveManager.Instance.LoadPlayerSaveData(username, loginResultData.playerSaveData);
                runResult = "successful";
            }
            else if (result.Contains("username not found"))
            {
                runResult = "failed";
                warningMessage = "username not found";
            }
            else if (result.Contains("password incorrect")){
                runResult = "failed";
                warningMessage = "password incorrect";
            }else if (result.Contains("Cannot connect to destination host"))
            {
                runResult = "failed";
                warningMessage = "Cannot connect";
            }
        }));

        Debug.Log("Login END");

    }

    IEnumerator LoginRequest(WWWForm form, Action<string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Post("localhost:5050/login", form);
        yield return www.SendWebRequest();
        Debug.Log("Send Losin");
        if (www.result != UnityWebRequest.Result.Success)
        {
            callback(www.error);
        }
        else
        {
            Debug.Log("END");

            callback(www.downloadHandler.text);
        }
        www.Dispose();
    }
}

