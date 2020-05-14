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
        if (isRunning)
            yield break;

        isRunning = true;
        // Debug.Log($"Roll Dice Update Coroutine{count}");
        // count++;

        while (!Helper.IsObjClicked(GameObjects.Dice))
            yield return null;
        // while (!Input.GetMouseButton(button: 0))
        //     yield return null;
        // Debug.Log("Mouse button down");

        // Check if the Dice Falls on rolling
        checkFallingRoutine = GameObjects.MB.StartCoroutine(routine: CheckFalling());
        // Updating Value Displaying to the user
        updateDiceValueRoutine = GameObjects.MB.StartCoroutine(routine: UpdateDiceValue.Routine());

        var position = GameObjects.DiceTransform.position;

        Rigidbody rb = GameObjects.DiceRb;

        float tx = Random.Range(100, 2000);
        float ty = Random.Range(100, 2000);
        float tz = Random.Range(100, 2000);
        rb.AddForce(Vector3.up * 500);
        rb.AddTorque(tx, ty, tz);

        // position = new Vector3(x: position.x, y: position.y + DeltaY, z: position.z);
        // GameObjects.DiceTransform.position = position;
        // // GameObjects.DiceRb.angularVelocity = new Vector3(x: 1000, y: 1000, z: 1000);
        // GameObjects.DiceRb.angularVelocity = new Vector3(
        //     Random.Range(0, 1000) + 14000,
        //     0, 0);
        //                                 // Random.Range(-1000, 1000) + 14000,
        //                                 // Random.Range(-1000, 1000) + 14000);
        //
        // GameObjects.DiceRb.constraints = RigidbodyConstraints.FreezePositionY;
        // yield return new WaitForSeconds(0.3f);
        // GameObjects.DiceRb.constraints = RigidbodyConstraints.None;
        //
        // GameObjects.DiceRb.AddRelativeTorque(new Vector3(2f, 2f, 2f)) ;
        
        // Rolling Dice  for some frames and keeping it up in the air for that long
        // GameObjects.DiceRb.constraints = RigidbodyConstraints.FreezePositionY;
        //
        // for (int i = 0; i < rollingTime; i++)
        // {
        //     // GameObjects.DiceRb.AddTorque(new Vector3(
        //     //                                 Random.Range(-2, 2),
        //     //                                 Random.Range(-2, 2),
        //     //                                 Random.Range(-2, 2)));
        //     yield return null;
        // }
        // GameObjects.DiceRb.constraints = RigidbodyConstraints.None;
        
        
        // Waiting For the Dice To Stop
        frozeFor = 0;
        while (frozeFor < 2)
        {
            if (GameObjects.DiceRb.angularVelocity == Vector3.zero &&
                GameObjects.DiceRb.velocity == Vector3.zero)
                frozeFor++;
            yield return null;
        }
    
        // Stopping Coroutines
        GameObjects.MB.StopCoroutine(checkFallingRoutine);
        GameObjects.MB.StopCoroutine(updateDiceValueRoutine);
        
        // Moving Player by one step
        // GameObjects.MB.StartCoroutine(MovePlayerSingleStep.Routine(GameObjects.DebugPlayer));
        GameObjects.MB.StartCoroutine(MovePlayer.Routine());
        isRunning = false;
    }
    
    // Check if the Dice Falls of the table
    private static IEnumerator CheckFalling()
    {
        while (true)
        {
            if (GameObjects.DiceTransform.position.y < -10)
            {
                GameObjects.DiceTransform.localPosition = new Vector3(0, DeltaY, 0);
                GameObjects.DiceRb.angularVelocity = Vector3.zero; 
                GameObjects.DiceRb.velocity = Vector3.zero; 
            }
            yield return null;
        }
    }
    
}
