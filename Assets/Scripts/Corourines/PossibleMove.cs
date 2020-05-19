using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMoves
{
    public static int DiceNum;

    public static List<GameObject>[] CellsList;

    public static bool[] ValidPawn;

    public static int CurrPlayerTurn = 2;
    // Start is called before the first frame update
    public static void Main()
    {
        Debug.Log("in PossibleMoves.main");
        
        // TODO: Change current player based on (Winners)
        int playerGroup = CurrPlayerTurn;
        
        DiceNum = int.Parse(ClassObjects.Gameobj.diceScore.text);
        
        CellsList = new List<GameObject>[4];
        ValidPawn = new bool[4];

        int numValidMoves = 4;
        for (int pawnNum = 0; pawnNum < 4; pawnNum++)
        {
            GameObject player = ClassObjects.Gameobj.players[playerGroup, pawnNum];

            GameObject currCell = player.GetComponent<PlayerMetaData>().currCell;
            GameObject homeCell = player.GetComponent<PlayerMetaData>().homeCell;

            ValidPawn[pawnNum] = true;

            int numMoves = 0;
            if (currCell == homeCell)
            {
                if (DiceNum == 6)
                    numMoves = 1;
                else
                    numMoves = 0;
            }
            else
                numMoves = DiceNum;

            CellsList[pawnNum] = FindCellList(numMoves, player);

            if (CellsList[pawnNum].Count <= numMoves || numMoves < 1)
            {
                ValidPawn[pawnNum] = false;
                numValidMoves--;
                
                Debug.Log($"Invalidating for {pawnNum}");
            }
        }
        
        Debug.Log($"Num of valid moves {numValidMoves}");
        if (numValidMoves > 0)
            ClassObjects.Gameobj.mb.StartCoroutine(MovePlayer.Routine());
        else
            ClassObjects.Gameobj.mb.StartCoroutine(RollDice.Routine());
    }
    
    // including first cell also,
    // so first index gives cell on which player resides 
    public static List<GameObject> FindCellList(int numMoves, GameObject player)
    {
        PlayerMetaData playerMetaData = player.GetComponent<PlayerMetaData>();
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

    public static void UpdateCurrPlayerTurn()
    {
        CurrPlayerTurn = (CurrPlayerTurn + 1) % 4;
    }
}
