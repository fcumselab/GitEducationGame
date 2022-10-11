using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float moveSpeed = 10f;
    public float minDistance = 125f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Transform arrowTransfrom = transform.Find("arrow");
        //arrowTransfrom.localPosition = new Vector3(100f,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        if(Vector3.Distance(transform.position, target.transform.position) > minDistance)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            transform.position = Vector3.Lerp(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            //Debug.Log(false);
        }
        
    }
}
