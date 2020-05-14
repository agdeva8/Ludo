using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovePlayer
{
    public static void Start()
    {
    }

    public static IEnumerator Routine()
    {
        int numMoves = int.Parse(ClassObjects.Gameobj.DiceScore.text);
        
        int currStep = 0;
        while (currStep < numMoves)
        {
            if (!MovePlayerSingleStep.isRunning)
            {
                ClassObjects.Gameobj.MB.StartCoroutine(MovePlayerSingleStep.Routine(ClassObjects.Gameobj.Players[1, 0]));
                currStep++;
            }

            yield return null;
        }

        ClassObjects.Gameobj.MB.StartCoroutine(RollDice.Routine());

    }

}
