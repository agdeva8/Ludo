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
        StartCoroutine(RollDice.Routine());
    }

    void Update()
    {
        // if (Input.GetKeyDown("space"))
        //     StartCoroutine(RollDice.Routine());
    }
}
