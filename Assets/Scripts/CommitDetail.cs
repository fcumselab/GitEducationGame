using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class CommitDetail : SerializedMonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] Text commitMessage;

    //Singleton instantation
    private static CommitDetail instance;
    public static CommitDetail Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CommitDetail>();
            return instance;
        }
    }


    public void UpdateUI(GameObject obj)
    {
        CommitDatas data = obj.GetComponent<NewCommit>().GetCommitDatas();
        timeText.text = DateTime.Now.ToString();
        commitMessage.text = data.GetMessage();


    }
}
