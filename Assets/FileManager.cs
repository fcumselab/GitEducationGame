using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileManager : MonoBehaviour
{
    [SerializeField] Dictionary<string, List<NewFile>> fileLists = new Dictionary<string, List<NewFile>>();
    [SerializeField] List<NewFile> stagedFileLists = new List<NewFile>();

    public string fileLocation;
    [SerializeField] TextMeshProUGUI commandText;
    [SerializeField] TextMeshProUGUI fileSystemText;
    
    //Singleton instantation
    private static FileManager instance;
    public static FileManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<FileManager>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateFileLocationText();
        AddNewFile("aaa.txt", fileLocation);
        AddNewFile("bbb.txt", fileLocation);
        AddNewFile("ccc.txt", fileLocation);
        UpdateFileSystemUI();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddNewFile(string name, string location, string content = "")
    {
        NewFile newfile = new NewFile(name, location, content);
        if (fileLists.ContainsKey(location)) fileLists[location].Add(newfile);
        else
        {
            List<NewFile> newFileList = new List<NewFile>();
            newFileList.Add(newfile);
            fileLists.Add(location, newFileList);
        }
    }

    public void UpdateFileLocationText()
    {
        commandText.text = fileLocation;
        fileSystemText.text = fileLocation + "> ";
    }

    public void UpdateFileSystemUI()
    {
        Debug.Log("find: ");

        if (fileLists.ContainsKey(fileLocation)){
            for(int i=1; i<= fileLists[fileLocation].Count; i++)
            {
                Transform fileObject = transform.Find("file" + i);
                fileObject.gameObject.SetActive(true);
                fileObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = fileLists[fileLocation][i - 1].GetName();
            }
            Debug.Log("len: " + fileLists[fileLocation].Count);
        }
    }

    public void FindFile(string fileName)
    {
        if (fileLists.ContainsKey(fileLocation))
        {
            NewFile newfile = fileLists[fileLocation].Find(file => file.GetName() == fileName);
            if(newfile != null)
            {
                stagedFileLists.Add(newfile);
            }
            else
            {
                GitCommandController.Instance.AddFieldHistoryCommand("Cound't find " + fileName + " file.\n");
            }
        }
    }
}
