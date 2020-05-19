using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class MovePlayer
{
    public static GameObject Player;
    public static int NextPlayerTurn = 3;
    public static int NumPlayers = 4;
    public static Coroutine[] MovablePlayersRoutines;
    
    static BlinkHome[] blinkPlayerP = new BlinkHome[4];
    static GameObject[] playerP = new GameObject[4];
    static Coroutine[] blinkRoutine = new Coroutine[4];
    public static void Start()
    {
        MovablePlayersRoutines = new Coroutine[4];
    }

    public static IEnumerator Routine()
    {
        
        // PlayerP stands for playerPawn
        // for (int i = 0; i < 4; i++)
        // {
        //     blinkPlayerP[i] = new BlinkHome();
        //     playerP[i] = ClassObjects.Gameobj.Players[NextPlayerTurn, i];
        //     blinkRoutine[i] = ClassObjects.Gameobj.MB.StartCoroutine(blinkPlayerP[i].Routine(playerP[i]));
        // }
        
        while (Player == null)
            yield return null;
        
        int numMoves = int.Parse(ClassObjects.Gameobj.DiceScore.text);
        List<GameObject> cellsList = FindCellList(numMoves, Player);

        int cellsCount = cellsList.Count;
        
        Debug.Log($"cell count is {cellsCount}");
        GameObject lastCell = cellsList[cellsCount- 1];
        
        PlayerMetaData playerMetaData = Player.GetComponent<global::PlayerMetaData>();
        
        // cellsList[0].GetComponent<CellMetaData>().AddPlayer(Player);
        // playerMetaData.currCell.GetComponent<CellMetaData>().RemovePlayer(Player);
        Player.transform.localScale = new Vector3(84, 84, 84);
        // show movement
        int currStep = 1;
        while (currStep < cellsCount)
        {
            // Rescale Current cell players
            cellsList[0].GetComponent<CellMetaData>().RemovePlayer(Player);
            
            // Show Movement
            Vector3 desiredPosition = CreateBoard.NewPiecePostion(cellsList[currStep]);
    
            // intermediate postion
            // (Little Up in the air to show jump)
            Vector3 midPosition = (Player.transform.position + desiredPosition) / 2;
            midPosition.y += 1f;

            float t = 0;
            while (t < 1) {
                Player.transform.position = Vector3.Lerp(Player.transform.position, midPosition, t);
                t += 10f * Time.deltaTime;
                yield return null;
            }

            t = 0;
            while (t < 1) {
                Player.transform.position = Vector3.Lerp(Player.transform.position, desiredPosition, t);
                t += 10f * Time.deltaTime;
                yield return null;
            }
            currStep++;
        }
        
        // Last Cell Mechanics
        playerMetaData.currCell = lastCell;
        lastCell.GetComponent<CellMetaData>().AddPlayer(Player);

        // Next Turn Preparation
        Player = null;
        // NextPlayerTurn = (NextPlayerTurn + 1) % NumPlayers;
        ClassObjects.Gameobj.MB.StartCoroutine(RollDice.Routine());
    }

    public static void PlayerToMove(GameObject player)
    {
        if (Player != null)
            return;

        int playerGroup = player.GetComponent<PlayerMetaData>().PlayerGroup;
        Debug.Log("Player Group is " + playerGroup);
        
        if (playerGroup == NextPlayerTurn)
            Player = player;
        
        // for (int i = 0; i < 4; i++)
        // {
        //     ClassObjects.Gameobj.StopCoroutine(blinkRoutine[i]);
        //     blinkPlayerP[i].Reset();
        // }
    }

    public static List<GameObject> FindCellList(int numMoves, GameObject player)
    {
        PlayerMetaData playerMetaData = Player.GetComponent<PlayerMetaData>();
        int playerGroup = playerMetaData.PlayerGroup;
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
