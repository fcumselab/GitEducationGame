using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class FileManager : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, List<NewFile>> fileLists = new Dictionary<string, List<NewFile>>();

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

    void Start()
    {
        fileLocationSpot = 0;
        fileLocationHistory.Add(fileLocation);

        UpdateFileLocationText();
        AddNewFile("1.txt", fileLocation,0);
        AddNewFile("aLoc", fileLocation,0);
        AddNewFile("aa.txt", fileLocation + "\\aLoc", 1);


        AddNewFile("bLoc", fileLocation,0);
        AddNewFile("baLoc", fileLocation + "\\bLoc", 1);
        AddNewFile("baa.txt", fileLocation + "\\bLoc\\baLoc", 2);


        AddNewFile("b.txt", fileLocation + "\\bLoc", 1);

        AddNewFile("2.txt", fileLocation,0);

        UpdateFileSystemUI();
        StageFileManager.Instance.UpdateUI();

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

        if (name.Split(".").Length != 1) StageFileManager.Instance.unstagedFileLists.Add(newfile); //Folder
    }

    public void UpdateFileLocationText()
    {
        commandText.text = fileLocation;
        fileSystemText.text = fileLocation + "> ";
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

    public void FindFile(string fileName, string type)
    {
        if (fileLists.ContainsKey(fileLocation))
        {
            NewFile newfile = fileLists[fileLocation].Find(file => file.GetName() == fileName);
            try{
                if (newfile.GetName() != "")
                {
                    if (type == "add")
                    {
                        if (StageFileManager.Instance.unstagedFileLists.Exists(file => (file.GetName() == fileName && file.GetLocation() == fileLocation)))
                        {
                            StageFileManager.Instance.stagedFileLists.Add(newfile);
                            for (int i = 0; i < StageFileManager.Instance.unstagedFileLists.Count; i++)
                            {
                                if (StageFileManager.Instance.unstagedFileLists[i].GetName() == fileName && StageFileManager.Instance.unstagedFileLists[i].GetLocation() == fileLocation)
                                {
                                    StageFileManager.Instance.unstagedFileLists.RemoveAt(i);
                                    break;
                                }
                            }
                            StageFileManager.Instance.UpdateUI();
                        }
                        else GitCommandController.Instance.AddFieldHistoryCommand("Already add" + fileName + " file.\n");
                    }
                    else if (type == "reset")
                    {
                        if (StageFileManager.Instance.stagedFileLists.Exists(file => (file.GetName() == fileName && file.GetLocation() == fileLocation)))
                        {
                            StageFileManager.Instance.unstagedFileLists.Add(newfile);
                            for (int i = 0; i < StageFileManager.Instance.stagedFileLists.Count; i++)
                            {
                                if (StageFileManager.Instance.stagedFileLists[i].GetName() == fileName && StageFileManager.Instance.stagedFileLists[i].GetLocation() == fileLocation)
                                {
                                    StageFileManager.Instance.stagedFileLists.RemoveAt(i);
                                    break;
                                }
                            }
                            StageFileManager.Instance.UpdateUI();
                        }
                        else GitCommandController.Instance.AddFieldHistoryCommand("Not found " + fileName + " file.\n");
                    }
                }
            }
            catch
            {
                GitCommandController.Instance.AddFieldHistoryCommand("Cound't find " + fileName + " file.\n");
            }
        }
    }
}
