using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class FileManager : SerializedMonoBehaviour
{
    [SerializeField] int MaxFileNumbers;

    [SerializeField] Dictionary<string, List<FileDatas>> fileLists = new Dictionary<string, List<FileDatas>>();

    [Header("FileLocation")]
    public string fileLocation;
    public int fileLocationSpot = 0;
    [SerializeField] public List<string> fileLocationHistory = new List<string>();

    public PageButton PageButtonUp;

    [SerializeField] TextMeshProUGUI commandText;
    [SerializeField] TextMeshProUGUI fileSystemText;
    [SerializeField] RectTransform fieldInputTextBox;
    
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
        
        AddNewFile("aLoc", fileLocation,0);
        AddNewFile("aa.txt", fileLocation + "\\aLoc", 1);

        AddNewFile("bLoc", fileLocation,0);
        AddNewFile("baLoc", fileLocation + "\\bLoc", 1);
        AddNewFile("b.txt", fileLocation + "\\bLoc", 1);
        AddNewFile("baa.txt", fileLocation + "\\bLoc\\baLoc", 2);

        AddNewFile("1.txt", fileLocation, 0);
        AddNewFile("2.txt", fileLocation,0);

        UpdateFileSystemUI();
        //GitCommandController.Instance.RunCommand("git init");
        
        //GitCommandController.Instance.RunCommand("git add 1");
        //GitCommandController.Instance.RunCommand("git commit -m add_aLoc");
        /*
        GitCommandController.Instance.RunCommand("git add bLoc");
        GitCommandController.Instance.RunCommand("git commit -m add_bLoc");

        GitCommandController.Instance.RunCommand("git add 1");
        */

    }

    public void GoToLocation(string location)
    {
        PageButtonUp.UpdateButton(fileLocationSpot, fileLocationHistory.Count);
        fileLocation = location;

        GitFile.Instance.CheckLocation();
        UpdateFileLocationText();
        UpdateFileSystemUI();
    }

    public void AddNewFile(string name, string location,int level = 0, string content = "")
    {

        FileDatas newfile = ScriptableObject.CreateInstance<FileDatas>();
        newfile.InitValue(name, location, level, content);
        if (fileLists.ContainsKey(location)) fileLists[location].Add(newfile);
        else
        {
            List<FileDatas> newFileList = new();
            newFileList.Add(newfile);
            fileLists.Add(location, newFileList);
        }

        if (name.Split(".").Length != 1) StageFileManager.Instance.unstagedFileLists.Add(newfile); //Folder
    }

    public void UpdateFileLocationText()
    {
        commandText.text = fileLocation;
        fieldInputTextBox.offsetMin = new Vector2(22 + commandText.text.Length * 22, fieldInputTextBox.offsetMin.y);
        fileSystemText.text = fileLocation + "> ";
    }

    public void UpdateFileSystemUI()
    {
        if (gameObject.activeSelf)
        {
            int i = 1;
            if (fileLists.ContainsKey(fileLocation))
            {
                for (; i <= fileLists[fileLocation].Count; i++)
                {
                    Transform fileObject = transform.Find("file" + i);
                    fileObject.gameObject.SetActive(true);
                    fileObject.GetComponent<NewFile>().UpdateFileValue(fileLists[fileLocation][i - 1]);
                    fileObject.GetComponentInChildren<Text>().text = fileLists[fileLocation][i - 1].GetName();
                }
            }
            for (; i <= MaxFileNumbers; i++)
            {
                Transform fileObject = transform.Find("file" + i);
                fileObject.gameObject.SetActive(false);
            }
        }
    }

    public List<string> FindFile(string type, string keyword = "")
    {
        List<string> result = new List<string>();

        if (type == "cd")
        {
            List<FileDatas> findList = fileLists[fileLocation].FindAll(file => file.GetFileType() == "folder" && file.GetName().StartsWith(keyword));
            foreach (FileDatas f in findList) result.Add("cd " + f.GetName());
        }
        else if (type == "add" || type == "reset")
        {
            List<FileDatas> findList = fileLists[fileLocation].FindAll(file => file.GetName().StartsWith(keyword));
            foreach (FileDatas f in findList) result.Add("git " + type + " " + f.GetName());
        }
        return result;
    }

    public void FindFile(string fileName, string type, string location)
    {
        if (fileLists.ContainsKey(location))
        {
            FileDatas newfile = fileLists[location].Find(file => (file.GetName() == fileName || file.GetName().Split(".")[0] == fileName));
            try
            {
                if (newfile.GetName() != "")
                {
                    if (type == "add")
                    {
                        if (newfile.GetFileType() == "folder") FindFile(".", "add", newfile.GetLocation() + "\\" + newfile.GetName());
                        else if (StageFileManager.Instance.unstagedFileLists.Exists(file => (file.GetName() == fileName || file.GetName().Split(".")[0] == fileName) && file.GetLocation() == location))
                        {
                            StageFileManager.Instance.MoveToStageList(newfile, fileName, location);
                        }
                        else GitCommandController.Instance.AddFieldHistoryCommand("Already add " + fileName + " file.\n");
                    }
                    else if (type == "reset")
                    {
                        if (newfile.GetFileType() == "folder") FindFile(".", "reset", newfile.GetLocation() + "\\" + newfile.GetName());
                        else if (StageFileManager.Instance.stagedFileLists.Exists(file => (file.GetName() == fileName || file.GetName().Split(".")[0] == fileName) && file.GetLocation() == location))
                        {
                            StageFileManager.Instance.MoveToUnstageList(newfile, fileName, location);
                        }
                        else GitCommandController.Instance.AddFieldHistoryCommand("Not found " + fileName + " file.\n");
                    }else if(type == "cd")
                    {
                        newfile.ClickEvent();
                    }
                }
            }
            catch
            {
                if (fileName == ".")
                {
                    foreach (FileDatas f in fileLists[location])
                    {
                        if (type == "add")
                        {
                            if (f.GetFileType() == "folder") FindFile(".", "add", f.GetLocation() + "\\" + f.GetName());
                            else if (StageFileManager.Instance.unstagedFileLists.Exists(file => (file.GetName() == f.GetName() && file.GetLocation() == location)))
                            {
                                StageFileManager.Instance.MoveToStageList(f, f.GetName(), location);
                            }
                        }
                        else if (type == "reset")
                        {
                            if (f.GetFileType() == "folder") FindFile(".", "reset", f.GetLocation() + "\\" + f.GetName());
                            else if (StageFileManager.Instance.stagedFileLists.Exists(file => (file.GetName() == f.GetName() && file.GetLocation() == location)))
                            {
                                StageFileManager.Instance.MoveToUnstageList(f, f.GetName(), location);
                            }
                        }
                    }
                }
                else
                {
                    if(type == "cd") GitCommandController.Instance.AddFieldHistoryCommand("Cannot find the path.\n");
                    else GitCommandController.Instance.AddFieldHistoryCommand("Cannot find " + fileName + " file.\n");
                }
            }
        }
    }

    
}

