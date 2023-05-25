using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer : MonoBehaviour
{
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
}
