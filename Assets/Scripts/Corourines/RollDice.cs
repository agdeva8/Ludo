using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RollDice
{
    private static readonly float DeltaY = 3;
    private static readonly int rollingTime = 4;
    private static int frozeFor;
    // private static int count = 0;
    private static bool isRunning = false;
    private static Coroutine checkFallingRoutine; 
    private static Coroutine updateDiceValueRoutine;
    
    // Update is called once per frame
    public static IEnumerator Routine()
    {
        if (isRunning) yield break;

        isRunning = true;
        // Debug.Log($"Roll Dice Update Coroutine{count}");
        // count++;

        while (!Helper.IsObjClicked(ClassObjects.Gameobj.Dice))
            yield return null;
        // while (!Input.GetMouseButton(button: 0))
        //     yield return null;
        // Debug.Log("Mouse button down");

        // Check if the Dice Falls on rolling
        checkFallingRoutine = ClassObjects.Gameobj.MB.StartCoroutine(routine: CheckFalling());
        // Updating Value Displaying to the user
        updateDiceValueRoutine = ClassObjects.Gameobj.MB.StartCoroutine(routine: UpdateDiceValue.Routine());

        var position = ClassObjects.Gameobj.DiceTransform.position;

        Rigidbody rb = ClassObjects.Gameobj.DiceRb;

        float tx = Random.Range(100, 2000);
        float ty = Random.Range(100, 2000);
        float tz = Random.Range(100, 2000);
        rb.AddForce(Vector3.up * 500);
        rb.AddTorque(tx, ty, tz);

        // position = new Vector3(x: position.x, y: position.y + DeltaY, z: position.z);
        // ClassObjects.Gameobj.DiceTransform.position = position;
        // ClassObjects.Gameobj.DiceRb.angularVelocity = new Vector3(x: 2000, y: 0, 0);
        // ClassObjects.Gameobj.DiceRb.angularVelocity = new Vector3(
        //     Random.Range(0, 1000) + 14000,
        //     0, 0);
        //                                 // Random.Range(-1000, 1000) + 14000,
        //                                 // Random.Range(-1000, 1000) + 14000);
        //
        // ClassObjects.Gameobj.DiceRb.constraints = RigidbodyConstraints.FreezePositionY;
        // yield return new WaitForSeconds(0.3f);
        // ClassObjects.Gameobj.DiceRb.constraints = RigidbodyConstraints.None;
        //
        // ClassObjects.Gameobj.DiceRb.AddRelativeTorque(new Vector3(2f, 2f, 2f)) ;
        
        // Rolling Dice  for some frames and keeping it up in the air for that long
        // ClassObjects.Gameobj.DiceRb.constraints = RigidbodyConstraints.FreezePositionY;
        //
        // for (int i = 0; i < rollingTime; i++)
        // {
        //     // ClassObjects.Gameobj.DiceRb.AddTorque(new Vector3(
        //     //                                 Random.Range(-2, 2),
        //     //                                 Random.Range(-2, 2),
        //     //                                 Random.Range(-2, 2)));
        //     yield return null;
        // }
        // ClassObjects.Gameobj.DiceRb.constraints = RigidbodyConstraints.None;
        
        
        // Waiting For the Dice To Stop
        frozeFor = 0;
        while (frozeFor < 2)
        {
            if (ClassObjects.Gameobj.DiceRb.angularVelocity == Vector3.zero &&
                ClassObjects.Gameobj.DiceRb.velocity == Vector3.zero)
                frozeFor++;
            yield return null;
        }
    
        // Stopping Coroutines
        ClassObjects.Gameobj.MB.StopCoroutine(checkFallingRoutine);
        ClassObjects.Gameobj.MB.StopCoroutine(updateDiceValueRoutine);
        
        // Moving Player by one step
        // ClassObjects.Gameobj.MB.StartCoroutine(MovePlayerSingleStep.Routine(ClassObjects.Gameobj.DebugPlayer));
        ClassObjects.Gameobj.MB.StartCoroutine(MovePlayer.Routine());
        isRunning = false;
    }
    
    // Check if the Dice Falls of the table
    private static IEnumerator CheckFalling()
    {
        while (true)
        {
            if (ClassObjects.Gameobj.DiceTransform.position.y < -10)
            {
                ClassObjects.Gameobj.DiceTransform.localPosition = new Vector3(0, DeltaY, 0);
                ClassObjects.Gameobj.DiceRb.angularVelocity = Vector3.zero; 
                ClassObjects.Gameobj.DiceRb.velocity = Vector3.zero; 
            }

            if (ClassObjects.Gameobj.DiceTransform.position.y > 1)
            {
                float tx = Random.Range(0, 200);
                float ty = Random.Range(0, 200);
                float tz = Random.Range(0, 200);
                ClassObjects.Gameobj.DiceRb.AddTorque(tx, ty, tz);
            }
            
            yield return null;
        }
    }
    
}
