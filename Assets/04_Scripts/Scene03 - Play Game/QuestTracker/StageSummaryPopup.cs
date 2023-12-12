using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSummaryPopup : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject PrefabQuestHistoryTextBox;

    [Header("Children")]
    //stage quest history list
    [SerializeField] GameObject StageQuestHistoryPanel;
    //self leaderboard clear times
    [SerializeField] GameObject ClearTimesContent;
    //three histroy
    [SerializeField] GameObject SelfLeaderBoardGroup;
    //four history
    [SerializeField] GameObject playerCurrentScoreTextBox;

    [Header("Reference")]
    [SerializeField] GameObject StageManagerParent;

    // Start is called before the first frame update
    private void Start()
    {
        StageManagerParent = GameObject.Find("Stage Manager Parent");
    }

    public void RunSummaryFunc(string stageName, float time, int playerScore)
    {
        CreateQuestHistoryTextBoxes();
        int playerPlace = CompareLeaderBoard(time, playerScore);
        int playerStar = GetPlayerStar(playerScore);

        //update fourth leaderboard
        UpdatePlayerCurrentScoreTextBox(time, playerScore, playerPlace, playerStar, playerCurrentScoreTextBox);

        StageLeaderboardData newData = new StageLeaderboardData
        {
            playerName = SaveManager.Instance.userName,
            playerScore = playerScore,
            playerStar = playerStar,
            playerClearTime = (int)time
        };

        SaveManager.Instance.ClearTheStage(stageName, playerPlace, newData);

        //update three leaderboard
        GameDataManager.Instance.UpdateSelfLeaderBoardContent(ClearTimesContent, SelfLeaderBoardGroup, playerPlace);
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
        List<string> completeQuestNameList = GameDataManager.Instance.GetCompleteQuestName();
        List<int> completeQuestTimeList = GameDataManager.Instance.GetCompleteQuestTime();
        List<int> completeQuestUsedTimeList = GameDataManager.Instance.GetCompleteQuestUsedTime();
        List<string> completeQuestTypeList = GameDataManager.Instance.GetCompleteQuestType();

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
}
