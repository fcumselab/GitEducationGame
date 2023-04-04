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

    public void ShowTipTextBox(string content)
    {
        //Debug.Log(tipTextBoxs.Count);

        foreach (GameObject obj in tipTextBoxs)
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
