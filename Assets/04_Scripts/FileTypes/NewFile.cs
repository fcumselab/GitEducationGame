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
                fileIcon.sprite = ImageManager.Instance.GetIconImage("fileIconFolder");
                break;
            case FileDatas.FileType.Txt:
                fileIcon.sprite = ImageManager.Instance.GetIconImage("fileIconTxt");
                break;
        }

        switch (fileDatas.statusType) {
            case FileDatas.StatusType.Unstaged:
                statusIcon.sprite = ImageManager.Instance.GetIconImage("fileStatusUnstaged");
                statusIcon.color = new Color32(255, 168, 165, 255);
                break;
            case FileDatas.StatusType.Staged:
                statusIcon.sprite = ImageManager.Instance.GetIconImage("fileStatusStaged");
                statusIcon.color = new Color32(117, 196, 255, 255);
                break;
            case FileDatas.StatusType.Uploaded:
                statusIcon.sprite = ImageManager.Instance.GetIconImage("fileStatusUploaded");
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
        fileDatas.ClickEvent();
    }
    
}