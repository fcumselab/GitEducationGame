using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ImageManager : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, Sprite> IconDict = new Dictionary<string, Sprite>();
    [SerializeField] Dictionary<string, Sprite> TutorialImageDict = new Dictionary<string, Sprite>();

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

    public Sprite GetIconImage(string key)
    {
        if (IconDict.ContainsKey(key)) return IconDict[key];
        return null;
    }

    public Sprite GetTutorialImage(string key)
    {
        if (TutorialImageDict.ContainsKey(key)) return TutorialImageDict[key];
        return null;
    }
}
