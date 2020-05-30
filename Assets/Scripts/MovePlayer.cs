using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MovePlayer : MonoBehaviourPun, IPunObservable
{
    public PlayerMetaData playerMetaData;

    private int numMoves;

    private int distanceFromHome;

    private bool isRunning;
    public PhotonView PV;
    void Start()
    {
    }

    // Update is called once per frame
    public void OnMouseDown()
    {
        // Debug.Log("is My turn is " + TurnManager.TM.isMyTurn); 
        // if (!TurnManager.TM.isMyTurn)
        // {
        //     // Debug.Log("its not my turn");
        //     return;
        // }

        if (isRunning)
        {
            // Debug.Log("Move player running");
            return;
        }

        if (!PV.IsMine)
            return;
        
        if (!TurnManager.TM.CheckTurn())
            return;

        TurnManager.TM.StartTurn();
        
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
       
        currCell.GetComponent<CellMetaData>().RemovePlayer(gameObject);
        newCell.GetComponent<CellMetaData>().AddPlayer(gameObject);
    }

    private void ProcessInputs()
    {
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)	
    {
        Debug.Log("On Photon Serialization");
        if (stream.IsWriting)
        {
            Debug.Log($"writing to stream{numMoves}");
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

        if (PV.IsMine && TurnManager.TM.isMyTurn)
            TurnManager.TM.EndTurn();
        
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
        GameObject currCell = playerMetaData.currCell;
        // TODO: Checking condition of three Sixes in One Go
        if (currCell == playerMetaData.homeCell)
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
