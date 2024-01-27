using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Lean.Localization;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class HintTrigger : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, PlayMakerFSM> enableHintDict = new();

    // Start is called before the first frame update
    void Start()
    {
        DialogueSystemFeatureManager.Instance.RegisterHintDict(enableHintDict);
    }
}
