using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Dicescore : MonoBehaviour
{
    [FormerlySerializedAs("DiceAngle")] public Vector3 diceAngle;
    [FormerlySerializedAs("DiceNum")] public string diceNum;
    [FormerlySerializedAs("Dice")] public GameObject dice;

    // Start is called before the first frame update
    void Start()
    {
        // Dice Game Object;
        dice = GameObject.Find("Dice");
        
        // if (Dice == null)
        //     Debug.Log("dice is null");
        // else
        //     Debug.Log("angle is " + Dice.transform.rotation.eulerAngles);
        diceAngle = dice.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        diceAngle = dice.transform.rotation.eulerAngles;
        diceNum  = GetNum().ToString();
        // Debug.Log("Dice Num is " + DiceNum);
        Debug.Log("Dice angle is " + diceAngle);
        GetComponent<TextMeshProUGUI>().text = diceNum;
    }

    int GetNum()
    {
        int diceNum = 0;
        if (IsAngleEqual(diceAngle.x, 0))
        {
            // Debug.Log("in 0");
            if (IsAngleEqual(diceAngle.z, 0))
            {
                diceNum = 2;
            }
            else if (IsAngleEqual(diceAngle.z, 90))
                diceNum = 6;
            else if (IsAngleEqual(diceAngle.z, 180))
                diceNum = 5;
            else if (IsAngleEqual(diceAngle.z, 270))
            {
                diceNum = 1;
            }
        }            
        else if (IsAngleEqual(diceAngle.x, 90))
            diceNum = 4;
        else if (IsAngleEqual(diceAngle.x, 180))
        {
            if (IsAngleEqual(diceAngle.z, 0))
                diceNum = 5;
            else if (IsAngleEqual(diceAngle.z, 90))
                diceNum = 4;
            else if (IsAngleEqual(diceAngle.z, 180))
                diceNum = 2;
            else if (IsAngleEqual(diceAngle.z, 270))
                diceNum = 3;
        }
        else if (IsAngleEqual(diceAngle.x, 270))
            diceNum = 3;

        return diceNum;
    }

    bool IsAngleEqual(float angle, float angleCompTo)
    {
        float minRange = (angleCompTo - 45) % 360;
        float maxRange = (angleCompTo + 45) % 360;

        float normalisedAngle = angle % 360;
        
        // Handling Case for angleCompTo is 0;
        if (normalisedAngle >= 315 || normalisedAngle <= 45)
            return (angleCompTo == 0.0f); 
        
        return (normalisedAngle >= minRange && normalisedAngle <= maxRange);
    }
}
