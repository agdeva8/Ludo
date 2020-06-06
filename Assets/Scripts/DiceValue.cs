using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceValue : MonoBehaviour
{
    // It is dice transform
    public Transform transform;
    public TextMeshProUGUI score;

    public void Update()
    {
        string displayNum = GetDiceCount().ToString();
        score.text = displayNum;
    }

    public IEnumerator Routine()
    {
        while (true)
        {
            string displayNum = GetDiceCount().ToString();
            score.text = displayNum;
            yield return null;
        }
    }
    
    // This code is taken from http://www.theappguruz.com/blog/roll-a-dice-unity-3d
    // Dice Numbers are changed
    int GetDiceCount()
    {
        int diceCount = 0;
        
        if (Vector3.Dot (transform.forward, Vector3.up) > 0.6f)
            diceCount = 6;
        if (Vector3.Dot (-transform.forward, Vector3.up) > 0.6f)
            diceCount = 1;
        if (Vector3.Dot (transform.up, Vector3.up) > 0.6f)
            diceCount = 5;
        if (Vector3.Dot (-transform.up, Vector3.up) > 0.6f)
            diceCount = 2;
        if (Vector3.Dot (transform.right, Vector3.up) > 0.6f)
            diceCount = 4;
        if (Vector3.Dot (-transform.right, Vector3.up) > 0.6f)
            diceCount = 3;

        return diceCount;
    }
}
