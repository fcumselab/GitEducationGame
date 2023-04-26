using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ImageManager : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, Sprite> IconDict = new Dictionary<string, Sprite>();

    //Singleton instantation
    private static ImageManager instance;
    public static ImageManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<ImageManager>();
            return instance;
        }
    }

    public Sprite GetImage(string key)
    {
        if (IconDict.ContainsKey(key))
        {
            return IconDict[key];
        }

        return null;
    }
}
