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

    [SerializeField] GameObject CategoryListTitle;
    LeanLocalizedText CategoryListTitlei18n;

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
        if (!CategoryListTitlei18n) CategoryListTitlei18n = CategoryListTitle.GetComponent<LeanLocalizedText>();

        switch (categoryType) {
            case CategoryType.Command:
                CategoryListTitlei18n.TranslationName = "GameManualWindow/ListTitle/Command";
                ListPanelTopBar.color = new Color32(255, 230, 198, 255);
                ContentPanelTopBar.color = new Color32(255, 230, 198, 255);
                break;
            case CategoryType.RuleAndWindow:
                CategoryListTitlei18n.TranslationName = "GameManualWindow/ListTitle/RuleAndWindow";
                ListPanelTopBar.color = new Color32(194, 236, 170, 255);
                ContentPanelTopBar.color = new Color32(194, 236, 170, 255);
                break;
            case CategoryType.VersionControl:
                CategoryListTitlei18n.TranslationName = "GameManualWindow/ListTitle/VersionControl";
                ListPanelTopBar.color = new Color32(167, 195, 255, 255);
                ContentPanelTopBar.color = new Color32(167, 195, 255, 255);
                break;
        }
    }
}
