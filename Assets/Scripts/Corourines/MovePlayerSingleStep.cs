using System.Collections;
using System.Collections.Generic;
// using AppStoreModel;
using UnityEngine;

public static class MovePlayerSingleStep
{
    private static GameObject player;
    private static float movementSpeed = 10f;
    private static GameObject nextCell;

    public static bool IsRunning = false;
    // This is the main function which is called
    public  static IEnumerator Routine(GameObject player, GameObject nextCell)
    {
        Debug.Log("routine started");
        if (IsRunning)
        {
            // Debug.Log("is running breaking the yield");
            yield break;
        }

        // Debug.Log("new routine started");

        IsRunning = true;
        MovePlayerSingleStep.player = player;
        // GameObject currCell = player.GetComponent<PlayerMetaData>().currCell;

        // if (currCell == null)
        //     Debug.Log("curr cell is null");
        
        // Finding Next cell accociated with it
        // nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj(0);
        
        if (nextCell == null)
            yield break;

        Vector3 desiredPosition = CreateBoard.NewPiecePosition(nextCell);

        // intermediate postion
        // (Little Up in the air to show jump)
        Vector3 midPosition = (MovePlayerSingleStep.player.transform.position + desiredPosition) / 2;
        midPosition.y = midPosition.y + 0.5f;
        
        while (midPosition != MovePlayerSingleStep.player.transform.position) {
            MovePlayerSingleStep.player.transform.position = Vector3.MoveTowards(MovePlayerSingleStep.player.transform.position, midPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        while (desiredPosition != MovePlayerSingleStep.player.transform.position) {
            MovePlayerSingleStep.player.transform.position = Vector3.MoveTowards(MovePlayerSingleStep.player.transform.position, desiredPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        // Updating curr cell in meta data
        // player.GetComponent<PlayerMetaData>().currCell = nextCell;

        // Changing bit for coroutine status
        IsRunning = false;
    }
}
