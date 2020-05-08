using System; using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private float jumpVel;
    private Vector3 p; 
    private float movementSpeed;
    private GameObject nextCell; 
    void Start()
    {
        // movementSpeed = 0.5f;
        // rb = transform.GetComponent<Rigidbody>();
        // jumpVel = 5f;
        //
        // nextCell = GameObject.Find("C2");
        // p = transform.position;
    }
    
    private void OnMouseDown()
    {
        nextCell = GameObject.Find("C2");
        movementSpeed = 10f;
        StartCoroutine("MovePeice");
    }

    // Coroutine to animate movement from one cell to another
    IEnumerator MovePeice()
    {
        Vector3 desiredPosition = CreateBoard.NewPeicePostion(nextCell);

        // intermediate postion (Little Up in the air to show jump)
        Vector3 midPosition = (transform.position + desiredPosition) / 2;
        midPosition.y = midPosition.y + 0.5f;
        
        while (midPosition != transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, midPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        while (desiredPosition != transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        // if (p != transform.position)
        //     transform.position = Vector3.MoveTowards(transform.position, p, movementSpeed * Time.deltaTime);
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     rb.velocity = Vector3.up * jumpVel;
        // } 

    }
}
