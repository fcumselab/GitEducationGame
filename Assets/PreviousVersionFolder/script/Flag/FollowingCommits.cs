using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowingCommits : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private bool isDragging = false;

    public float moveSpeed = 10f;
    public float minDistance = 175f;
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    
    private Transform arrow;
    int nowCommitNum;
    Transform nextCommit;
    Transform preCommit;
    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        nowCommitNum = transform.GetSiblingIndex();
        arrow = transform.Find("arrow");
        if (transform.parent.childCount != (nowCommitNum + 1))
        {
            nextCommit = transform.parent.GetChild(nowCommitNum + 1);
        }
        preCommit = (nowCommitNum != 0) ? transform.parent.GetChild(nowCommitNum - 1) : null;
    }

    void Update()
    {
        arrowFollow();

        if (!isDragging){ followCommit(); }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("click down");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("begin drag");
        isDragging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("end");
        isDragging = false;
    }
    void arrowFollow(){

        if (nextCommit != null)
        {
            Vector3 direction = (arrow.position - nextCommit.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.GetComponent<Rigidbody2D>().rotation = angle;
            arrow.position = Vector3.Lerp(transform.position, nextCommit.transform.position, 0.5f);
            
        }
    }
    void followCommit()
    {        
        if(nextCommit != null && nowCommitNum != 0)
        {
            if (Vector3.Distance(transform.position, nextCommit.transform.position) > minDistance)
            {

                transform.position = Vector3.Lerp(transform.position, nextCommit.transform.position, moveSpeed * Time.deltaTime);
                
                
            }
        }
        
        if (preCommit != null)
        {
            if (Vector3.Distance(transform.position, preCommit.transform.position) > minDistance)
            {

                transform.position = Vector3.Lerp(transform.position, preCommit.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }
}
