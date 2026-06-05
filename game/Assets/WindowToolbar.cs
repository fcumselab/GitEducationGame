using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

class SelectLineController
{
    public PlayMakerFSM buttonFsm;
    public PlayMakerFSM lineAnimationFsm;
}

public class WindowToolbar : SerializedMonoBehaviour
{
    [SerializeField] string selectedButtonLineKey;
    [SerializeField] bool enableMouseDetect = false;

    [SerializeField] Dictionary<string, SelectLineController> selectedLineDict = new();

    public void UpdateSelectedButtonLine(GameObject clickedButtonLine)
    {
        if (clickedButtonLine)
        {
            if (!selectedLineDict.ContainsKey(clickedButtonLine.transform.parent.name))
            {
                PlayMakerFSM animation = MyPlayMakerScriptHelper.GetFsmByName(clickedButtonLine, "Animation");
                PlayMakerFSM buttonFsm = MyPlayMakerScriptHelper.GetFsmByName(clickedButtonLine.transform.parent.gameObject, "Button");
                SelectLineController newController = new();
                newController.lineAnimationFsm = animation;
                newController.buttonFsm = buttonFsm;
                selectedLineDict.Add(clickedButtonLine.transform.parent.name, newController);
            }
        }

        if (selectedButtonLineKey != "")
        {
            Debug.Log(selectedLineDict[selectedButtonLineKey].buttonFsm.FsmVariables.GetFsmBool("isOpen").Value);
            if (selectedLineDict[selectedButtonLineKey].buttonFsm.FsmVariables.GetFsmBool("isOpen").Value) {
                selectedLineDict[selectedButtonLineKey].lineAnimationFsm.SendEvent("Common/Controller/Enable Control");
            }
            else
            {
                selectedLineDict[selectedButtonLineKey].lineAnimationFsm.SendEvent("Common/Controller/Disable Control");
            }
        }

        if(clickedButtonLine)
        {
            selectedButtonLineKey = clickedButtonLine.transform.parent.name;
            selectedLineDict[selectedButtonLineKey].lineAnimationFsm.SendEvent("Common/Button/Click Button");
            enableMouseDetect = true;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (enableMouseDetect)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                enableMouseDetect = false;
                UpdateSelectedButtonLine(null);
            }
        }   
    }
}
