using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CommitLine : MonoBehaviour
{
    [SerializeField] UILineRenderer uiLine;

    public void GenerateCommitLine(Vector2 pos, Color color)
    {
        uiLine.enabled = false;
        uiLine.Points[0] = pos;
        uiLine.color = color;
        uiLine.enabled = true;
    }
}
