using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class WindowManager : MonoBehaviour
{
    [SerializeField] bool isEntering;
    [SerializeField] Canvas canva;
    [SerializeField] RectTransform targetPanel;

    [SerializeField] Texture2D resizeMouseIcon;

    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DragObject(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        targetPanel.anchoredPosition += eventData.delta / canva.scaleFactor;
        //Debug.Log(targetPanel.anchoredPosition);
    }

    public void EnterPanel(BaseEventData data)
    {
        isEntering = true;
        //Debug.Log("enter");
        //Cursor.SetCursor(resizeMouseIcon, hotSpot, cursorMode);
    }

    public void LeavePanel(BaseEventData data)
    {
        isEntering = false;
        //Debug.Log("leave");

        //Cursor.SetCursor(null, hotSpot, cursorMode);
    }
}
