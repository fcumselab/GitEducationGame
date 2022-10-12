using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowingCommits : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Start is called before the first frame update
    //List<Transform> commits = new List<Transform>();
    //public Transform followingTarget; 

    private bool isDragging = false;

    public float moveSpeed = 10f;
    public float minDistance = 175f;
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    
    int nowCommitNum;
    Transform nextCommit;
    Transform preCommit;
    void Start()
    {
        /*
        commits.Add(transform.Find("arrow"));

        for (int i=2; i< transform.childCount; i++)
        {
            if (transform.GetChild(i).Find("arrow") != null)
            {
                commits.Add(transform.GetChild(i));
            }
        }
       
        foreach(Transform child in commits)
        {
            Debug.Log(child.name);
        }*/
        
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        nowCommitNum = transform.GetSiblingIndex();
        if (transform.parent.childCount != (nowCommitNum + 1))
        {
            nextCommit = transform.parent.GetChild(nowCommitNum + 1);
        }
        preCommit = (nowCommitNum != 0) ? transform.parent.GetChild(nowCommitNum - 1) : null;
    }
    // Update is called once per frame
    void Update()
    {

        if (!isDragging)
        {
            followObj();
        }
        //Vector3 position = transform.position - followingTarget.transform.position;
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

        //transform.Rotate(new Vector3(0,0,, XaxisRoatation);
        //transform.Rotate(Vector3.right, YaxisRoatation);
        //Debug.Log(XaxisRoatation + " " + YaxisRoatation);
        //Debug.Log(eventData.delta);
        //Debug.Log(nextCommit.name + " " + nowCommitNum);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("end");
        isDragging = false;

    }

    void followObj()
    {        
        

        //Vector3 direction = nextCommit.transform.position - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rb.rotation = angle;
        //Debug.Log(Vector3.Distance(transform.position, nextCommit.transform.position));
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
