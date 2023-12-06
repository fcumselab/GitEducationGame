using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManual : SerializedMonoBehaviour
{
    [SerializeField] List<GameManualData> playerGameManualData;

    [Header("FSMs")]
    [SerializeField] PlayMakerFSM FSMInitialManual;
    // Start is called before the first frame update
    void Start()
    {
        playerGameManualData = SaveManager.Instance.GetGameManualDataListFromPlayerData();

        FSMInitialManual = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Initial Manual");
        FSMInitialManual.enabled = true;
    }

    public void LoadGameManualData()
    {
        for (int i = 0; i < playerGameManualData.Count; i++)
        {
            GameManualData manualData = playerGameManualData[i];
            string type = manualData.manualType;
            //CommandNameList, RuleAndWindowNameList, VersionControlNameList...
            FsmArray nameList = FSMInitialManual.FsmVariables.FindFsmArray(type + "NameList");
            FsmArray unlockProgressList = FSMInitialManual.FsmVariables.FindFsmArray(type + "UnlockProgressList");

            for (int t = 0; t < manualData.items.Count; t++)
            {
                GameManualItem item = manualData.items[t];
                nameList.InsertItem(item.listName, t);
                unlockProgressList.InsertItem(item.listUnlockProgress, t);
            }
        }
    }

    public void SaveGameManualData(string[] typeList, string[] nameList, int[] unlockProgressList)
    {
        GameManualData CommandManual = playerGameManualData[0];
        GameManualData RuleAndWindowManual = playerGameManualData[1];
        GameManualData VersionControlManual = playerGameManualData[2];

        for (int i = 0; i < typeList.Length; i++)
        {
            GameManualItem findItem;
            switch (typeList[i])
            {
                case "C":
                    findItem = CommandManual.items.Find((item) => item.listName == nameList[i]);
                    break;
                case "RAW":
                    findItem = RuleAndWindowManual.items.Find((item) => item.listName == nameList[i]);
                    break;
                case "VC":
                    findItem = VersionControlManual.items.Find((item) => item.listName == nameList[i]);
                    break;
                default:
                    Debug.Log("SaveGameManualData Error!");
                    return;
            }

            findItem.listUnlockProgress = unlockProgressList[i];
        }
    }
}
