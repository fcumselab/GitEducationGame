using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class FileManager : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, List<NewFile>> fileLists = new Dictionary<string, List<NewFile>>();
    [SerializeField] List<NewFile> stagedFileLists = new List<NewFile>();

    [Header("FileLocation")]
    public string fileLocation;
    public int fileLocationSpot = 0;
    [SerializeField] public List<string> fileLocationHistory = new List<string>();

    [SerializeField] PageButton PageButtonUp;

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
        fileLocationSpot = 0;
        fileLocationHistory.Add(fileLocation);

        UpdateFileLocationText();
        AddNewFile("1.txt", fileLocation,0);
        AddNewFile("aLoc", fileLocation,0);
        AddNewFile("bLoc", fileLocation,0);
        AddNewFile("baLoc", "bLoc", 1);
        AddNewFile("b.txt", "bLoc", 1);

        AddNewFile("2.txt", fileLocation,0);


        UpdateFileSystemUI();
    }

    public void GoToLocation(string location)
    {
       
        PageButtonUp.UpdateButton(fileLocationSpot, fileLocationHistory.Count);
        fileLocation = location;

        UpdateFileLocationText();
        UpdateFileSystemUI();
    }

    public void AddNewFile(string name, string location,int level = 0, string content = "")
    {
        
        NewFile newfile = new(name, location, level,  content);
        
        if (fileLists.ContainsKey(location)) fileLists[location].Add(newfile);
        else
        {
            List<NewFile> newFileList = new();
            newFileList.Add(newfile);
            fileLists.Add(location, newFileList);
        }
    }

    public void UpdateFileLocationText()
    {
        string newLocation = "";
        for(int i=0; i< fileLocationHistory.Count-1;i++)
        {
            newLocation += (fileLocationHistory[i] + "\\");
        }
        commandText.text = (newLocation + fileLocation);
        fileSystemText.text = (newLocation + fileLocation + "> ");
    }

    public void UpdateFileSystemUI()
    {
        int i = 1;
        if (fileLists.ContainsKey(fileLocation)){
            for(; i<= fileLists[fileLocation].Count; i++)
            {
                Transform fileObject = transform.Find("file" + i);
                fileObject.gameObject.SetActive(true);
                fileObject.GetComponent<NewFile>().UpdateFileValue(fileLists[fileLocation][i-1]);
                fileObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = fileLists[fileLocation][i - 1].GetName();
            }
        }
        for (; i <= 8; i++)
        {
            Transform fileObject = transform.Find("file" + i);
            fileObject.gameObject.SetActive(false);
        }
    }

    public void FindFile(string fileName)
    {
        if (fileLists.ContainsKey(fileLocation))
        {
            NewFile newfile = fileLists[fileLocation].Find(file => file.GetName() == fileName);
            try{
                if(newfile.GetName() != "") stagedFileLists.Add(newfile);
            }
            catch
            {
                GitCommandController.Instance.AddFieldHistoryCommand("Cound't find " + fileName + " file.\n");
            }
        }
    }
}
