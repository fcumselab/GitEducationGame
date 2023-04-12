using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitManager : MonoBehaviour
{
    [SerializeField] GameObject commit;
    [SerializeField] GameObject commitPanel;
    [SerializeField] GameObject spawnLocation;


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
        CommitDatas newCommit = ScriptableObject.CreateInstance<CommitDatas>();
        if (nowCommit != null)
        {
            newCommit.InitValue(message, StageFileManager.Instance.stagedFileLists, nowCommit.name, nowBranch);
        }else
        {
            newCommit.InitValue(message, StageFileManager.Instance.stagedFileLists);
        }
        StageFileManager.Instance.ClearStageList();

        FocusOnCommit();
        GameObject obj;
        if (nowCommit != null)
        {
            obj = Instantiate(commit, spawnLocation.transform.position + intervel, Quaternion.identity);
            nowCommit.GetComponent<NewCommit>().UpdateCommitUI(false);
        }
        else
        {
            obj = Instantiate(commit, spawnLocation.transform.position, Quaternion.identity);
        }
        obj.SetActive(true);
        obj.GetComponent<NewCommit>().SetCommitDatas(newCommit);
        obj.GetComponent<NewCommit>().UpdateCommitUI(true);
        obj.transform.SetParent(commitPanel.transform);
        obj.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        obj.name = obj.GetComponent<NewCommit>().GetCommitDatas().GetId();
        nowCommit = obj;
        FocusOnCommit();

    }

    

    public void FocusOnCommit() {
        if (nowCommit != null)
        {
            float diff = nowCommit.transform.position.y - spawnLocation.transform.position.y;
            commitPanel.transform.position = new Vector2(commitPanel.transform.position.x, commitPanel.transform.position.y - diff);
        }
    }
}
