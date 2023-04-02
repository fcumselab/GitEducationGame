using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WindowManager : MonoBehaviour
{
    [SerializeField] bool isEntering;
    [SerializeField] int activeTipTextBox;
    [SerializeField] List<GameObject> tipTextBoxs;

    //Singleton instantation
    private static WindowManager instance;
    public static WindowManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<WindowManager>();
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

    public void ShowTipTextBox(string content)
    {
        foreach(GameObject obj in tipTextBoxs)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                obj.transform.Find("Text").GetComponent<Text>().text = content;
                break;
            }
        }
        
        //tipTextBoxs[activeTipTextBox].SetActive(true);
        //tipTextBoxs[activeTipTextBox].GetComponent<Animator>().Play("TipTextBoxShow");
        //activeTipTextBox++;
    } 

    public void CloseTipTextBox(GameObject obj)
    {
        obj.SetActive(false);
        //obj.GetComponent<Animator>().Play("TipTextBoxClose");
        //activeTipTextBox--;
    }
    public void SetObjectActive(GameObject obj)
    {

        if (obj.activeSelf)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.SetActive(true);
        }
    }

}
