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
    public static int NextPlayerTurn = 0;
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
        for (int i = 0; i < 4; i++)
        {
            blinkPlayerP[i] = new BlinkHome();
            playerP[i] = ClassObjects.Gameobj.Players[NextPlayerTurn, i];
            blinkRoutine[i] = ClassObjects.Gameobj.MB.StartCoroutine(blinkPlayerP[i].Routine(playerP[i]));
        }
        
        while (Player == null)
            yield return null;
        
        int numMoves = int.Parse(ClassObjects.Gameobj.DiceScore.text);

        GameObject lastCell = FindLastCell(numMoves, Player, NextPlayerTurn); 
        
        // show movement
        // int currStep = 0;
        // while (currStep < numMoves)
        // {
        //     if (!MovePlayerSingleStep.isRunning)
        //     {
        //         ClassObjects.Gameobj.MB.StartCoroutine(MovePlayerSingleStep.Routine(Player));
        //         currStep++;
        //     }
        //     yield return null;
        // }
        
        // Last Cell Mechanics
        
        Player.GetComponent<PlayerMetaData>().currCell = lastCell;
        lastCell.GetComponent<CellMetaData>().AddPlayer(Player);

        Player = null;
        NextPlayerTurn = (NextPlayerTurn + 1) % NumPlayers;
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
        
        for (int i = 0; i < 4; i++)
        {
            ClassObjects.Gameobj.StopCoroutine(blinkRoutine[i]);
            blinkPlayerP[i].Reset();
        }
    }

    public static GameObject FindLastCell(int numMoves, GameObject player, int playerGroup)
    {
        GameObject currCell = player.GetComponent<PlayerMetaData>().currCell;
        GameObject nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj(playerGroup);

        int i = 1;
        while (i < numMoves && nextCell != null)
        {
            currCell = nextCell;
            nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj(playerGroup);
            i++;
        }

        return nextCell;
    }

}
