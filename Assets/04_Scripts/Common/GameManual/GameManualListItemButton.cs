using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;
using UnityEngine.SceneManagement;

public class GameManualListItemButton : SerializedMonoBehaviour
{
    string currentSceneName = "";

    [SerializeField] Button button;
    [SerializeField] Text ID;
    [SerializeField] LeanLocalizedText text;
    [SerializeField] Image border;
    [SerializeField] GameObject Star;

    public Button InitializeButton(GameManual gameManual, string itemKey, string categoryType)
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        if(currentSceneName == "Play Game")
        {
            Star.SetActive(StageManager.Instance.CheckGameManualListItemUseInStage(itemKey, categoryType));
        }
        else
        {
            Star.SetActive(false);
        }

        text.TranslationName = $"GameManualItem/list/{itemKey}";
        button = GetComponent<Button>();
        button.onClick.AddListener(() => gameManual.SwitchGameManualItem(button));
        SetButtonColor(categoryType);

        return button;
    }

    private void OnEnable()
    {
        SetID();
    }

    void SetID()
    {
        int index = transform.GetSiblingIndex() + 1;

        if (index >= 100)
        {
            ID.text = $"{index} - ";
        }else if(index >= 10)
        {
            ID.text = $"0{index} - ";
        }
        else
        {
            ID.text = $"00{index} - ";
        }
    }

    void SetButtonColor(string categoryType)
    {
        ColorBlock buttonColor = button.colors;
        switch (categoryType)
        {
            case "Command":
                border.color = new(255, 148, 109);
                buttonColor.normalColor = new(255,230,198);
                buttonColor.selectedColor = new(255, 230, 198);
                buttonColor.highlightedColor = new(255,193,119);
                buttonColor.pressedColor = new(255, 183, 96);
                buttonColor.disabledColor = new(255, 183, 96);
                break;
            case "RuleAndWindow":
                border.color = new(108, 159, 79);
                buttonColor.normalColor = new(194, 236, 170);
                buttonColor.selectedColor = new(194, 236, 170);
                buttonColor.highlightedColor = new(178, 227, 151);
                buttonColor.pressedColor = new(164, 224, 130);
                buttonColor.disabledColor = new(164, 224, 130);
                break;
            case "VersionControl":
                border.color = new(61, 84, 108);
                buttonColor.normalColor = new(167, 195, 255);
                buttonColor.selectedColor = new(167, 195, 255);
                buttonColor.highlightedColor = new(153, 186, 255);
                buttonColor.pressedColor = new(132, 170, 255);
                buttonColor.disabledColor = new(132, 170, 255);
                break;
        }
    }
    
}
