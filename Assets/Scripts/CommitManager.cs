using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitManager : MonoBehaviour
{
    [SerializeField] GameObject commit;
    [SerializeField] GameObject commitPanel;


    [SerializeField] Vector3 intervel;
    public GameObject nowCommit;
    public string nowBranch;

    //Singleton instantation
    private static CommitManager instance;
    public static CommitManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<CommitManager>();
            return instance;
        }
    }

    public void AddNewCommit(string message)
    {
        if(nowCommit != null)
        {
            commit.GetComponent<NewCommit>().SetValue(message, StageFileManager.Instance.stagedFileLists, nowCommit.name, nowBranch);
        }else
        {
            commit.GetComponent<NewCommit>().SetValue(message, StageFileManager.Instance.stagedFileLists);
        }
        StageFileManager.Instance.stagedFileLists.Clear();
        StageFileManager.Instance.UpdateUI();

        GameObject obj;
        if(nowCommit != null) obj = Instantiate(commit, transform.position + intervel, Quaternion.identity);
        else obj = Instantiate(commit, transform.position, Quaternion.identity);
        
        obj.transform.SetParent(commitPanel.transform);
        obj.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        obj.name = obj.GetComponent<NewCommit>().GetId();
        nowCommit = obj;
    }

    public void FocusOnCommit() {
        if (nowCommit != null)
        {
            float diff = nowCommit.transform.position.y - transform.position.y;
            commitPanel.transform.position = new Vector2(commitPanel.transform.position.x, commitPanel.transform.position.y - diff);
        }
    }
}
