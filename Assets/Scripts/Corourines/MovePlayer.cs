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
        int numMoves = int.Parse(GameObjects.DiceScore.text);
        
        int currStep = 0;
        while (currStep < numMoves)
        {
            if (!MovePlayerSingleStep.isRunning)
            {
                GameObjects.MB.StartCoroutine(MovePlayerSingleStep.Routine(GameObjects.DebugPlayer));
                currStep++;
            }

            yield return null;
        }

        GameObjects.MB.StartCoroutine(RollDice.Routine());

    }

}
