using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class UpdateDiceValue
{
    private static Transform transform;
    public static IEnumerator Routine()
    {
        transform = ClassObjects.Gameobj.diceTransform;
        while (true)
        {
            string displayNum = GetDiceCount().ToString();
            ClassObjects.Gameobj.diceScore.text = displayNum;
            yield return null;
        }
    }

    // This code is taken from http://www.theappguruz.com/blog/roll-a-dice-unity-3d
    // Dice Numbers are changed
    static int GetDiceCount()
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
