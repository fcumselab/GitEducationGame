using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PlayerGameRecordsWindow : SerializedMonoBehaviour
{
    [FoldoutGroup("Button")]
    [SerializeField] Button PlayerGameRecordsButton;

    [FoldoutGroup("PlayerGameRecordsWindow")]
    [SerializeField] PlayMakerFSM WindowFsm;
    [FoldoutGroup("PlayerGameRecordsWindow/Children")]
    [SerializeField] GameObject MainRecords;
    [FoldoutGroup("PlayerGameRecordsWindow/Children")]
    [SerializeField] GameObject OtherRecords;
    [FoldoutGroup("PlayerGameRecordsWindow/Children")]
    [SerializeField] Text PlayerNameText;

    PlayerSaveData playerSaveData;

    private void Start()
    {
        PlayerGameRecordsButton.onClick.AddListener(() => OpenWindow());
    }

    void OpenWindow()
    {
        WindowFsm.SendEvent("Common/Window/Show Window");
        UpdateGameProgressData();
    }

    public void UpdateGameProgressData()
    {
        Transform recordPanel = OtherRecords.transform.GetChild(1);
        Text text = recordPanel.Find("Content").GetComponent<Text>();
        text.text = $"{playerSaveData.gameRecordData.totalTimesUsedGameManual}";
    }

    public void InitializeGameProgressData(SaveManager saveManager)
    {
        playerSaveData = saveManager.GetPlayerSaveData();
        PlayerNameText.text = saveManager.userName;
        for (int i = 0; i < MainRecords.transform.childCount; i++)
        {
            Transform recordPanel = MainRecords.transform.GetChild(i);
            Text text = recordPanel.Find("Content").GetComponent<Text>();
            switch (i)
            {
                case 0:
                    text.text = $"{playerSaveData.gameRecordData.totalGameProgress} %";
                    break;
                case 1:
                    text.text = $"{playerSaveData.gameRecordData.totalStageScore}";
                    break;
                case 2:
                    text.text = MyTimer.Instance.StopWatch(playerSaveData.gameRecordData.totalPlayTime);
                    break;
                case 3:
                    text.text = $"{playerSaveData.gameRecordData.totalStarCount}";
                    break;
            }
        }

        for (int i = 0; i < OtherRecords.transform.childCount; i++)
        {
            Transform recordPanel = OtherRecords.transform.GetChild(i);
            Text text = recordPanel.Find("Content").GetComponent<Text>();
            switch (i)
            {
                case 0:
                    text.text = $"{playerSaveData.gameRecordData.totalTimesStageClear}";
                    break;
                case 1:
                    text.text = $"{playerSaveData.gameRecordData.totalTimesUsedGameManual}";
                    break;
                case 2:
                    text.text = $"{playerSaveData.gameRecordData.totalCommandExecuteTimes}";
                    break;
                case 3:
                    text.text = $"{playerSaveData.gameRecordData.totalTimesQuestClearPerfect}";
                    break;
                case 4:
                    text.text = $"{playerSaveData.gameRecordData.totalTimesQuestClearGood}";
                    break;
                case 5:
                    text.text = $"{playerSaveData.gameRecordData.totalTimesQuestClearHint}";
                    break;
                case 6:
                    text.text = $"{playerSaveData.gameRecordData.totalTimesQuestClearAnswer}";
                    break;
            }
        }
    }
}
