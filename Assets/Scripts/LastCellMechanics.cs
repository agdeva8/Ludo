using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCellMechanics : MonoBehaviour
{
    [SerializeField] private PlayerMetaData playerMetaData;
    [SerializeField] private GameObject player;
    private GameObject cell;
    private CellMetaData cellMetaData;
    private int runningProcesses;
    private List<GameObject> currPlayers;

    public void Main()
    {
        runningProcesses = 1;
        cell = playerMetaData.currCell;

        cellMetaData = cell.GetComponent<CellMetaData>();
        currPlayers = cellMetaData.players;

        // Handling cases where only need to be Added 
        if (cellMetaData.isStop || currPlayers.Count == 0)
        {
            cellMetaData.AddPlayer(player);
            runningProcesses--;
            return;
        }

        // Getting count of each player group
        // so as to decide whether to kick the player or not
        int[] playerGroupCount = new int[4];
        foreach (GameObject currPlayer in currPlayers)
            playerGroupCount[currPlayer.GetComponent<PlayerMetaData>().playerGroup]++;

        List<GameObject> defeatedPlayers = new List<GameObject>();
        foreach (GameObject currPlayer in currPlayers)
        {
            int currPlayerGroup = currPlayer.GetComponent<PlayerMetaData>().playerGroup;

            if (playerMetaData.playerGroup == currPlayerGroup)
                continue;
            if (playerGroupCount[currPlayerGroup] % 2 == 0)
                continue;
            
            playerGroupCount[currPlayerGroup]--;

            defeatedPlayers.Add(currPlayer);
        }
        
        foreach (GameObject defeatedPlayer in defeatedPlayers)
        {
            // TODO: When Additional chance is there
            // // Giving chance to player is he/she kicked out other player token
            // PossibleMoves.GiveAdditionalChance++;
            
            ClassObjects.Gameobj.mb.StartCoroutine(PushBack2Home(defeatedPlayer));
            runningProcesses++;
        }
        
        cellMetaData.AddPlayer(player);
        
        // Now This routine is last of player turn and it 
        // tells the TurnManager that I am done;
        // hence it will not be counted in running process;
        
        
        runningProcesses--;
    }
    
    IEnumerator PushBack2Home(GameObject defeatedPawn)
    {
        cellMetaData.RemovePlayer(defeatedPawn);
        PlayerMetaData defeatedPMetaData = defeatedPawn.GetComponent<PlayerMetaData>();
        GameObject homeCell = defeatedPMetaData.homeCell;

        GameObject currCell = defeatedPMetaData.currCell;
        while (currCell != homeCell)
        {
            Vector3 desiredPosition = CreateBoard.NewPiecePosition(currCell);
                defeatedPawn.transform.position = Vector3.Lerp( defeatedPawn.transform.position,
                
                                                                desiredPosition, 1);

            currCell = currCell.GetComponent<CellMetaData>().GetPrevGameObj(defeatedPMetaData.playerGroup);

            if (currCell == null)
                currCell = homeCell;
            
            // wasnt able to show smooth behvaviour using progress (t) & movementspeed in lerp
            // so used this. 
            yield return new WaitForSeconds(0.05f);
        }
        
        // Now formally adding defeated pawn to home cell
        
        homeCell.GetComponent<CellMetaData>().AddPlayer(defeatedPawn);
        runningProcesses--;
    }

    // Running Processes indicates whther some function / coroutine is still running or not
    // It only gives whether any coroutine/ function is running or not
    // And not gives which process is running
    public bool IsRunning()
    {
        if (runningProcesses > 0)
            return true;
        return false;
    }
}

