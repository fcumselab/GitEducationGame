using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManual : SerializedMonoBehaviour
{
    #region Variables
    [FoldoutGroup("Data")]
    [SerializeField] List<GameManualData> playerGameManualData;
    [FoldoutGroup("Data")]
    PlayMakerFSM WindowFsm;

    [FoldoutGroup("Game Manual Items")]
    [SerializeField] GameObject gameManualButton;
    PlayMakerFSM gameManualButtonFsm;
    [FoldoutGroup("Game Manual Items")]
    [SerializeField] Button CommandCategoryButton;
    [FoldoutGroup("Game Manual Items")]
    [SerializeField] GameObject DefaultContent;

    [FoldoutGroup("Reference")]
    GitCommandValider gitCommandValider;

    #region Generate Location
    [FoldoutGroup("Generate Location")]
    [SerializeField] GameObject CommandCategoryList;
    [FoldoutGroup("Generate Location")]
    [SerializeField] GameObject RuleAndWindowCategoryList;
    [FoldoutGroup("Generate Location")]
    [SerializeField] GameObject VersionControlCategoryList;
    [FoldoutGroup("Generate Location")]
    [SerializeField] GameObject ItemContentLocation;
    #endregion

    #region Prefabs
    [FoldoutGroup("Prefabs")]
    [SerializeField] GameObject LockGameManualItem;
    [FoldoutGroup("Prefabs")]
    [SerializeField] GameObject UnlockGameManualItem;
    [FoldoutGroup("Prefabs")]
    [SerializeField] Dictionary<string, GameObject> GameManualContentDict = new();
    #endregion

    [SerializeField] Dictionary<Button, GameObject> UnlockGameManualItemDict = new();

    Button SelectedButton;
    Button SelectedCategoryButton;
    #endregion

    #region Initialize
    void Start()
    {
        //Set Name & Location
        GameObject gameManualPanel = GameObject.Find("GameManualPanel");
        transform.SetParent(gameManualPanel.transform);
        transform.localScale = new(1, 1, 1);
        name = "GameManualWindow";

        WindowFsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Window");

        gameManualButton = GameObject.Find("GameManualBtn");
        gameManualButtonFsm = MyPlayMakerScriptHelper.GetFsmByName(gameManualButton, "Update Button");
        //FSMInitialManual = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Initial Manual");
        //FSMInitialManual.enabled = true;

        InitializeGameManualData();
    }

    public void InitializeGameManualData()
    {
        playerGameManualData = SaveManager.Instance.GetGameManualDataListFromPlayerData();

        InitializeCategoryItem("Command", CommandCategoryList);
        InitializeCategoryItem("RuleAndWindow", RuleAndWindowCategoryList);
        InitializeCategoryItem("VersionControl", VersionControlCategoryList);
        UpdateButtonStatus();
    }

    void InitializeCategoryItem(string categoryKey, GameObject createLocation)
    {
        GameManualData manualData = playerGameManualData.Find((category) => category.manualType == categoryKey);
        foreach (GameManualItem item in manualData.items)
        {
            GameObject newItem;
            if (item.listUnlockProgress > 0)
            {
                //Add new ItemContent
                if (!GameManualContentDict.ContainsKey(item.listName)) //Debug
                {
                    Debug.Log("Please add new prefab:" + item.listName);
                    newItem = Instantiate(LockGameManualItem);
                }
                else
                {
                    GameObject ItemContent = Instantiate(GameManualContentDict[item.listName]);
                    ItemContent.name = item.listName;
                    ItemContent.transform.SetParent(ItemContentLocation.transform);
                    ItemContent.transform.localScale = new(1, 1, 1);
                    ItemContent.SetActive(false);
                    //Add new ListItem
                    newItem = Instantiate(UnlockGameManualItem);
                    Button button = newItem.GetComponent<GameManualItemButton>().InitializeButton(GetComponent<GameManual>(), item.listName, categoryKey);

                    UnlockGameManualItemDict.Add(button, ItemContent);
                }
            }
            else
            {
                newItem = Instantiate(LockGameManualItem);
            }
            newItem.name = item.listName;
            newItem.transform.SetParent(createLocation.transform);
            newItem.transform.localScale = new(1, 1, 1);
        }

    }
    #endregion

    #region Button Action
    public void SwitchGameManualCategory(Button clickCategoryButton)
    {
        if (SelectedButton)
        {
            SelectedButton.interactable = true;
            UnlockGameManualItemDict[SelectedButton].SetActive(false);
            SelectedButton = null;
        }

        if (SelectedCategoryButton)
        {
            SelectedCategoryButton.interactable = true;
            SelectedCategoryButton.GetComponent<GameManualCategoryButton>().ActivateCategoryList(false);
        }

        if (!DefaultContent.activeSelf)
        {
            DefaultContent.SetActive(true);
        }
        clickCategoryButton.interactable = false;
        SelectedCategoryButton = clickCategoryButton;
        clickCategoryButton.GetComponent<GameManualCategoryButton>().ApplyColor();
        SelectedCategoryButton.GetComponent<GameManualCategoryButton>().ActivateCategoryList(true);
    }

    public void SwitchGameManualItem(Button clickButton)
    {
        if (DefaultContent.activeSelf)
        {
            DefaultContent.SetActive(false);
        }

        if (SelectedButton)
        {
            SelectedButton.interactable = true;
            UnlockGameManualItemDict[SelectedButton].SetActive(false);
        }
        clickButton.interactable = false;
        UnlockGameManualItemDict[clickButton].SetActive(true);
        SelectedButton = clickButton;
    }

    void OpenWindow()
    {
        WindowFsm.SendEvent("Common/Window/Show Window");
        SwitchGameManualCategory(CommandCategoryButton);
    }

    public void UpdateButtonStatus()
    {
        if (gameManualButtonFsm.FsmVariables.GetFsmString("runType").Value != "open")
        {
            GameManualData RuleAndWindowItemList = playerGameManualData[1];
            GameManualItem introduceManualItem = RuleAndWindowItemList.items.Find((item) => item.listName == "Introduce GameManualWindow");

            if (introduceManualItem.listUnlockProgress > 0)
            {
                gameManualButtonFsm.FsmVariables.GetFsmString("runType").Value = "open";
                gameManualButton.GetComponent<Button>().onClick.AddListener(() => OpenWindow());
            }
            else
            {
                gameManualButtonFsm.FsmVariables.GetFsmString("runType").Value = "close";
            }
            gameManualButtonFsm.enabled = true;
        }
    }

    #endregion

    #region Unlock new Game Manual Item
    public void SaveGameManualData(string[] typeList, string[] nameList, int[] unlockProgressList)
    {
        if (!gitCommandValider)
        {
            gitCommandValider = GameObject.Find("GitCommandValider").GetComponent<GitCommandValider>();
        }
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

        gitCommandValider.UpdatePlayerUnlockDict();
        UpdateButtonStatus();
    }
    #endregion
}
