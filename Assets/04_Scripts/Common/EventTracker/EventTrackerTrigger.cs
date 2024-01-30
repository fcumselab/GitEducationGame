using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EventTrackerTrigger : SerializedMonoBehaviour
{
    public enum TriggerType { Window, Other };
    public TriggerType triggerType = TriggerType.Other;

    public void SendEvent(string eventName, string eventDetail)
    {
        EventTrackerManager.Instance.AddNewEvent(eventName, eventDetail);
    }
}
