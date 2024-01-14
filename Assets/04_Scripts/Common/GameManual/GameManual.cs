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
        if(SceneManager.GetActiveScene().name == "Play Game")
        {
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
                    newItem = UnlockItemContent(categoryKey, createLocation, listName);
                }
            }
            else
            {
                newItem = Instantiate(LockGameManualItem);
            }

            SetNewItemLocation(newItem, item.listName, createLocation, true);
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
                if(unlockNum > fouundItem.listUnlockProgress)
                {
                    newUnlockItem = true;
                    if (fouundItem.listUnlockProgress == 0)
                    {
                        GameObject newItem = UnlockItemContent(categoryKey, generateLocation, type[1]);
                        SetNewItemLocation(newItem, type[1], generateLocation, false);
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

            if (!gitCommandValider)
            {
                gitCommandValider = GameObject.Find("GitCommandValider").GetComponent<GitCommandValider>();
            }

            gitCommandValider.UpdatePlayerUnlockDict();
        }
    }

    void SetNewItemLocation(GameObject newItem, string listName, GameObject createLocation, bool isInitial)
    {
        int newItemIndex = -1;

        if (!isInitial)
        {
            Debug.Log("is not Initial");
            Transform oldItem = createLocation.transform.Find(listName);
            newItemIndex = oldItem.GetSiblingIndex();
            Destroy(oldItem.gameObject);
        }

        newItem.name = listName;
        newItem.transform.SetParent(createLocation.transform);
        newItem.transform.localScale = new(1, 1, 1);

        newItem.transform.SetSiblingIndex(newItemIndex);
    }

    GameObject UnlockItemContent(string categoryKey, GameObject createLocation, string listName)
    {

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

        return newItem;
    }

    public void SetIndex(Button button)
    {
        button.transform.SetSiblingIndex(0);
    }

    #endregion
}
