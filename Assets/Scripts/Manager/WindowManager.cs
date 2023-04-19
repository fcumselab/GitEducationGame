using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class WindowManager : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, GameObject> windowObjects = new Dictionary<string, GameObject>();

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

    public bool CheckWindowOpen(string key, bool wantedOpen)
    {
        if (windowObjects.ContainsKey(key))
        {
            if (windowObjects[key].activeSelf)
            {
                if (!wantedOpen)
                {
                    windowObjects[key].SetActive(false);
                    return false;
                }
                return true;
            }
            else
            {
                if (wantedOpen)
                {
                    windowObjects[key].SetActive(true);
                    return true;
                }
                return false;
            }
        }
        Debug.Log("Error, Cannot find key: " + key);
        return false;
    }
}
