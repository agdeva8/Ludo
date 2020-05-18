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
    public static void Start()
    {
        MovablePlayersRoutines = new Coroutine[4];
        // NextPlayerTurn = 0;
        // NumPlayers = ClassObjects.Gameobj.NumPlayers;
        // NumPlayers = 4;
    }

    public static IEnumerator Routine()
    {
        // PlayerP stands for playerPawn
        
        // Make Real Player notice of their players
        BlinkHome[] blinkPlayerP = new BlinkHome[4];
        GameObject[] playerP = new GameObject[4];
        Coroutine[] blinkRoutine = new Coroutine[4];
        for (int i = 0; i < 4; i++)
        {
            blinkPlayerP[i] = new BlinkHome();
            playerP[i] = ClassObjects.Gameobj.Players[NextPlayerTurn, i];
            blinkRoutine[i] = ClassObjects.Gameobj.MB.StartCoroutine(blinkPlayerP[i].Routine(playerP[i]));
        }
        
        for (int i = 0; i < 4; i++)
        {
            // BlinkHome blinkHomeObj = new BlinkHome();
            // MovablePlayersRoutines[i] = ClassObjects.Gameobj.MB.StartCoroutine(
            //     blinkHomeObj.Routine(ClassObjects.Gameobj.Players[NextPlayerTurn, i]));
        }
        
        int numMoves = int.Parse(ClassObjects.Gameobj.DiceScore.text);
        
        int currStep = 0;
        while (currStep < numMoves)
        {
            if (!MovePlayerSingleStep.isRunning && Player != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    ClassObjects.Gameobj.StopCoroutine(blinkRoutine[i]);
                    blinkPlayerP[i].Reset();
                }
                // Stoppng Blinking Player coroutine
                // for (int i = 0; i < 4; i++)
                //     ClassObjects.Gameobj.MB.StopCoroutine(MovablePlayersRoutines[i]); 
                
                ClassObjects.Gameobj.MB.StartCoroutine(MovePlayerSingleStep.Routine(Player));
                currStep++;
            }

            yield return null;
        }

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
        // Player = EventSystem.current.currentSelectedGameObject;
        // Player = ClassObjects.Gameobj.Players[0,0];
    }

}
