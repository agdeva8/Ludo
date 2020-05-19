using System; using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    // Start is called before the first frame update
    private float _movementSpeed;
    private GameObject _nextCell; 
    void Start()
    {
        _movementSpeed = 10f;
    }
    
    private void OnMouseDown()
    {
        GameObject currCell = GetComponent<PlayerMetaData>().currCell;
        if (currCell == null)
            Debug.Log("curr cell is null");
        
        _nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj();
        if (_nextCell == null)
            Debug.Log("next cell is null");
        
        StartCoroutine("MovePeice");
    }

    // Coroutine to animate movement from one cell to another
    IEnumerator MovePeice()
    {
        Vector3 desiredPosition = CreateBoard.NewPiecePosition(_nextCell);

        // intermediate postion (Little Up in the air to show jump)
        Vector3 midPosition = (transform.position + desiredPosition) / 2;
        midPosition.y = midPosition.y + 0.5f;
        
        while (midPosition != transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, midPosition, _movementSpeed * Time.deltaTime);
            yield return null;
        }

        while (desiredPosition != transform.position) {
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, _movementSpeed * Time.deltaTime);
            yield return null;
        }

        GetComponent<PlayerMetaData>().currCell = _nextCell;
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
