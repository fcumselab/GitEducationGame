using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class GameManual : SerializedMonoBehaviour
{
    #region Variables
    [FoldoutGroup("Data")]
    [SerializeField] List<GameManualData> playerGameManualData;
    string currentSceneName = "";
    [FoldoutGroup("Fsms")]
    PlayMakerFSM WindowFsm;
    [FoldoutGroup("Fsms")]
    PlayMakerFSM UnlockAnimationFsm;

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
    public void InitializeGameManualData()
    {
        Debug.Log("InitializeGameManualData..");
        Debug.Log(SceneManager.GetActiveScene().name);

        if(SceneManager.GetActiveScene().name == "Play Game")
        {
            Debug.Log("h");
            DialogueSystemFeatureManager.Instance.RegisterFunctionGameManual(GetComponent<GameManual>());
        }

        //Set Name & Location
        GameObject gameManualPanel = GameObject.Find("GameManualPanel");
        transform.SetParent(gameManualPanel.transform);
        transform.localScale = new(1, 1, 1);
        name = "GameManualWindow";

        WindowFsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "Window");
        UnlockAnimationFsm = MyPlayMakerScriptHelper.GetFsmByName(gameObject, "UnlockMessage");

        gameManualButton = GameObject.Find("GameManualBtn");
        gameManualButtonFsm = MyPlayMakerScriptHelper.GetFsmByName(gameManualButton, "Update Button");

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
                    //Debug.Log("Please add new prefab:" + item.listName);
                    newItem = Instantiate(LockGameManualItem);
                }
                else
                {
                    string listName = item.listName;
                    newItem = UnlockItemContent(categoryKey, createLocation, listName, true);
                }
            }
            else
            {
                newItem = Instantiate(LockGameManualItem);
            }

            SetNewItemLocation(newItem, item.listName, createLocation);
        }

    }
    
    public void RegisterFunction(bool enable)
    {
        if (enable)
        {
            Lua.RegisterFunction("UnlockGameManualItem", this, SymbolExtensions.GetMethodInfo(() => DialogueUnlockGameManualItem(string.Empty)));
        }
        else
        {
            Lua.UnregisterFunction("UnlockGameManualItem");
        }
    }

    #endregion

    public bool CheckPlayerHasUnlockCommand(string contentKey, int currentPageNum)
    {
        GameManualItem foundItem = playerGameManualData[0].items.Find((item) => item.listName == contentKey);
        if (foundItem != null)
        {
            return (foundItem.listUnlockProgress >= currentPageNum);
        }
        
        return false;
    }


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
            GameManualItem introduceManualItem = RuleAndWindowItemList.items.Find((item) => item.listName == "IntroGameManual");
            Debug.Log("Found : " + introduceManualItem);
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
    public void DialogueUnlockGameManualItem(string unlockKey)
    {
        // type/key/num, type/key/num...
        //type -> C, RAW, VC...
        //key -> git, git add, git reset...
        //num -> 1, 2, 3
        //use ',' divide mult unlock key.
        bool newUnlockItem = false;
        string[] unlockItemList = unlockKey.Split(",");
        foreach(string unlockItem in unlockItemList)
        {
            GameManualItem fouundItem = null;
            GameObject generateLocation = null;
            string categoryKey = "";

            string[] type = unlockItem.Split("/");
            switch (type[0])
            {
                case "C":
                    fouundItem = playerGameManualData[0].items.Find((item) => item.listName == type[1]);
                    generateLocation = CommandCategoryList;
                    categoryKey = "Command";
                    break;
                case "RAW":
                    fouundItem = playerGameManualData[1].items.Find((item) => item.listName == type[1]);
                    generateLocation = RuleAndWindowCategoryList;
                    categoryKey = "RuleAndWindow";
                    break;
                case "VC":
                    fouundItem = playerGameManualData[2].items.Find((item) => item.listName == type[1]);
                    generateLocation = VersionControlCategoryList;
                    categoryKey = "VersionControl";
                    break;
                default:
                    Debug.LogError("Not found target manual type.");
                    break;
            }

            if (fouundItem != null)
            {
                int unlockNum = int.Parse(type[2]);
                Debug.Log("Translate: " + unlockNum);
                if(unlockNum > fouundItem.listUnlockProgress)
                {
                    Debug.Log("New ! Change: " + unlockNum + " to: " + fouundItem.listUnlockProgress);
                    newUnlockItem = true;
                    if (fouundItem.listUnlockProgress == 0)
                    {
                        Debug.Log("New GameManualButton !!!");
                        GameObject newItem = UnlockItemContent(categoryKey, generateLocation, type[1], false);
                        SetNewItemLocation(newItem, type[1], generateLocation);
                    }
                    fouundItem.listUnlockProgress = unlockNum;
                }
            }
            else
            {
                Debug.LogError("Not found target manual key.");
            }
        }

        if (newUnlockItem)
        {
            UnlockAnimationFsm.SendEvent("GameManualWindow/Popup Notification");
            UpdateButtonStatus();
        }
    }

    void SetNewItemLocation(GameObject newItem, string listName, GameObject createLocation)
    {
        newItem.name = listName;
        newItem.transform.SetParent(createLocation.transform);
        newItem.transform.localScale = new(1, 1, 1);
    }

    GameObject UnlockItemContent(string categoryKey, GameObject createLocation, string listName, bool isInitial)
    {
        int newItemIndex = -1;
        if (!isInitial)
        {
            Transform oldItem = createLocation.transform.Find(listName);
            if(oldItem != null)
            {
                newItemIndex = oldItem.GetSiblingIndex();
                Destroy(oldItem.gameObject);
            }
        }

        //Add new ListItem
        GameObject newItem = Instantiate(UnlockGameManualItem);
        Button button = newItem.GetComponent<GameManualListItemButton>().InitializeButton(GetComponent<GameManual>(), listName, categoryKey);

        //Add new ContentItem
        GameObject ItemContent = Instantiate(GameManualContentDict[listName]);
        ItemContent.name = listName;
        ItemContent.transform.SetParent(ItemContentLocation.transform);
        ItemContent.transform.localScale = new(1, 1, 1);
        ItemContent.SetActive(false);
        ItemContent.GetComponent<GameManualContentItem>().InitializeContent(GetComponent<GameManual>(), listName, categoryKey);

        //Add two into Dict
        UnlockGameManualItemDict.Add(button, ItemContent);

        if (!isInitial)
        {
            ItemContent.transform.SetSiblingIndex(newItemIndex);
        }

        return newItem;
    }

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
