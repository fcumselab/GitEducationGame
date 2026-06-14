using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileContentWindow : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    }

    // Update is called once per frame
    public void ExpandScreenByFileContent()
    {
        StartCoroutine(WaitTimes());
    }

    IEnumerator WaitTimes()
    {
        gameObject.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    }
}
