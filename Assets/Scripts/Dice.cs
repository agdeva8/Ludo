using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
            StartCoroutine(RollDice.Routine());
    }

    // // Height it will reach while rolling
    // public float deltaY;
    //
    // public int freezedFor;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     deltaY = 3;
    // }
    //
    // private void OnMouseDown()
    // {
    //     freezedFor = 0;
    //     transform.position = new Vector3(
    //                             transform.position.x,
    //                             transform.position.y + deltaY,
    //                             transform.position.z);
    //     GameObjects.DiceRb.angularVelocity = new Vector3(1000, 1000, 1000);
    //
    //     StartCoroutine(routine: UpdateDiceValue.Update());
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     if (GameObjects.DiceTransform.position.y < -10)
    //     {
    //         GameObjects.DiceTransform.localPosition = new Vector3(0, deltaY, 0);
    //         GameObjects.DiceRb.angularVelocity = Vector3.zero; 
    //         GameObjects.DiceRb.velocity = Vector3.zero; 
    //     }
    //
    //     if (GameObjects.DiceRb.angularVelocity == Vector3.zero && rb.velocity == Vector3.zero)
    //         freezedFor++;
    //
    //     if (freezedFor > 2)
    //     {
    //         GetComponent<Dice>().enabled = false;
    //         StopCoroutine(UpdateDiceValue.Update());
    //     }
    // }
}
