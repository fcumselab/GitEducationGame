using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class StageItemButton : SerializedMonoBehaviour
{
    [SerializeField] Button itemButton;

    //Called StageSelectionDetailedWindow only is unlock
    public void Initialize(StageSelectionDetailedWindow script, string stageName)
    {
        itemButton.onClick.AddListener(() => script.OpenWindow(stageName));
    }
}