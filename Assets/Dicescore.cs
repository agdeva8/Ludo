using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dicescore : MonoBehaviour
{
    public Vector3 DiceAngle;
    public string DiceNum;
    public GameObject Dice;

    // Start is called before the first frame update
    void Start()
    {
        // Dice Game Object;
        Dice = GameObject.Find("Dice");
        
        // if (Dice == null)
        //     Debug.Log("dice is null");
        // else
        //     Debug.Log("angle is " + Dice.transform.rotation.eulerAngles);
        DiceAngle = Dice.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        DiceAngle = Dice.transform.rotation.eulerAngles;
        DiceNum  = GetNum().ToString();
        // Debug.Log("Dice Num is " + DiceNum);
        Debug.Log("Dice angle is " + DiceAngle);
        GetComponent<TextMeshProUGUI>().text = DiceNum;
    }

    int GetNum()
    {
        int diceNum = 0;
        if (IsAngleEqual(DiceAngle.x, 0))
        {
            // Debug.Log("in 0");
            if (IsAngleEqual(DiceAngle.z, 0))
            {
                diceNum = 2;
            }
            else if (IsAngleEqual(DiceAngle.z, 90))
                diceNum = 6;
            else if (IsAngleEqual(DiceAngle.z, 180))
                diceNum = 5;
            else if (IsAngleEqual(DiceAngle.z, 270))
            {
                diceNum = 1;
            }
        }            
        else if (IsAngleEqual(DiceAngle.x, 90))
            diceNum = 4;
        else if (IsAngleEqual(DiceAngle.x, 180))
        {
            if (IsAngleEqual(DiceAngle.z, 0))
                diceNum = 5;
            else if (IsAngleEqual(DiceAngle.z, 90))
                diceNum = 4;
            else if (IsAngleEqual(DiceAngle.z, 180))
                diceNum = 2;
            else if (IsAngleEqual(DiceAngle.z, 270))
                diceNum = 3;
        }
        else if (IsAngleEqual(DiceAngle.x, 270))
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
