using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class EventTrackerManager : SerializedMonoBehaviour
{
    // http:/xxx.xxx.xxx.xxx:xxx/ 
    [SerializeField] string baseUrl;

    [SerializeField] bool debugMode;
    [SerializeField] float retryTriggerTime = 5f;
    [SerializeField] List<EventData> saveEventDataList = new();

    #region instance
    //Singleton instantation
    private static EventTrackerManager instance;
    public static EventTrackerManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<EventTrackerManager>();
            return instance;
        }
    }
    #endregion

    private void Start()
    {
        if (!debugMode)
        {
            StartCoroutine(RetryPostEventData());
        }
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            a++;
            Debug.Log("Click K");
            AddNewEvent("Testing", a.ToString());
        }*/
    }

    IEnumerator RetryPostEventData()
    {
        while (true)
        {
            yield return new WaitForSeconds(retryTriggerTime);

            if (saveEventDataList.Count > 0)
            {
                foreach (var eventData in saveEventDataList)
                {
                    StartCoroutine(PostEventData(eventData));
                }
            }
        }
    
    }

    public void AddNewEvent(string eventName, string eventResult)
    {
        if (!debugMode) {
            EventData newEventData = new();
            newEventData.player = SaveManager.Instance.userName;
            newEventData.eventName = eventName;
            newEventData.eventResult = eventResult;
            newEventData.gameScene = SceneManager.GetActiveScene().name;
            newEventData.eventTime = DateTime.UtcNow.ToString("o");
        
            saveEventDataList.Add(newEventData);

            StartCoroutine(PostEventData(newEventData));
        }
    }

    IEnumerator PostEventData(EventData newEventData)
    {
        WWWForm form = new();
        string json = JsonUtility.ToJson(newEventData);
        form.AddField("eventData", json);

        using (UnityWebRequest request = UnityWebRequest.Post($"{baseUrl}SendEventTracker", form))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                //Debug.Log("Failed: " + request.error);
            }
            else
            {
                saveEventDataList.Remove(newEventData);
                Debug.Log("Event upload success");
            }
        }
    }
}

[Serializable]
class EventData
{
    public string player;
    public string eventName;
    public string eventResult;
    public string gameScene;
    public string eventTime;
}
