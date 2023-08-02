using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CommitLine : MonoBehaviour
{
    [SerializeField] UILineRenderer uiLine;
    [SerializeField] PlayMakerFSM line;
    [SerializeField] UnityEngine.UI.Extensions.Gradient gradient;

    public void GenerateCommitLine(Vector2 pos, Color startColor, Color endColor)
    {
        uiLine.Points[0] = pos;
        gradient.Vertex1 = startColor;
        gradient.Vertex2 = endColor;
    }
}
