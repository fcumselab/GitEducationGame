using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilter_001_GameIntroduction_Tutorial : SerializedMonoBehaviour
{
    [SerializeField] QuestFilterManager questFilterManager;
    [SerializeField] List<string> i18nTranslateList;

    [SerializeField]
    Dictionary<string, List<int>> actionTagDict = new()
    {
        { "File/FileFunctionSelection", new List<int>() },
        { "FileContentWindow/AddButtonSelection", new List<int>() },
        { "FileContentWindow/RenameButtonSelection", new List<int>() },
        { "FileContentWindow/ModifyButtonSelection", new List<int>() },
        { "FileContentWindow/DeleteButtonSelection", new List<int>() },
    };

    [Header("Get from Sender")]
    public GameObject Sender;
    public string SenderFSMName;
    public int currentQuestNum;

    void Initializei18nList()
    {
        if (i18nTranslateList.Count == 0)
        {
            questFilterManager = transform.parent.GetComponent<QuestFilterManager>();
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Quest Filter");
            foreach (string text in fsm.FsmVariables.GetFsmArray("i18nTranslateList/Valider").Values)
            {
                i18nTranslateList.Add(text);
            }
        }
    }

    public string StartQuestFilter()
    {
        Initializei18nList();

        string runResult = "";
        List<int> actionTagList = actionTagDict[Sender.tag];
        //Run Git Command
        if (Sender.CompareTag("Window/CommandLineWindow/InputField"))
        {

        }
        else //Other action in File Manager/Content Window
        {
            if (actionTagList.Count == 0 || (actionTagList.Find((num) => num == currentQuestNum) == -1))
            {
                return $"{Sender.tag}/Wrong Quest";
            }
            else
            {
                switch (currentQuestNum)
                {
                    case 1:
                        return $"{Sender.tag}/Wrong Quest";
                    case 2:
                        if (Sender.CompareTag("File/FileFunctionSelection"))
                        {
                            return questFilterManager.DetectAction_CopyFile(Sender, SenderFSMName, "v1");
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                    case 3:
                        if (Sender.CompareTag("FileContentWindow/RenameButtonSelection"))
                        {
                            return questFilterManager.DetectAction_RenameFile(i18nTranslateList[0]);
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                    case 4:
                        if (Sender.CompareTag("FileContentWindow/DeleteButtonSelection"))
                        {
                            return questFilterManager.DetectAction_DeleteFile(Sender, SenderFSMName, i18nTranslateList[1], i18nTranslateList[3]);
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                    case 5:
                        if (Sender.CompareTag("File/FileFunctionSelection"))
                        {

                            return questFilterManager.DetectAction_CopyFile(Sender, SenderFSMName, "v2");
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                    case 6:
                        if (Sender.CompareTag("FileContentWindow/RenameButtonSelection"))
                        {
                            return questFilterManager.DetectAction_RenameFile(i18nTranslateList[0]);
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                    case 7:
                        if (Sender.CompareTag("FileContentWindow/ModifyButtonSelection"))
                        {
                            return questFilterManager.DetectAction_ModifyFile(Sender, SenderFSMName, i18nTranslateList[2], i18nTranslateList[4]);
                        }
                        else
                        {
                            return $"{Sender.tag}/Wrong Quest";
                        }
                }
            }
        }

        return runResult;
    }


}
