using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;

public class PossibleMoves : MonoBehaviour
{
    public List<GameObject> cellsList;
    public PlayerMetaData playerMetaData;
    public MovePlayer movePlayer;
    public int diceNum;
    public int interpretedDiceNum;
    public int distance;
    
    private int myTeam;

    public void Main()
    {
        Debug.Log("in PossibleMoves.main");

        diceNum = Convert.ToInt32(GameObjects.GO.diceScore.text);
        interpretedDiceNum = InterpretDiceNum(diceNum);
        
        cellsList = new List<GameObject>();
        myTeam = TurnManager.TM.myTeam;

        cellsList = FindCellList(interpretedDiceNum);
        distance = cellsList.Count;
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
    public List<GameObject> FindCellList(int numMoves)
    {
        GameObject currCell = playerMetaData.currCell;
        GameObject nextCell;

        List<GameObject> _cellsList = new List<GameObject>();
        int i = 0;
        while (i <= numMoves && currCell != null)
        {
            _cellsList.Add(currCell);
            nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj(playerMetaData.myTeam);
            currCell = nextCell;
            i++;
        }
        return _cellsList;
    }

    public bool IsValid()
    {
        return !(cellsList.Count <= interpretedDiceNum || interpretedDiceNum == 0);
    }
}

