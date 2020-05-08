using System; using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // Start is called before the first frame update
    private float movementSpeed;
    private GameObject nextCell; 
    void Start()
    {
        movementSpeed = 10f;
    }
    
    private void OnMouseDown()
    {
        GameObject currCell = GetComponent<PlayerMetaData>().currCell;
        if (currCell == null)
            Debug.Log("curr cell is null");
        
        nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj();
        if (nextCell == null)
            Debug.Log("next cell is null");
        
        StartCoroutine("MovePeice");
    }

    // Coroutine to animate movement from one cell to another
    IEnumerator MovePeice()
    {
        Vector3 desiredPosition = CreateBoard.NewPiecePostion(nextCell);

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

        GetComponent<PlayerMetaData>().currCell = nextCell;
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
