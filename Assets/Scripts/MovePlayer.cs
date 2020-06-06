using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class MovePlayer : MonoBehaviourPun, IPunObservable
{
    // MP is just used for debugging purpose
    // Cant be used in development;
    public static MovePlayer MP;
    public GameObject player;
    public PlayerMetaData playerMetaData;
    public LastCellMechanics lastCellMechanics;
    public PossibleMoves possibleMoves;
    
    private int numMoves;

    private int distanceFromHome;

    private bool isRunning;
    public bool amAllowed;
    public PhotonView PV;
    void Start()
    {
        if (MP == null || MP != this)
            MP = this;
    }

    // Update is called once per frame
    public void OnMouseDown()
    {
        // This means I am not allowed to move 
        // when user clicked
        if (!amAllowed)
            return;
        
        if (isRunning)
        {
            // Debug.Log("Move player running");
            return;
        }

        if (!PV.IsMine)
            return;
        
        // Disallowing all other player to run because I am chosen
        int myTeam = TurnManager.TM.myTeam;
        for (int i = 0; i < 4; i++)
            GameObjects.GO.players[myTeam, i].GetComponent<MovePlayer>().amAllowed = false;

        List<GameObject> cellsList = possibleMoves.cellsList; 
        int distance = cellsList.Count - 1;

        Debug.Log("Distance is " + distance);
        // playerMetaData.distanceFromHome = distanceFromHome;
        
        // updating variables to be written 
        numMoves = distance;
        distanceFromHome += distance;
        
        MoveViaList(cellsList);
    }

    void Update()
    {
        if (PV.IsMine)
        {
            ProcessInputs();
        }
        else {
            smoothMovement();
        }
    }

    private void smoothMovement()
    {
        if (isRunning)
            return;

        if (distanceFromHome != playerMetaData.distanceFromHome)
        {
            Debug.Log("moving " + numMoves);
            MoveViaDist(numMoves);
        }
    }

    private void correctPlayerPos()
    {
        if (playerMetaData.distanceFromHome == distanceFromHome)
            return;

        GameObject currCell = playerMetaData.currCell;
        GameObject newCell = currCell;
        while (playerMetaData.distanceFromHome > distanceFromHome)
        {
            newCell = newCell.GetComponent<CellMetaData>().GetPrevGameObj();
            playerMetaData.distanceFromHome--;
        }
        while (playerMetaData.distanceFromHome < distanceFromHome)
        {
            newCell = newCell.GetComponent<CellMetaData>().GetNextGameObj();
            playerMetaData.distanceFromHome++;
        }
       
        currCell.GetComponent<CellMetaData>().RemovePlayer(player);
        newCell.GetComponent<CellMetaData>().AddPlayer(player);
    }

    private void ProcessInputs()
    {
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)	
    {
        // Debug.Log("On Photon Serialization");
        if (stream.IsWriting)
        {
            // Debug.Log($"writing to stream{numMoves}");
            stream.SendNext(numMoves);
            stream.SendNext(distanceFromHome);
        }
        else if (stream.IsReading)
        {
            numMoves = (int) stream.ReceiveNext();
            distanceFromHome = (int) stream.ReceiveNext();
            // Debug.Log("Reading from stream" + numMoves);
        }
    }

    public void MoveViaDist(int distance)
    {
        if (distance < 1)
            return;
        List<GameObject> cellsList = possibleMoves.FindCellList(distance);
        MoveViaList(cellsList);
    }

    public void MoveViaList(List<GameObject> cellsList)
    {
        StartCoroutine(MoveRoutine(cellsList));
    }

    public IEnumerator MoveRoutine(List<GameObject> cellsList)
    {
        Debug.Log("Showing Move Routine");
        isRunning = true;
        
        int cellsCount = cellsList.Count;

        Debug.Log("Cells count is " + cellsCount);

        // Noting to move
        if (cellsCount <= 1)
            yield break;

        GameObject lastCell = cellsList[cellsCount - 1];

        // Removing it from curr Cell 
        cellsList[0].GetComponent<CellMetaData>().RemovePlayer(player);

        // show movement
        int currStep = 1;
        float movementSpeed = 10f;
        while (currStep < cellsCount)
        {
            // Show Movement
            Vector3 desiredPosition = NewPiecePosition(cellsList[currStep]);

            // intermediate postion
            // (Little Up in the air to show jump)
            Vector3 midPosition = (player.transform.position + desiredPosition) / 2;
            midPosition.y += 1f;

            float t = 0;
            while (t < 1)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, midPosition, t);
                t += movementSpeed * Time.deltaTime;
                yield return null;
            }

            t = 0;
            while (t < 1)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, desiredPosition, t);
                t += movementSpeed * Time.deltaTime;
                yield return null;
            }

            currStep++;
        }

        // Stop home cell blinking
        // ClassObjects.Gameobj.homeOut[PossibleMoves.CurrPlayerTurn].GetComponent<Blink>().Stop();
        
        // Updating Turn;
        // PossibleMoves.UpdateCurrPlayerTurn();
        // Now Time for Dice Rolling
        // ClassObjects.Gameobj.mb.StartCoroutine(RollDice.Routine());

        playerMetaData.distanceFromHome += cellsCount - 1;
        correctPlayerPos();
        
        // changing curr cell
        playerMetaData.currCell = lastCell;
        
        // Now Starting last cell mechanics 
        lastCellMechanics.Main();
        
        // Waiting for last cell mechanics to get over
        while (lastCellMechanics.IsRunning())
            yield return 0.01;
        
        // Resetting local parameters
        isRunning = false;
        amAllowed = false;
        
        // Asking turn manager to end my client's turn
        if (PV.IsMine && TurnManager.TM.isMyTurn)
            TurnManager.TM.EndTurn();
    }
    
    public Vector3 NewPiecePosition(GameObject cell)
    {
        GameObject tempGo = new GameObject("temp");
        tempGo.transform.parent = cell.transform;

        // 0.5f to bring to level and 0.01 to avoid overlaps
        tempGo.transform.localPosition = Vector3.up * (0.5f + 0.02f);
        // tempGo.transform.localRotation = Quaternion.Euler(Vector3.zero);
                
        Vector3 retPosition = tempGo.transform.position;
        GameObject.DestroyImmediate(tempGo);

        return retPosition;
    }

     // Mapping from what the number is and what it will be interpetted
    // according to the rules
}
