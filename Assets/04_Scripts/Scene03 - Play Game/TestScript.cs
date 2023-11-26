using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using HutongGames.PlayMaker;

public class TestScript : MonoBehaviour
{
    [SerializeField] UILineRenderer uiLine;
    [SerializeField] PlayMakerFSM test;
    // Start is called before the first frame update
    void Start()
    {
        uiLine = GetComponent<UILineRenderer>();
        Debug.Log(test.FsmVariables.GetFsmVector2("point2Pos").Value);
        foreach(var a in uiLine.Points)
        {
            Debug.Log(a);
        }
    }

    public void GenerateCommitLine()
    {

    }
}
