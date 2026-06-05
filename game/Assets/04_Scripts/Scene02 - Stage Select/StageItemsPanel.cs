using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class StageItemsPanel : SerializedMonoBehaviour
{
    [SerializeField] List<GameObject> UnlockButtonList = new();

    [SerializeField] Tool tool;
    [SerializeField] GameObject StageSelectionScrollView;
    [SerializeField] GameObject StageSelectionContentPanel;

    private void OnEnable()
    {
        if(UnlockButtonList.Count > 0)
        {
            StartCoroutine(ScrollToPositionAfterDelay(0.05f));
        }
    }

    IEnumerator ScrollToPositionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        tool.ScrollRectLockTargetContent(UnlockButtonList[UnlockButtonList.Count - 1], StageSelectionScrollView, StageSelectionContentPanel, "x");
    }

    public void ScrollRectLockTargetContent(GameObject ClickButton)
    {
        tool.ScrollRectLockTargetContent(ClickButton, StageSelectionScrollView, StageSelectionContentPanel, "x");
    }

    public void AddNewStageButtonList(GameObject UnlockButton)
    {
        UnlockButtonList.Add(UnlockButton);
    }
}
