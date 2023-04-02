using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton instantation
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (GameObject.Find("Origin Game Manager")) Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Origin Game Manager";
        //Cursor.lockState = CursorLockMode.Confined;

    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown("2"))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    
}
