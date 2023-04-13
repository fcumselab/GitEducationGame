using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitFile : MonoBehaviour
{
    [SerializeField] bool isInitial;
    [SerializeField] GameObject imageAndText;

    //Singleton instantation
    private static GitFile instance;
    public static GitFile Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GitFile>();
            return instance;
        }
    }

    void Start()
    {
        isInitial = false;
        imageAndText.SetActive(false);
    }

    public bool GetInitial()
    {
        return isInitial;
    }

    public void SetInitial(bool status)
    {
        isInitial = status;
        CheckLocation();
    }

    public void CheckLocation()
    {
        if(FileManager.Instance.fileLocation != "D:\\Task")
        {
            imageAndText.SetActive(false);
        }
        else
        {
            imageAndText.SetActive(true);
        }
    }


}
