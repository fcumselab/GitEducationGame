using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using PixelCrushers.DialogueSystem;

public class StageManagerController : SerializedMonoBehaviour
{
    string selectedStageName = "";
    [SerializeField] PlayMakerFSM LoadingStageManagerFsm;
    [SerializeField] Dictionary<string, GameObject> StageManagerDict = new();
    [SerializeField] GameObject selectedStageManager;

    public void Initialize(string selectedStageName)
    {
        if (StageManagerDict.ContainsKey(selectedStageName))
        {
            selectedStageManager = Instantiate(StageManagerDict[selectedStageName]);
            selectedStageManager.transform.SetParent(transform);
            selectedStageManager.name = "Stage Manager";
            selectedStageManager.SetActive(true);
            selectedStageManager.GetComponent<StageManager>().Initialize(selectedStageName);

            LoadingStageManagerFsm.FsmVariables.GetFsmGameObject("selectedStageManager").Value = selectedStageManager;
            LoadingStageManagerFsm.FsmVariables.GetFsmString("selectStageName").Value = selectedStageName;
            LoadingStageManagerFsm.enabled = true;
        }
        else
        {
            Debug.LogError("Not found Target StageManager! Please add one.");
        }
    }
}
