using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer : MonoBehaviour
{
    //Singleton instantation
    private static MyTimer instance;
    public static MyTimer Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<MyTimer>();
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string StopWatch(float timer)
    {
        int min = (int)(timer / 60);
        int sec = (int)(timer % 60);
        if(min >= 10 && sec >= 10)
        {
            return min + ":" + sec;
        }
        else if (min >= 10 && sec < 10)
        {
            return min + ":" + "0" + sec;
        }
        else if(min < 10 && sec < 10)
        {
            return "0" + min + ":" + "0" + sec;
        }
        else if(min < 10 && sec >= 10)
        {
            return "0" + min + ":" + sec;
        }
        return "error";
    }

    public int ChangeTimeToSec(string timeStr)
    {
        string[] splitTime = timeStr.Split(':');
        int min = Int32.Parse(splitTime[0]);
        int sec = Int32.Parse(splitTime[1]);


        return min * 60 + sec;
    }
}
