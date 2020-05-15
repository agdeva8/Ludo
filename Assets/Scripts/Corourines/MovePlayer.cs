using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class MovePlayer
{
    public static GameObject Player;
    public static void Start()
    {
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
        ClassObjects.Gameobj.MB.StartCoroutine(RollDice.Routine());
    }

    public static void PlayerToMove(GameObject player)
    {
        if (Player != null)
            return;

        Player = player;
        // Player = EventSystem.current.currentSelectedGameObject;
        // Player = ClassObjects.Gameobj.Players[0,0];
    }

}
