using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class NewFile : SerializedMonoBehaviour
{
    public enum FileType { Txt, Folder }
    [SerializeField] Image icon;
    [SerializeField] FileType fileType;
    [SerializeField] string fileName;
    [SerializeField] int level;
    [SerializeField] string location;
    [SerializeField] string content;

    private void Start()
    {
        UpdateSprite();
    }

    public NewFile(string name, string location,int level, string content)
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
        fileName = name;
        this.level = level;
        this.location = location;
        this.content = content;
    }

    public void UpdateFileValue(NewFile file)
    {
        fileType = file.fileType;
        fileName = file.GetName();
        location = file.GetLocation();
        content = file.GetContent();
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (fileType == FileType.Folder)
        {
            icon.sprite = ImageManager.Instance.GetImage("fileIconFolder");
        }
        else if (fileType == FileType.Txt)
        {
            icon.sprite = ImageManager.Instance.GetImage("fileIconTxt");
        }
    }

    public void ClickEvent()
    {
        if (fileType == FileType.Folder)
        {
            //go to next folder, file history add.
            Debug.Log("folder");
            FileManager.Instance.fileLocationHistory.Add(fileName);
            FileManager.Instance.fileLocationSpot++;
            FileManager.Instance.GoToLocation(fileName);

        }
        else if (fileType == FileType.Txt)
        {
            Debug.Log("Txt");
        }
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
}