using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class MovePlayer
{
    public static GameObject Player;
    public static int NextPlayerTurn = 0;
    public static int NumPlayers = 4;
    public static void Start()
    {
        // NextPlayerTurn = 0;
        // NumPlayers = ClassObjects.Gameobj.NumPlayers;
        // NumPlayers = 4;
    }

    public static IEnumerator Routine()
    {
        int numMoves = int.Parse(ClassObjects.Gameobj.DiceScore.text);
        
        int currStep = 0;
        while (currStep < numMoves)
        {
            if (!MovePlayerSingleStep.isRunning && Player != null)
            {
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
