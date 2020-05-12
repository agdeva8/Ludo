using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class UpdateDiceValue
{
    public static IEnumerator Routine()
    {
        while (true)
        {
            string displayNum = GetNum().ToString();
            GameObjects.DiceScore.text = displayNum;
            yield return null;
        }
    }

    static int GetNum()
    {
        int diceNum = 0;
        Vector3 diceAngle = GameObjects.DiceTransform.rotation.eulerAngles;

        if (IsAngleEqual( diceAngle.x, 0))
        {
            // Debug.Log("in 0");
            if (IsAngleEqual( diceAngle.z, 0))
            {
                 diceNum = 2;
            }
            else if (IsAngleEqual( diceAngle.z, 90))
                 diceNum = 6;
            else if (IsAngleEqual( diceAngle.z, 180))
                 diceNum = 5;
            else if (IsAngleEqual( diceAngle.z, 270))
            {
                 diceNum = 1;
            }
        }            
        else if (IsAngleEqual( diceAngle.x, 90))
             diceNum = 4;
        else if (IsAngleEqual( diceAngle.x, 180))
        {
            if (IsAngleEqual( diceAngle.z, 0))
                 diceNum = 5;
            else if (IsAngleEqual( diceAngle.z, 90))
                 diceNum = 4;
            else if (IsAngleEqual( diceAngle.z, 180))
                 diceNum = 2;
            else if (IsAngleEqual( diceAngle.z, 270))
                 diceNum = 3;
        }
        else if (IsAngleEqual( diceAngle.x, 270))
             diceNum = 3;

        return  diceNum;
    }

    static bool IsAngleEqual(float angle, float angleCompTo)
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
