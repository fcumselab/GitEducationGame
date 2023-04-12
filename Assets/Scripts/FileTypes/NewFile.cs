using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class NewFile : SerializedMonoBehaviour
{
    public Image statusIcon;
    public Image fileIcon;

    [SerializeField] FileDatas fileDatas;

    private void Start()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        switch (fileDatas.fileType)
        {
            case FileDatas.FileType.Folder:
                fileIcon.sprite = ImageManager.Instance.GetImage("fileIconFolder");
                break;
            case FileDatas.FileType.Txt:
                fileIcon.sprite = ImageManager.Instance.GetImage("fileIconTxt");
                break;
        }

        switch (fileDatas.statusType) {
            case FileDatas.StatusType.Unstaged:
                statusIcon.sprite = ImageManager.Instance.GetImage("fileStatusUnstaged");
                statusIcon.color = new Color32(255, 168, 165, 255);

                break;
            case FileDatas.StatusType.Staged:
                statusIcon.sprite = ImageManager.Instance.GetImage("fileStatusStaged");
                break;
            case FileDatas.StatusType.Uploaded:
                statusIcon.sprite = ImageManager.Instance.GetImage("fileStatusUploaded");
                statusIcon.color = new Color32(109, 255, 165, 255);
                break;
        }

    }

    public void UpdateFileValue(FileDatas file)
    {
        fileDatas = file;
        UpdateSprite();
    }

    public void ClickEvent()
    {
        if (fileDatas.fileType == FileDatas.FileType.Folder)
        {
            //go to next folder, file history add.
            string newFileLocation = FileManager.Instance.fileLocation + "\\" + fileDatas.fileName;
            FileManager.Instance.fileLocationHistory.Add(newFileLocation);
            FileManager.Instance.fileLocationSpot++;
            
            FileManager.Instance.GoToLocation(newFileLocation);

        }
        else if (fileDatas.fileType == FileDatas.FileType.Txt)
        {

        }
    }

    
}