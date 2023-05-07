using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "MyProject/FileDatas", menuName = "ScriptableObjects/File")]
public class FileDatas : SerializedScriptableObject
{
    public enum FileType { Txt, Folder }
    public enum StatusType { Unstaged, Staged, Uploaded }

    [Header("FileStatus")]
    public StatusType statusType;

    [Header("FileContents")]

    public FileType fileType;
    public string fileName;
    public int level;
    public string location;
    public string content;

    public void InitValue(string name, string location, int level, string content)
    {
        string[] list = name.Split('.');
        if (list.Length == 1)
        {
            fileType = FileType.Folder;
        }
        else if (list.Length == 2 && list[1] == "txt")
        {
            fileType = FileType.Txt;
        }
        statusType = StatusType.Unstaged;
        fileName = name;
        this.level = level;
        this.location = location;
        this.content = content;
    }

    
    public string GetName()
    {
        return fileName;
    }

    public string GetLocation()
    {
        return location;
    }

    public string GetContent()
    {
        return content;
    }

    public string GetFileType()
    {
        if (fileType == FileType.Folder) return "folder";
        else if (fileType == FileType.Txt) return "txt";
        return "";
    }

    public void SetStatus(string type)
    {
        switch (type)
        {
            case "unstaged":
                statusType = StatusType.Unstaged;
                break;
            case "staged":
                statusType = StatusType.Staged;
                break;
            case "uploaded":
                statusType = StatusType.Uploaded;
                break;
        }
    }

    public void ClickEvent()
    {
        if (fileType == FileType.Folder)
        {
            //go to next folder, file history add.
            string newFileLocation = FileManager.Instance.fileLocation + "\\" + fileName;
            FileManager.Instance.fileLocationHistory.Add(newFileLocation);
            FileManager.Instance.fileLocationSpot++;

            FileManager.Instance.GoToLocation(newFileLocation);
        }
        else if (fileType == FileType.Txt)
        {

        }
    }
}
