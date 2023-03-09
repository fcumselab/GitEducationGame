using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCommit : MonoBehaviour
{
    string id;
    string message;
    List<NewFile> modifyFileList;
    [SerializeField] Dictionary<string, List<NewFile>> fileLists = new Dictionary<string, List<NewFile>>();


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
