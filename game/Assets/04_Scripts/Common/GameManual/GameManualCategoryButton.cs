using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;

public class GameManualCategoryButton : SerializedMonoBehaviour
{
    [SerializeField] GameManual gameManual;
    [SerializeField] GameObject CategoryList;

    [SerializeField] Image ListPanelTopBar;
    [SerializeField] Image ContentPanelTopBar;

    enum CategoryType { Command, RuleAndWindow, VersionControl};
    [SerializeField] CategoryType categoryType;
    // Start is called before the first frame update

    public void ActivateCategoryList(bool enable)
    {
        CategoryList.SetActive(enable);
    }

    public void ApplyColor()
    {
        switch (categoryType) {
            case CategoryType.Command:
                ListPanelTopBar.color = new Color32(255, 186, 148, 255);
                ContentPanelTopBar.color = new Color32(255, 186, 148, 255);
                break;
            case CategoryType.RuleAndWindow:
                ListPanelTopBar.color = new Color32(194, 236, 170, 255);
                ContentPanelTopBar.color = new Color32(194, 236, 170, 255);
                break;
            case CategoryType.VersionControl:
                ListPanelTopBar.color = new Color32(167, 195, 255, 255);
                ContentPanelTopBar.color = new Color32(167, 195, 255, 255);
                break;
        }
    }

    public string GetCategoryType()
    {
        switch (categoryType)
        {
            case CategoryType.Command:
                return "Command";
            case CategoryType.RuleAndWindow:
                return "RuleAndWindow";
            case CategoryType.VersionControl:
                return "VersionControl";
            default:
                break;
        }
        return "";
    }
}
