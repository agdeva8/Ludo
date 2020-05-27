using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Threading;
using Photon.Realtime;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{

    public PhotonView photonView;
    public PlayerMetaData playerMetaData;

    private int numMoves;

    private int distanceFromHome;

    public GameObject hc1;
    public GameObject hc2;

    private GameObject homeCell;
    private bool isRunning;

    void Start()
    {
        hc1 = GameObject.Find("StartHomeCell00");
        hc2 = GameObject.Find("StartHomeCell10");
        
        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
                homeCell = hc1;
            else
                homeCell = hc2;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                homeCell = hc2;
            else
                homeCell = hc1;
        }

        Debug.Log("Adding player to home cell");
        homeCell.GetComponent<CellMetaData>().AddPlayer(gameObject);
        playerMetaData.homeCell = homeCell;
        playerMetaData.currCell = homeCell;
    }

    private void OnMouseDown()
    {
        if (isRunning)
            return;
        
        if (photonView.IsMine)
        {
            int diceNum = 6;
            int interprettedDiceNum = InterpretDiceNum(diceNum);

            Debug.Log("interpretted Dice num is " + interprettedDiceNum);
            List<GameObject> cellsList = FindCellList(interprettedDiceNum);
            int distance = cellsList.Count - 1;

            Debug.Log("Distance is " + distance);
            // playerMetaData.distanceFromHome = distanceFromHome;
            
            // updating variables to be written 
            numMoves = distance;
            distanceFromHome += distance;
            
            MoveViaList(cellsList);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
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
            MoveViaDist(numMoves);
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
       
        currCell.GetComponent<CellMetaData>().RemovePlayer(gameObject);
        newCell.GetComponent<CellMetaData>().AddPlayer(gameObject);
    }

    private void ProcessInputs()
    {
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info )	
    {
        Debug.Log("on photon serialization");
        if (stream.IsWriting)
        {
            Debug.Log("writing to stream" + numMoves);
            stream.SendNext(numMoves);
            stream.SendNext(distanceFromHome);
        }
        else if (stream.IsReading)
        {
            numMoves = (int) stream.ReceiveNext();
            distanceFromHome = (int) stream.ReceiveNext();
            Debug.Log("Reading from stream" + numMoves);
        }
    }
    


    public void MoveViaDice(int diceNum)
    {
        // This case will not happen in lifetime
        // Can only happen forcefully for debugging purpose
        if (diceNum < 1)
            return;
        
        int interprettedDiceNum = InterpretDiceNum(diceNum);
        List<GameObject> cellsList = FindCellList(interprettedDiceNum);
        int distance = cellsList.Count;
        
        MoveViaList(cellsList);
    }

    public void MoveViaDist(int distance)
    {
        if (distance < 1)
            return;
        List<GameObject> cellsList = FindCellList(distance);
        MoveViaList(cellsList);
    }

    public void MoveViaList(List<GameObject> cellsList)
    {
        StartCoroutine(MoveRoutine(cellsList));
    }

    public IEnumerator MoveRoutine(List<GameObject> cellsList)
    {
        GameObject player = gameObject;
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

        // // Last Cell Mechanics
        lastCell.GetComponent<CellMetaData>().AddPlayer(player);
        // LastCellMechanics.Main(Player, lastCell);
        
        // Wating for all defeated pawns to go back to their home
        // while (LastCellMechanics.IsRunning())
        //     yield return new WaitForSeconds(0.01f);
        
        // Stop home cell blinking
        // ClassObjects.Gameobj.homeOut[PossibleMoves.CurrPlayerTurn].GetComponent<Blink>().Stop();
        
        // Updating Turn;
        // PossibleMoves.UpdateCurrPlayerTurn();
        // Now Time for Dice Rolling
        // ClassObjects.Gameobj.mb.StartCoroutine(RollDice.Routine());

        playerMetaData.distanceFromHome += cellsCount - 1;
        correctPlayerPos();
        isRunning = false;
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
    public int InterpretDiceNum(int diceNum)
    {
        int numMoves;
        GameObject currCell = playerMetaData.GetComponent<PlayerMetaData>().currCell;
        // TODO: Checking condition of three Sixes in One Go
        if (currCell == homeCell)
        {
            if (diceNum == 6)
                numMoves = 1;
            else
                numMoves = 0;
        }
        else
            numMoves = diceNum;

        return numMoves;
    }
    
    // It will give the whole path and also what the actual distance 
    // It has travelled given all the restrictions
    public List<GameObject> FindCellList(int numMoves)
    {
        int playerGroup = playerMetaData.playerGroup;
        GameObject currCell = playerMetaData.currCell;
        GameObject nextCell;

        List<GameObject> cellsList = new List<GameObject>();
        int i = 0;
        while (i <= numMoves && currCell != null)
        {
            cellsList.Add(currCell);
            nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj(playerGroup);
            currCell = nextCell;
            i++;
        }
        return cellsList;
    }
}
