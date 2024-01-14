using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Lean.Localization;

public class GameManualContentItem : SerializedMonoBehaviour
{
    GameManual gameManual;
    string currentSceneName = "";
    bool isInitial = false;

    enum ButtonType {Command, RuleAndWindow, VersionControl}
    [SerializeField] ButtonType buttonType;
    
    [SerializeField] string contentKey;

    #region Only Command buttonType need Reference
    [FoldoutGroup("Command Selection")]
    [SerializeField] int currentPageNum = 0;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Text CurrentPageNumText;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Text MaxPageNumText;
    [FoldoutGroup("Command Selection")]
    [SerializeField] LeanLocalizedText CommnandDetailedText;
    [FoldoutGroup("Command Selection")]
    [SerializeField] GameObject LockIcon;
    [FoldoutGroup("Command Selection")]
    [SerializeField] GameObject Star;
    [FoldoutGroup("Command Selection")]
    [SerializeField] GameObject CommandDetailedPageGroup;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Button PageUpButton;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Button PageDownButton;
    [FoldoutGroup("Command Selection")]
    [SerializeField] List<GameObject> CommandList = new();
    GameObject lastOpenCommand;
    #endregion 

    private void OnEnable()
    {
        if (isInitial)
        {
            if (buttonType == ButtonType.Command)
            {
                currentPageNum = 1;
                SwitchContent(currentPageNum);
            }
        }
    }

    public void InitializeContent(GameManual gameManual, string itemKey, string categoryKey)
    {
        this.gameManual = gameManual;
        contentKey = itemKey;
        switch (categoryKey)
        {
            case "Command":
                buttonType = ButtonType.Command;
                currentSceneName = SceneManager.GetActiveScene().name;
                PageUpButton.onClick.AddListener(() => PageUp());
                PageDownButton.onClick.AddListener(() => PageDown());
                for (int i = 0; i < CommandDetailedPageGroup.transform.childCount; i++)
                {
                    Transform Command = CommandDetailedPageGroup.transform.GetChild(i);
                    CommandList.Add(Command.gameObject);
                }
                MaxPageNumText.text = $"{CommandDetailedPageGroup.transform.childCount}";

                currentPageNum = 1;
                SwitchContent(currentPageNum);
                break;
            case "RuleAndWindow":
                buttonType = ButtonType.RuleAndWindow;
                break;
            case "VersionControl":
                buttonType = ButtonType.VersionControl;
                break;
        }

        //Debug.Log("End!!");

        isInitial = true;
    }

    void PageUp()
    {
        currentPageNum--;
        SwitchContent(currentPageNum);
    }

    void PageDown()
    {
        currentPageNum++;
        SwitchContent(currentPageNum);
    }

    void SwitchContent(int targetPageNum)
    {
        CurrentPageNumText.text = $"{targetPageNum}";
        CommnandDetailedText.TranslationName = ($"GameManualItem/content/{contentKey}/commandDetail/{targetPageNum}");

        if (lastOpenCommand)
        {
            lastOpenCommand.SetActive(false);
        }
        
        CommandList[targetPageNum - 1].SetActive(true);
        lastOpenCommand = CommandList[targetPageNum - 1];
        UpdateButtonStatus();
    }

    void UpdatePageUp()
    {
        if (currentPageNum == 1)
        {
            PageUpButton.interactable = false;
        }
        else
        {
            PageUpButton.interactable = true;
        }
    }

    void UpdatePageDown()
    {
        if (currentPageNum == CommandList.Count)
        {
            PageDownButton.interactable = false;
            LockIcon.SetActive(false);
        }
        else
        {
            bool isUnlock = gameManual.CheckPlayerHasUnlockCommand(contentKey, currentPageNum + 1);
            if (!isUnlock)
            {
                LockIcon.SetActive(true);
                PageDownButton.interactable = false;
            }
            else
            {
                LockIcon.SetActive(false);
                PageDownButton.interactable = true;
            }

        }
    }

    void UpdateStar()
    {
        if (currentSceneName == "Play Game")
        {
            Star.SetActive(StageManager.Instance.CheckGameManualContentItemUseInStage(contentKey, currentPageNum));
        }
        else
        {
            Star.SetActive(false);
        }
    }

    void UpdateButtonStatus()
    {
        UpdatePageUp();
        UpdateStar();
        UpdatePageDown();
    }
}
