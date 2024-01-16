using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

class Selection
{
    public bool isCorrect;
    public string message;
    public string reason;
}

public class SelectionPopup : SerializedMonoBehaviour
{
    enum SelectionType { BranchName, CommitMessage };
    [SerializeField] SelectionType selectionType;

    #region Popup UI
    [FoldoutGroup("Popup UI")]
    [SerializeField] PlayMakerFSM windowFsm;

    [FoldoutGroup("Popup UI/Button")]
    [SerializeField] GameObject SelectionButtonGroup;
    [FoldoutGroup("Popup UI/Button")]
    [SerializeField] List<GameObject> SelectionButtonList = new();
    [FoldoutGroup("Popup UI/Button")]
    [SerializeField] Button BackButton;

    [FoldoutGroup("Popup UI/Icon")]
    [SerializeField] GameObject IconCannotUse;
    [FoldoutGroup("Popup UI/Icon")]
    [SerializeField] GameObject IconWrong;
    [FoldoutGroup("Popup UI/Icon")]
    [SerializeField] GameObject IconCorrect;
    [FoldoutGroup("Popup UI/Icon")]
    [SerializeField] GameObject IconSelection;
    [FoldoutGroup("Popup UI/Icon")]
    [SerializeField] GameObject ActiveIcon;

    #endregion

    [SerializeField] InputField commandInputField;

    [FoldoutGroup("Popup Content")]
    [SerializeField] Dictionary<int, List<Selection>> SelectionDict = new();
    [FoldoutGroup("Popup Content")]
    [SerializeField] SelectionPopupButton correctAnswerButton;
    [FoldoutGroup("Popup Content")]
    [SerializeField] List<int> numList = new() { 0, 1, 2 };

    PlayMakerFSM targetContentFsm;
    bool needUpdate;

    public void InitializePopup(PlayMakerFSM contentFsm)
    {
        targetContentFsm = contentFsm;
        needUpdate = targetContentFsm.enabled;
        if (needUpdate)
        {
            if(selectionType == SelectionType.BranchName)
            {
                for (int i = 0; i < targetContentFsm.FsmVariables.GetFsmArray("branchNameActiveList").Length; i++)
                {
                    int questNum = (int)targetContentFsm.FsmVariables.GetFsmArray("branchNameActiveList").Get(i);
                    SelectionDict.Add(questNum, GetValuesFromFsm(i));
                }
            }
            else if (selectionType == SelectionType.CommitMessage)
            {
                for (int i = 0; i < targetContentFsm.FsmVariables.GetFsmArray("commitMessageActiveList").Length; i++)
                {
                    int questNum = (int)targetContentFsm.FsmVariables.GetFsmArray("commitMessageActiveList").Get(i);
                    SelectionDict.Add(questNum, GetValuesFromFsm(i));
                }
            }
        }

        for (int i = 0; i < SelectionButtonGroup.transform.childCount; i++)
        {
            SelectionButtonList.Add(SelectionButtonGroup.transform.GetChild(i).gameObject);
        }

        
    }

    List<Selection> GetValuesFromFsm(int index)
    {
        List<Selection> newList = new();

        for (int n = 0; n < 3; n++)
        {
            Selection newSelection = new();
            if (selectionType == SelectionType.BranchName)
            {
                newSelection.isCorrect = (bool)targetContentFsm.FsmVariables.GetFsmArray("branchNameAnswerList").Get(index * 4 + n);
                newSelection.message = targetContentFsm.FsmVariables.GetFsmArray("branchNameList").Get(index * 4 + n).ToString();
                newSelection.reason = targetContentFsm.FsmVariables.GetFsmArray("branchNameReasonList").Get(index * 4 + n).ToString();
            }
            else if (selectionType == SelectionType.CommitMessage)
            {
                newSelection.isCorrect = (bool)targetContentFsm.FsmVariables.GetFsmArray("commitMessageAnswerList").Get(index * 4 + n);
                newSelection.message = targetContentFsm.FsmVariables.GetFsmArray("commitMessageList").Get(index * 4 + n).ToString();
                newSelection.reason = targetContentFsm.FsmVariables.GetFsmArray("commitMessageReasonList").Get(index * 4 + n).ToString();
            }
            newList.Add(newSelection);
        }
        return newList;
    }

    public void OpenPopup()
    {
        windowFsm.SendEvent("Common/Window/Show Window");
        UpdatePopup();
    }

    public void ClosePopup()
    {
        windowFsm.SendEvent("Common/Window/Hide Window");
    }

    public void UpdatePopup()
    {
        int currentQuestnum = QuestTrackerManager.Instance.GetCurrentQuestNum();
        if (SelectionDict.ContainsKey(currentQuestnum))
        {
            numList.Shuffle();
            for(int i=0; i<numList.Count; i++)
            {
                Selection selection = SelectionDict[currentQuestnum][numList[i]];
                SelectionButtonList[i].SetActive(true);
                if (selection.isCorrect)
                {
                    correctAnswerButton = SelectionButtonList[i].GetComponent<SelectionPopupButton>().SetValue(this, selection.isCorrect, selection.message, selection.reason);
                }
                else
                {
                    SelectionButtonList[i].GetComponent<SelectionPopupButton>().SetValue(this, selection.isCorrect, selection.message, selection.reason);
                }
            }

            ChangeIcon(IconSelection);
            BackButton.interactable = false;
        }
        else
        {
            correctAnswerButton = null;
            foreach (GameObject item in SelectionButtonList)
            {
                item.SetActive(false);
            }
            ChangeIcon(IconCannotUse);
            BackButton.interactable = true;
        }
    }

    void ChangeIcon(GameObject selectedIcon)
    {
        if (ActiveIcon)
        {
            if(ActiveIcon != selectedIcon)
            {
                ActiveIcon.SetActive(false);
            }
        }

        selectedIcon.SetActive(true);
        ActiveIcon = selectedIcon;
    }

    public void ShowResult(bool isCorrect, Text correctText = null)
    {
        if (isCorrect)
        {
            ChangeIcon(IconCorrect);
            foreach(GameObject item in SelectionButtonList)
            {
                item.GetComponent<SelectionPopupButton>().ShowResult();
            }

            //Send to inputField
            if (!commandInputField)
            {
                commandInputField = GameObject.Find("CommandInputField").GetComponent<InputField>();
            }

            if (selectionType == SelectionType.BranchName)
            {
                commandInputField.text = $"git branch {correctText.text}";
            }
            else if (selectionType == SelectionType.CommitMessage)
            {
                commandInputField.text = $"git commit -m \"{correctText.text}\"";
            }
                
            BackButton.interactable = true;
        }
        else
        {
            ChangeIcon(IconWrong);
        }
    }

    //For Run Command
    public bool CheckSelectItemIsCorrectAnswer(string command)
    {
        int currentQuestnum = QuestTrackerManager.Instance.GetCurrentQuestNum();
        if (SelectionDict.ContainsKey(currentQuestnum))
        {
            if (selectionType == SelectionType.BranchName)
            {
                return (command == $"git branch {correctAnswerButton.GetCorrectText()}");
            }
            else if (selectionType == SelectionType.CommitMessage)
            {
                return (command == $"git commit -m \"{correctAnswerButton.GetCorrectText()}\"");
            }
        }

        return false;
    }

}
