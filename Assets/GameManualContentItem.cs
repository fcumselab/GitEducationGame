using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;

public class GameManualContentItem : SerializedMonoBehaviour
{
    enum ButtonType {Command, RuleAndWindow, VersionControl}
    [SerializeField] ButtonType buttonType;

    #region Only Command buttonType need Reference
    [FoldoutGroup("Command Selection")]
    [SerializeField] int currentPageNum = 0;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Text CurrentPageNumText;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Text MaxPageNumText;
    [FoldoutGroup("Command Selection")]
    [SerializeField] GameObject LockIcon;
    [FoldoutGroup("Command Selection")]
    [SerializeField] GameObject Star;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Button PageUpButton;
    [FoldoutGroup("Command Selection")]
    [SerializeField] Button PageDownButton;
    #endregion 

    private void OnEnable()
    {
        if (buttonType == ButtonType.Command)
        {

        }
    }

    public void InitializeContent()
    {
        if(buttonType == ButtonType.Command)
        {

        }
    }

    
}
