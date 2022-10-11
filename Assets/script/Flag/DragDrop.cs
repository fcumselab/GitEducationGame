using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    Transform arrowTransfrom;

    private RectTransform rectTransform;
    float rotationSpeed = 1f;
    void Start()
    {
        
    }

    private void Awake()
    {
        arrowTransfrom = transform.Find("arrow");
        arrowTransfrom.localPosition = new Vector3(100f, 0, 0);
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("click down");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("begin drag");
    }
    public void OnDrag(PointerEventData eventData)
    {

        //transform.Rotate(new Vector3(0,0,, XaxisRoatation);
        //transform.Rotate(Vector3.right, YaxisRoatation);
        //Debug.Log(XaxisRoatation + " " + YaxisRoatation);
        //Debug.Log(eventData.delta);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("end");
    }

}
