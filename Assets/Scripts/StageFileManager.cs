using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageFileManager : MonoBehaviour
{
    public List<NewFile> stagedFileLists = new List<NewFile>();
    public List<NewFile> unstagedFileLists = new List<NewFile>();

    [SerializeField] GameObject fieldStaged;
    [SerializeField] GameObject fieldUnstaged;

    //Singleton instantation
    private static StageFileManager instance;
    public static StageFileManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<StageFileManager>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateUI()
    {
        
        int i;
        for (i = 1; i <= unstagedFileLists.Count; i++)
        {
            Transform fileObject = fieldUnstaged.transform.Find("file" + i);
            fileObject.gameObject.SetActive(true);
            fileObject.GetComponent<NewFile>().UpdateFileValue(unstagedFileLists[i - 1]);
            fileObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = unstagedFileLists[i - 1].GetLocation() + "\\" + unstagedFileLists[i - 1].GetName();
        }
        for (; i <= 8; i++)
        {
            Transform fileObject = fieldUnstaged.transform.Find("file" + i);
            fileObject.gameObject.SetActive(false);
        }

        for (i = 1; i <= stagedFileLists.Count; i++)
        {
            Transform fileObject = fieldStaged.transform.Find("file" + i);
            fileObject.gameObject.SetActive(true);
            fileObject.GetComponent<NewFile>().UpdateFileValue(stagedFileLists[i - 1]);
            fileObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = stagedFileLists[i - 1].GetLocation() + "\\" + stagedFileLists[i - 1].GetName();
        }
        for (; i <= 8; i++)
        {
            Transform fileObject = fieldStaged.transform.Find("file" + i);
            fileObject.gameObject.SetActive(false);
        }
    }
}
