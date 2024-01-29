using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyPlayMakerScriptHelper
{
    public static void RunTest()
    {
        Debug.Log("Run MyPlayMakerScriptHelper successfully!");
    }

    public static PlayMakerFSM GetFsmByName(GameObject TargetObj ,string fsmName)
    {
        PlayMakerFSM[] fsm = TargetObj.GetComponents<PlayMakerFSM>();
        for (int i = 0; i < fsm.Length; i++)
        {
            if (fsm[i].FsmName == fsmName)
            {
                return fsm[i];
            }
        }

        Debug.Log($"Cannot find Target Fsm! \nObjName: {TargetObj.name}\n fsmName: {fsmName}");
        return null;
    }
}
