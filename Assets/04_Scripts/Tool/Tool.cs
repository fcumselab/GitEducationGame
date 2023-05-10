using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] SplitStringIntoArray(string word, string keyword)
    {
        return word.Split(new string[] { keyword }, StringSplitOptions.None);
    }
}
