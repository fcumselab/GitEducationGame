using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageFileManager : MonoBehaviour
{
    public List<FileDatas> stagedFileLists = new List<FileDatas>();
    public List<FileDatas> unstagedFileLists = new List<FileDatas>();

    //Singleton instantation
    private static StageFileManager instance;
    public static StageFileManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<StageFileManager>();
            return instance;
        }
    }

    public void ClearStageList()
    {
        foreach (var f in stagedFileLists)
        {
            f.SetStatus("uploaded");
        }
        stagedFileLists.Clear();
    }

    public void MoveToStageList(FileDatas newfile, string fileName, string location)
    {
        stagedFileLists.Add(newfile);
        newfile.SetStatus("staged");
        for (int i = 0; i < unstagedFileLists.Count; i++)
        {
            if ((unstagedFileLists[i].GetName() == fileName || unstagedFileLists[i].GetName().Split(".")[0] == fileName) && unstagedFileLists[i].GetLocation() == location)
            {
                unstagedFileLists.RemoveAt(i);
                break;
            }
        }
    }

    public void MoveToUnstageList(FileDatas newfile, string fileName, string location)
    {
        unstagedFileLists.Add(newfile);
        newfile.SetStatus("unstaged");
        for (int i = 0; i < stagedFileLists.Count; i++)
        {
            if ((stagedFileLists[i].GetName() == fileName || stagedFileLists[i].GetName().Split(".")[0] == fileName) && stagedFileLists[i].GetLocation() == location)
            {
                stagedFileLists.RemoveAt(i);
                break;
            }
        }
    }

    
}
