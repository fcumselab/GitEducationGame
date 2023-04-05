using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WindowManager : MonoBehaviour
{
    [SerializeField] GameObject tipTextBox;
    [SerializeField] GameObject tipTextBoxLocation;

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
        GameObject obj;
        obj = Instantiate(tipTextBox, transform.position, Quaternion.identity);
        obj.transform.SetParent(tipTextBoxLocation.transform);
        obj.transform.Find("Text").GetComponent<Text>().text = content;
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.name = "TipTextBox";
    } 

    public void CloseTipTextBox(GameObject obj)
    {
        Destroy(obj);
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
