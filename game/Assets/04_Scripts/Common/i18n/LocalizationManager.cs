using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LocalizationManager : SerializedMonoBehaviour
{
    [SerializeField] Dictionary<string, GameObject> StageCSVDictEnglish = new();
    [SerializeField] Dictionary<string, GameObject> StageCSVDictChinese = new();
    Transform CSVLocation;
    public void CloneStageCSV()
    {
        CSVLocation = transform.Find("CSV");

        string selectedStageName = SaveManager.Instance.GetSelectedStageName();
        if (StageCSVDictEnglish.ContainsKey(selectedStageName) || StageCSVDictChinese.ContainsKey(selectedStageName))
        {
            GameObject cloneCSV = Instantiate(StageCSVDictEnglish[selectedStageName]);
            cloneCSV.transform.SetParent(CSVLocation);
            cloneCSV = Instantiate(StageCSVDictChinese[selectedStageName]);
            cloneCSV.transform.SetParent(CSVLocation);
        }
        else
        {
            Debug.LogError("Not found target Stage CSV file! Please add one");
        }
    }
}
