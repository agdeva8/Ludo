using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollDice : MonoBehaviour
{
    private bool isRunning;
    [SerializeField] private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetMouseButton(0))
        {
            // Checking whether it is my turn or not
            if (!TurnManager.TM.CheckTurn())
                return;

            // This will let the turn manager know that the 
            // process has started
            TurnManager.TM.StartTurn();
            
            StartCoroutine(RollRoutine());
        }
    }

    public void OnMouseDown()
    {
        // Checking whether it is my turn or not
        if (!TurnManager.TM.CheckTurn())
            return;

        // This will let the turn manager know that the 
        // process has started
        TurnManager.TM.StartTurn();
        
        StartCoroutine(RollRoutine());
    }
    
    public IEnumerator RollRoutine()
    {
        Debug.Log("Dice roll coroutine started");
        if (isRunning)
        {
            Debug.Log("dice routine already running");
            yield break;
        }

        isRunning = true;
        
        // Debug.Log($"Roll Dice Update Coroutine{count}");
        // count++;

        // Blink Current Player Home to notify users of their chance
        // ClassObjects.Gameobj.homeOut[PossibleMoves.CurrPlayerTurn].GetComponent<Blink>().StartRoutine();
        
        // blinkHomeRoutine = ClassObjects.Gameobj.MB.StartCoroutine(blinkHome.Routine(blinkObj));

        Debug.Log("Waiting for dice to get clicked");
        
        // while (!Helper.IsObjClicked(ClassObjects.Gameobj.dice))
        //     yield return null;
        
        // while (!Input.GetMouseButton(button: 0))
        //     yield return null;
        // Debug.Log("Mouse button down");

        // Check if the Dice Falls on rolling
        // checkFallingRoutine = ClassObjects.Gameobj.mb.StartCoroutine(routine: CheckFalling());
        // Updating Value Displaying to the user
        
        // Debug.Log("Starting update dice value routine");
        // updateDiceValueRoutine = ClassObjects.Gameobj.mb.StartCoroutine(routine: UpdateDiceValue.Routine());

        // Rigidbody rb = ClassObjects.Gameobj.diceRb;

        float tx = Random.Range(500, 2000);
        float ty = Random.Range(500, 2000);
        float tz = Random.Range(500, 2000);
        rb.AddForce(Vector3.up * 300);
        rb.AddTorque(tx, ty, tz);

        // Waiting For the Dice To Stop
        int freezedFor = 0;
        while (freezedFor < 2)
        {
            if (ClassObjects.Gameobj.diceRb.angularVelocity == Vector3.zero &&
                ClassObjects.Gameobj.diceRb.velocity == Vector3.zero)
                freezedFor++;
            yield return null;
        }
    
        // Stopping Coroutines
        // ClassObjects.Gameobj.mb.StopCoroutine(checkFallingRoutine);
        // ClassObjects.Gameobj.mb.StopCoroutine(updateDiceValueRoutine);
        
        // time for moving the pawns
        isRunning = false;
        
        // Letting Move Player script is do its job
        int myTeam = TurnManager.TM.myTeam;

        for (int i = 0; i < 4; i++)
            GameObjects.GO.players[myTeam, i].GetComponent<MovePlayer>().amAllowed = true;
        // PossibleMoves.Main();
    }
}
