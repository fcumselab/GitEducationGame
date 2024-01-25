using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class StageSummaryPopup : SerializedMonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject PrefabQuestHistoryTextBox;

    #region Children 
    [FoldoutGroup("Children")]
    [SerializeField] SelfLeaderboard selfLeaderboard;
    [FoldoutGroup("Children/Left")]
    //LeftTop
    [SerializeField] GameObject StarsAndScorePanel;
    [FoldoutGroup("Children/Left")]
    //LeftBottom
    [SerializeField] GameObject ActionDetailGroup;

    [FoldoutGroup("Children/Right")]
    //RightBottom
    [SerializeField] GameObject StageQuestHistoryPanel;
    [FoldoutGroup("Children/Right")]
    //RightTop
    [SerializeField] GameObject ClearTimesContent;
    [FoldoutGroup("Children/Right")]
    //RightTop
    [SerializeField] GameObject SelfLeaderBoardGroup;
    [FoldoutGroup("Children/Right")]
    //RightTop
    [SerializeField] GameObject playerCurrentScoreTextBox;

    #endregion

    [FoldoutGroup("Managers")]
    [SerializeField] GameManagerPlayGame gameManager;
    [FoldoutGroup("Managers")]
    [SerializeField] GameObject gameDataManager;
    [FoldoutGroup("Managers")]
    [SerializeField] GameDataManager gameDataManagerScript;

    [FoldoutGroup("Button")]
    [SerializeField] Button replayButton;
    [FoldoutGroup("Button")]
    [SerializeField] Button backToStageSelectionButton;

    // Start is called before the first frame update
    private void Start()
    {
        replayButton.onClick.AddListener(() => GoToSceneButtonAction("replay"));
        backToStageSelectionButton.onClick.AddListener(() => GoToSceneButtonAction("stageSelection"));
        gameDataManager = GameObject.Find("Game Data Manager");
        gameDataManagerScript = gameDataManager.GetComponent<GameDataManager>();
    }

    #region Button Action 
    void GoToSceneButtonAction(string targetKey)
    {
        replayButton.interactable = false;
        backToStageSelectionButton.interactable = false;
        switch (targetKey)
        {
            case "replay":
                gameManager.GoToPlayGameScene();
                break;
            case "stageSelection":
                gameManager.GoToStageSelectScene();
                break;
            default:
                Debug.LogError("Not found Correct switchSceneKey");
                break;
        }
    }
    #endregion

    public void Initialize(StageData stageData)
    {
        selfLeaderboard.SetSelectedStageData(stageData);
    }

    public void RunSummaryFunc(string stageName, float time, int playerScore)
    {
        //Get Player Place & Star
        int playerPlace = CompareLeaderBoard(time, playerScore);
        int playerStar = GetPlayerStar(playerScore);

        //Update Left Bottom Panel
        UpdateStarsAndScorePanel(playerStar, playerScore, playerPlace);

        //Update Left Bottom Panel
        UpdateActionDetailsPanel(time);

        //Update Right Bottom Panel
        CreateQuestHistoryTextBoxes();


        StageLeaderboardData newData = new StageLeaderboardData
        {
            playerName = SaveManager.Instance.userName,
            playerScore = playerScore,
            playerStar = playerStar,
            playerClearTime = (int)time
        };

        //Update StageData
        SaveManager.Instance.ClearTheStage(time, stageName, playerPlace, newData);

        //update Three Leaderboard UI & Player Current Panel (Fourth)
        UpdatePlayerCurrentScoreTextBox(time, playerScore, playerPlace, playerStar, playerCurrentScoreTextBox);
        selfLeaderboard.UpdateSelfLeaderBoardContent(playerPlace);
    }

    void UpdateStarsAndScorePanel(int playerStar, int playerScore, int playerPlace)
    {
        Transform Stars = StarsAndScorePanel.transform.Find("Stars");
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
        fsm.FsmVariables.GetFsmInt("getStarNum").Value = playerStar;
        fsm.enabled = true;

        Transform Text = StarsAndScorePanel.transform.Find("Score/TotalScoreText");
        Text.GetComponent<Text>().text = $"{playerScore}";

        Transform NewRecordBox = StarsAndScorePanel.transform.Find("Score/NewRecordBox");
        NewRecordBox.gameObject.SetActive((playerPlace != 4));
    }

    int CompareLeaderBoard(float time, int playerScore)
    {
        List<StageLeaderboardData> SelfLeaderBoardList = GameDataManager.Instance.GetSelfLeaderBoardData();
        int place = 1;
        foreach (StageLeaderboardData item in SelfLeaderBoardList)
        {
            if (playerScore > item.playerScore)
            {
                break;
            }
            else if (playerScore == item.playerScore)
            {
                if (time < item.playerClearTime)
                {
                    break;
                }
            }
            place++;
        }
        return place;
    }

    int GetPlayerStar(int playerScore)
    {
        // Get correct star count
        GameObject StageManager = GameObject.Find("Stage Manager");
        List<int> getStarScoreLine = StageManager.GetComponent<StageManager>().getStarScoreLine;
        int starCount = 4;
        foreach (int starScoreLine in getStarScoreLine)
        {
            if (playerScore >= starScoreLine) break;
            else starCount--;
        }
        return starCount;
    }

    void CreateQuestHistoryTextBoxes()
    {
        List<string> completeQuestNameList = gameDataManagerScript.GetCompleteQuestName();
        List<int> completeQuestTimeList = gameDataManagerScript.GetCompleteQuestTime();
        List<int> completeQuestUsedTimeList = gameDataManagerScript.GetCompleteQuestUsedTime();
        List<string> completeQuestTypeList = gameDataManagerScript.GetCompleteQuestType();

        //create historyTextBox and give values.
        for (int i = 0; i < completeQuestNameList.Count; i++)
        {
            GameObject cloneObj = Instantiate(PrefabQuestHistoryTextBox, StageQuestHistoryPanel.transform);
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(cloneObj, "Initial Quest UI");
            fsm.FsmVariables.GetFsmString("runType").Value = completeQuestTypeList[i];
            fsm.enabled = true;
            Text Text = cloneObj.transform.Find("TextBox/QuestNamePanel/Quest Name").GetComponent<Text>();
            Text.text = completeQuestNameList[i];
            Text = cloneObj.transform.Find("TextBox/TimePanel/completedTime/TotalTimeText").GetComponent<Text>();
            Text.text = MyTimer.Instance.StopWatch(completeQuestTimeList[i]);
            Text = cloneObj.transform.Find("TextBox/TimePanel/usedTime/usedTimeText").GetComponent<Text>();
            Text.text = MyTimer.Instance.StopWatch(completeQuestUsedTimeList[i]);
        }
    }

    void UpdatePlayerCurrentScoreTextBox(float time, int playerScore, int playerPlace, int playerStar, GameObject playerTextBox)
    {
        // Update Content
        Text text = playerTextBox.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
        text.text = $"{playerPlace}";

        text = playerTextBox.transform.Find("TextPanel/Name Text").GetComponent<Text>();
        text.gameObject.SetActive(true);
        text.text = SaveManager.Instance.userName;

        Transform Stars = playerTextBox.transform.Find("Stars");
        PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
        fsm.FsmVariables.GetFsmInt("getStarNum").Value = playerStar;
        fsm.enabled = true;

        text = playerTextBox.transform.Find("Score & Time/Score/Score Text").GetComponent<Text>();
        text.text = $"{playerScore}";
        text = playerTextBox.transform.Find("Score & Time/Time/Time Text").GetComponent<Text>();
        text.text = MyTimer.Instance.StopWatch(time);

        fsm = MyPlayMakerScriptHelper.GetFsmByName(playerTextBox, "Highlight TextBox");
        fsm.FsmVariables.GetFsmBool("needHighlight").Value = true;
        fsm.enabled = true;
    }

    public void UpdateActionDetailsPanel(float time)
    {
        PlayMakerFSM fsm;
        int count;
        for (int i=0; i< ActionDetailGroup.transform.childCount; i++)
        {
            Transform ResultColumn = ActionDetailGroup.transform.GetChild(i);
            Transform ContentText = ResultColumn.Find("ContentPanel/first/Content Text");
            Transform ScoreText = ResultColumn.Find("ContentPanel/second/Score Text");
            
            switch (i)
            {
                case 0: //TotalStageTime
                    ContentText.GetComponent<Text>().text = $"{MyTimer.Instance.StopWatch(time)}";
                    ScoreText.gameObject.SetActive(false);
                    break;
                case 1: //MissTimes
                    fsm = MyPlayMakerScriptHelper.GetFsmByName(gameDataManager, "GameData");
                    ContentText.GetComponent<Text>().text = $"{fsm.FsmVariables.GetFsmInt("missNum").Value}";
                    ScoreText.gameObject.SetActive(false);
                    break;
                case 2: //CommandExecutedTimes
                    ContentText.GetComponent<Text>().text = $"{gameDataManagerScript.GetCommandExecuteTime()}";
                    ScoreText.gameObject.SetActive(false);
                    break;
                case 3: //GameManualUsedTimes
                    ContentText.GetComponent<Text>().text = $"{gameDataManagerScript.GetGameManualUsedTimes()}";
                    ScoreText.gameObject.SetActive(false);
                    break;
                case 4:
                    count = gameDataManagerScript.GetQuestCountPerfect();
                    ContentText.GetComponent<Text>().text = $"{count}";
                    ScoreText.GetComponent<Text>().text = $"+{count * 1000}";
                    ScoreText.gameObject.SetActive(true);
                    break;
                case 5:
                    count = gameDataManagerScript.GetQuestCountGood();
                    ContentText.GetComponent<Text>().text = $"{count}";
                    ScoreText.GetComponent<Text>().text = $"+{count * 750}";
                    ScoreText.gameObject.SetActive(true);
                    break;
                case 6:
                    count = gameDataManagerScript.GetQuestCountHint();
                    ContentText.GetComponent<Text>().text = $"{count}";
                    ScoreText.GetComponent<Text>().text = $"+{count * 500}";
                    ScoreText.gameObject.SetActive(true);
                    break;
                case 7:
                    count = gameDataManagerScript.GetQuestCountAnswer();
                    ContentText.GetComponent<Text>().text = $"{count}";
                    ScoreText.GetComponent<Text>().text = $"+{0}";
                    ScoreText.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
