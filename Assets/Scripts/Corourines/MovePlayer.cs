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
    public static int NumPlayers = 4;
    public static Coroutine[] MovablePlayersRoutines;
    
    static BlinkHome[] blinkPlayerP = new BlinkHome[4];
    static GameObject[] playerP = new GameObject[4];
    static Coroutine[] blinkRoutine = new Coroutine[4];
    
    static bool isRunning;
    public static void Start()
    {
        MovablePlayersRoutines = new Coroutine[4];
    }

    public static IEnumerator Routine()
    {

        Debug.Log("in Routine");
        isRunning = true;
        // PlayerP stands for playerPawn
        // for (int i = 0; i < 4; i++)
        // {
        //     blinkPlayerP[i] = new BlinkHome();
        //     playerP[i] = ClassObjects.Gameobj.Players[NextPlayerTurn, i];
        //     blinkRoutine[i] = ClassObjects.Gameobj.MB.StartCoroutine(blinkPlayerP[i].Routine(playerP[i]));
        // }

        Debug.Log("Waiting for player");
        while (Player == null)
            yield return null;
        
        Debug.Log("now showing movement");
        
        PlayerMetaData playerMetaData = Player.GetComponent<global::PlayerMetaData>();

        List<GameObject> cellsList = PossibleMoves.CellsList[playerMetaData.pawnNum]; 

        int cellsCount = cellsList.Count;

        Debug.Log($"cell count is {cellsCount}");
        GameObject lastCell = cellsList[cellsCount- 1];
        
        // Rescale Current cell players
        cellsList[0].GetComponent<CellMetaData>().RemovePlayer(Player);
        
        // cellsList[0].GetComponent<CellMetaData>().AddPlayer(Player);
        // playerMetaData.currCell.GetComponent<CellMetaData>().RemovePlayer(Player);
        // show movement
        int currStep = 1;
        while (currStep < cellsCount)
        {
            
            // Show Movement
            Vector3 desiredPosition = CreateBoard.NewPiecePosition(cellsList[currStep]);
    
            // intermediate postion
            // (Little Up in the air to show jump)
            Vector3 midPosition = (Player.transform.position + desiredPosition) / 2;
            midPosition.y += 1f;

            float t = 0;
            float movementSpeed = 10f;
            while (t < 1) {
                Player.transform.position = Vector3.Lerp(Player.transform.position, midPosition, t);
                t += movementSpeed * Time.deltaTime;
                yield return null;
            }

            t = 0;
            while (t < 1) {
                Player.transform.position = Vector3.Lerp(Player.transform.position, desiredPosition, t);
                t += movementSpeed * Time.deltaTime;
                yield return null;
            }
            currStep++;
        }
        
        // Last Cell Mechanics
        LastCellMechanics.Main(Player, lastCell);

        // Updating Turn;
        // if dice score was not 6
        if (PossibleMoves.DiceNum != 6)
            PossibleMoves.UpdateCurrPlayerTurn();
        
        // Next Turn Preparation
        Player = null;
        // NextPlayerTurn = (NextPlayerTurn + 1) % NumPlayers;
        ClassObjects.Gameobj.mb.StartCoroutine(RollDice.Routine());

        isRunning = false;
    }

    public static void PlayerToMove(GameObject player)
    {
        if (!isRunning)
            return;
        
        if (Player != null)
            return;

        int playerGroup = player.GetComponent<PlayerMetaData>().playerGroup;
        if (playerGroup != PossibleMoves.CurrPlayerTurn)
            return;
        // Debug.Log($"Player Group is {playerGroup}");

        int playerPawn = player.GetComponent<PlayerMetaData>().pawnNum;

        Debug.Log($"player pawn is {playerPawn}");

        if (!PossibleMoves.ValidPawn[playerPawn])
            return;
        
        Player = player;
        
        // Stopping Cell Blinking
        PossibleMoves.StopBlinkPawns();
        
        // for (int i = 0; i < 4; i++)
        // {
        //     ClassObjects.Gameobj.StopCoroutine(blinkRoutine[i]);
        //     blinkPlayerP[i].Reset();
        // }
    }
}
