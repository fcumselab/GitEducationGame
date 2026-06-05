using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlSetting : MonoBehaviour
{
    [SerializeField] string url;
    private static UrlSetting instance;
    public static UrlSetting Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UrlSetting>();
            return instance;
        }
    }
    public string GetUrl()
    {
        return url; 
    }
}
