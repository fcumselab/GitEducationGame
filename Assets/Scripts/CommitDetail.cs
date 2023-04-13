using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class CommitDetail : SerializedMonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] Text commitMessage;
    [SerializeField] GameObject changedFileParent;

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
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        
        CommitDatas data = obj.GetComponent<NewCommit>().GetCommitDatas();
        timeText.text = data.GetCommitTime();
        commitMessage.text = data.GetMessage();

        List<FileDatas> filedatas = data.GetModifyFileList();
        int loc = 0;
        foreach(Transform child in changedFileParent.transform)
        {
            if (filedatas.Count <= loc)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                Text[] textList = child.GetComponentsInChildren<Text>();

                // 0 = Location text, 1 = Name text
                textList[0].text = filedatas[loc].GetLocation();
                textList[1].text = filedatas[loc].GetName();

                loc++;
            }
        }
    }
}
