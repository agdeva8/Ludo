using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RollDice
{
    private static readonly float deltaY = 3;
    private static int frozeFor;
    // private static int count = 0;
    private static bool isRunning = false;
    

    // Update is called once per frame
    public static IEnumerator Routine()
    {
        if (isRunning)
            yield break;

        isRunning = true;
        // Debug.Log($"Roll Dice Update Coroutine{count}");
        // count++;

        while (!Input.GetMouseButton(0))
            yield return null;

        // Check if the Dice Falls on rolling
        GameObjects.MB.StartCoroutine(CheckFalling());
        // Updating Value Displaying to the user
        GameObjects.MB.StartCoroutine(routine: UpdateDiceValue.Routine());
        
        // GameObjects.MB.StopCoroutine(routine: UpdateDiceValue.Routine());
        
        while (!Input.GetMouseButtonDown(0))
            yield return null;
        
        // Debug.Log("Mouse button down");
        var position = GameObjects.DiceTransform.position;
        position = new Vector3(position.x, position.y + deltaY, position.z);
        GameObjects.DiceTransform.position = position;
        GameObjects.DiceRb.angularVelocity = new Vector3(1000, 1000, 1000);
        
        // Waiting For the Dice To Stop
        frozeFor = 0;
        while (frozeFor < 3)
        {
            if (GameObjects.DiceRb.angularVelocity == Vector3.zero &&
                GameObjects.DiceRb.velocity == Vector3.zero)
                frozeFor++;
            yield return null;
        }
    
        // Stopping Coroutines
        GameObjects.MB.StopCoroutine(CheckFalling());
        GameObjects.MB.StopCoroutine(routine: UpdateDiceValue.Routine());
        GameObjects.MB.StopCoroutine(Routine());
        isRunning = false;
    }
    
    // Check if the Dice Falls of the table
    private static IEnumerator CheckFalling()
    {
        while (true)
        {
            if (GameObjects.DiceTransform.position.y < -10)
            {
                GameObjects.DiceTransform.localPosition = new Vector3(0, deltaY, 0);
                GameObjects.DiceRb.angularVelocity = Vector3.zero; 
                GameObjects.DiceRb.velocity = Vector3.zero; 
            }
            yield return null;
        }
    }
}
