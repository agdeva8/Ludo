using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dice1 : MonoBehaviour
{
    void Start()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 30;
        
        StartCoroutine(RollDice.Routine());
    }

    void Update()
    {
        // if (Input.GetKeyDown("space"))
        //     StartCoroutine(RollDice.Routine());
    }
}
