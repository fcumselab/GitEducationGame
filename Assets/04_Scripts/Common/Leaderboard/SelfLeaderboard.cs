using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class SelfLeaderboard : SerializedMonoBehaviour
{

    [FoldoutGroup("SelfLeaderBoard")]
    [SerializeField] GameObject SelfLeaderBoardGroup;
    [FoldoutGroup("SelfLeaderBoard")]
    [SerializeField] GameObject ClearTimesContent;

    [SerializeField] StageData selectedStageData;

    public void SetSelectedStageData(StageData stageData)
    {
        selectedStageData = stageData;
    }

    public void UpdateSelfLeaderBoardContent(int highlightIndex = -1)
    {
        Debug.Log("UPdaet self LB");
        List<StageLeaderboardData> stageLeaderboardData = selectedStageData.stageLeaderboardData;
        ClearTimesContent.GetComponent<Text>().text = selectedStageData.stageClearTimes.ToString();

        int index = 0;
        foreach (StageLeaderboardData item in stageLeaderboardData)
        {
            Transform selfLeaderBoardItem = SelfLeaderBoardGroup.transform.GetChild(index);
            bool hasRecord = (item.playerScore != 0);

            Text text = selfLeaderBoardItem.transform.Find("PlacePanel/Place Text").GetComponent<Text>();
            text.text = $"{index + 1}";

            if (hasRecord)
            {
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text").GetComponent<Text>();
                Debug.Log("Has record" + text.name);
                text.gameObject.SetActive(true);
                text.text = item.playerName;
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text (None)").GetComponent<Text>();
                Debug.Log("Has record" + text.name);

                text.gameObject.SetActive(false);
            }
            else
            {
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text").GetComponent<Text>();
                text.gameObject.SetActive(false);
                text = selfLeaderBoardItem.transform.Find("TextPanel/Name Text (None)").GetComponent<Text>();
                text.gameObject.SetActive(true);
            }

            Transform Stars = selfLeaderBoardItem.transform.Find("Stars");
            PlayMakerFSM fsm = MyPlayMakerScriptHelper.GetFsmByName(Stars.gameObject, "Update Star");
            fsm.FsmVariables.GetFsmInt("getStarNum").Value = item.playerStar;
            fsm.enabled = true;

            text = selfLeaderBoardItem.transform.Find("Score & Time/Score/Score Text").GetComponent<Text>();
            text.text = $"{item.playerScore}";
            text = selfLeaderBoardItem.transform.Find("Score & Time/Time/Time Text").GetComponent<Text>();
            text.text = MyTimer.Instance.StopWatch(item.playerClearTime);

            if (highlightIndex != -1)
            {
                fsm = MyPlayMakerScriptHelper.GetFsmByName(selfLeaderBoardItem.gameObject, "Highlight TextBox");
                fsm.FsmVariables.GetFsmBool("needHighlight").Value = (index == highlightIndex - 1);
                fsm.enabled = true;
            }

            index++;
        }
    }
}


