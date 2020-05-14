using System.Collections;
using System.Collections.Generic;
// using AppStoreModel;
using UnityEngine;

public static class MovePlayerSingleStep
{
    private static GameObject player;
    private static float movementSpeed = 10f;
    private static GameObject nextCell;

    public static bool isRunning = false;
    // This is the main function which is called
    public  static IEnumerator Routine(GameObject _player)
    {
        if (isRunning)
            yield break;

        isRunning = true;
        player = _player;
        GameObject currCell = player.GetComponent<PlayerMetaData>().currCell;

        if (currCell == null)
            Debug.Log("curr cell is null");
        
        // Finding Next cell accociated with it
        nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj(0);
        
        if (nextCell == null)
            yield break;

        Vector3 desiredPosition = CreateBoard.NewPiecePostion(nextCell);

        // intermediate postion
        // (Little Up in the air to show jump)
        Vector3 midPosition = (player.transform.position + desiredPosition) / 2;
        midPosition.y = midPosition.y + 0.5f;
        
        while (midPosition != player.transform.position) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, midPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        while (desiredPosition != player.transform.position) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, desiredPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        // Updating curr cell in meta data
        player.GetComponent<PlayerMetaData>().currCell = nextCell;

        // Changing bit for coroutine status
        isRunning = false;
    }
}
