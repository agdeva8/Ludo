using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMoves
{
    public static int DiceNum;

    public static List<GameObject>[] CellsList;

    public static bool[] ValidPawn;

    public static int NumValidMoves;

    public static int CurrPlayerTurn = 0;

    public static int GiveAdditionalChance = 0;

    public static int RecentSixesCount = 0;
    
    // Start is called before the first frame update
    public static void Main()
    {
        Debug.Log("in PossibleMoves.main");
        
        // TODO: Change current player based on (Winners)
        int playerGroup = CurrPlayerTurn;
        
        DiceNum = int.Parse(ClassObjects.Gameobj.diceScore.text);
        
        CellsList = new List<GameObject>[4];
        ValidPawn = new bool[4];

        NumValidMoves = 4;

        for (int pawnNum = 0; pawnNum < 4; pawnNum++)
        {
            GameObject player = ClassObjects.Gameobj.players[playerGroup, pawnNum];

            GameObject currCell = player.GetComponent<PlayerMetaData>().currCell;
            GameObject homeCell = player.GetComponent<PlayerMetaData>().homeCell;

            ValidPawn[pawnNum] = true;

            int numMoves = 0;

            // Checking condition of three Sixes in One Go
            if ( !(DiceNum == 6 && RecentSixesCount == 2))
            {
                if (currCell == homeCell)
                {
                    if (DiceNum == 6)
                        numMoves = 1;
                    else
                        numMoves = 0;
                }
                else
                    numMoves = DiceNum;
            }

            CellsList[pawnNum] = FindCellList(numMoves, player);

            if (CellsList[pawnNum].Count <= numMoves || numMoves < 1)
            {
                ValidPawn[pawnNum] = false;
                NumValidMoves--;
                
                Debug.Log($"Invalidating for {pawnNum}");
            }
        }
        
        StartBlinkPawns();
        
        // Moving the player
        ClassObjects.Gameobj.mb.StartCoroutine(MovePlayer.Routine());
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
        // Giving chance when user got 6;
        if (DiceNum == 6)
        {
            GiveAdditionalChance++;
            RecentSixesCount++;
        }
        else
            RecentSixesCount = 0;

        if (GiveAdditionalChance > 0 && RecentSixesCount < 3)
            GiveAdditionalChance--;
        else
        {
            CurrPlayerTurn = (CurrPlayerTurn + 1) % 4;
            RecentSixesCount = 0;
            GiveAdditionalChance = 0;
        }
    }

    public static void StartBlinkPawns()
    {
        // Not blinking if only one pawn to move
        if (NumValidMoves < 2)
            return;
        
        for (int pi = 0; pi < 4; pi++)
        {
            if (ValidPawn[pi])
            {
               ClassObjects.Gameobj.players[CurrPlayerTurn, pi].GetComponent<Blink>().StartRoutine(); 
            }
        }
    }

    public static void StopBlinkPawns()
    {
        for (int pi = 0; pi < 4; pi++)
        {
            if (ValidPawn[pi])
            {
               ClassObjects.Gameobj.players[CurrPlayerTurn, pi].GetComponent<Blink>().Stop(); 
            }
            
        }
    }
}
