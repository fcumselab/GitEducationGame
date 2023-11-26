using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class QuestFilter : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, GameObject> ValidObjDict; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string CompareObject(GameObject SenderObj)
    {
        if (ValidObjDict.ContainsKey(SenderObj.name))
        {
            return SenderObj.name;
        }
        else
        {
            return "Not Found";
        }
    }
}
