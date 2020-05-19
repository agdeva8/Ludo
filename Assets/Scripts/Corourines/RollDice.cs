using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RollDice
{
    private static readonly float DeltaY = 3;
    private static readonly int RollingTime = 4;
    private static int frozeFor;
    // private static int count = 0;
    private static bool isRunning = false;
    private static Coroutine checkFallingRoutine; 
    private static Coroutine updateDiceValueRoutine;
    private static Coroutine blinkHomeRoutine;
    private static BlinkHome blinkHome;
    
    // Update is called once per frame
    public static IEnumerator Routine()
    {
        if (isRunning) yield break;

        isRunning = true;
        
        
        // Debug.Log($"Roll Dice Update Coroutine{count}");
        // count++;

        // Blink Current Player Home to notify users of their chance
        ClassObjects.Gameobj.homeOut[PossibleMoves.CurrPlayerTurn].GetComponent<Blink>().StartRoutine();
        
        // blinkHomeRoutine = ClassObjects.Gameobj.MB.StartCoroutine(blinkHome.Routine(blinkObj));
        
        while (!Helper.IsObjClicked(ClassObjects.Gameobj.dice))
            yield return null;
        // while (!Input.GetMouseButton(button: 0))
        //     yield return null;
        // Debug.Log("Mouse button down");

        // Check if the Dice Falls on rolling
        checkFallingRoutine = ClassObjects.Gameobj.mb.StartCoroutine(routine: CheckFalling());
        // Updating Value Displaying to the user
        updateDiceValueRoutine = ClassObjects.Gameobj.mb.StartCoroutine(routine: UpdateDiceValue.Routine());

        var position = ClassObjects.Gameobj.diceTransform.position;

        Rigidbody rb = ClassObjects.Gameobj.diceRb;

        float tx = Random.Range(100, 2000);
        float ty = Random.Range(100, 2000);
        float tz = Random.Range(100, 2000);
        rb.AddForce(Vector3.up * 1000);
        rb.AddTorque(tx, ty, tz);

        // Waiting For the Dice To Stop
        frozeFor = 0;
        while (frozeFor < 2)
        {
            if (ClassObjects.Gameobj.diceRb.angularVelocity == Vector3.zero &&
                ClassObjects.Gameobj.diceRb.velocity == Vector3.zero)
                frozeFor++;
            yield return null;
        }
    
        // Stopping Coroutines
        ClassObjects.Gameobj.mb.StopCoroutine(checkFallingRoutine);
        ClassObjects.Gameobj.mb.StopCoroutine(updateDiceValueRoutine);
        
        // Stop home cell blinking
        ClassObjects.Gameobj.homeOut[PossibleMoves.CurrPlayerTurn].GetComponent<Blink>().Stop();
        
        // time for moving the pawns
        isRunning = false;
        PossibleMoves.Main();
    }
    
    // Check if the Dice Falls of the table
    private static IEnumerator CheckFalling()
    {
        while (true)
        {
            if (ClassObjects.Gameobj.diceTransform.position.y < -10)
            {
                ClassObjects.Gameobj.diceTransform.localPosition = new Vector3(0, DeltaY, 0);
                ClassObjects.Gameobj.diceRb.angularVelocity = Vector3.zero; 
                ClassObjects.Gameobj.diceRb.velocity = Vector3.zero; 
            }

            if (ClassObjects.Gameobj.diceTransform.localPosition.y > 5)
            {
                float tx = Random.Range(0, 200);
                float ty = Random.Range(0, 200);
                float tz = Random.Range(0, 200);
                ClassObjects.Gameobj.diceRb.AddTorque(tx, ty, tz);
            }
            
            yield return null;
        }
    }
    
}
