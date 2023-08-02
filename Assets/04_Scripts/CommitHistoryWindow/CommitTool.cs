using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommitTool : MonoBehaviour
{
    [SerializeField] List<string> generateCommitIdList = new();

    public string SetRandomId()
    {
        
        string key = "0123456789abcdefghijkmnpqrstuvwxyz";
        while (true) {
            string result = "";

            for (int i = 0; i < 6; i++)
            {
                int ran = Random.Range(0, key.Length);
                result += key[ran];
            }

            if (!generateCommitIdList.Contains(result))
            {
                generateCommitIdList.Add(result);
                return result;
            }
        }
    }
}
