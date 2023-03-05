using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageButton : MonoBehaviour
{
    private Image image;
    private Button button;
    enum ButtonType { Up, Down };
    [SerializeField] private ButtonType buttonType;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ClickButton()
    {
        if (buttonType == ButtonType.Up)
        { 
            FileManager.Instance.fileLocationHistory.RemoveAt(FileManager.Instance.fileLocationSpot);
            FileManager.Instance.fileLocationSpot--;
        }

        FileManager.Instance.GoToLocation(FileManager.Instance.fileLocationHistory[FileManager.Instance.fileLocationSpot]);
        UpdateButton(FileManager.Instance.fileLocationSpot, FileManager.Instance.fileLocationHistory.Count);
    }

    public void UpdateButton(int nowPage, int pageCount)
    {
        if (buttonType == ButtonType.Up && (nowPage == 0)) button.interactable = false;
        else button.interactable = true;
    }
}
