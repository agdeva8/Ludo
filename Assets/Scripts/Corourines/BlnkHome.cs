using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlinkHome
{
    public static float multiplyFactor;
    public static float updateStep;
    public static float blinkVelocity;
    public static Material objMaterial;
    public static Color origColor;
    public static IEnumerator Routine(int PlayerGroup)
    {
        GameObject obj = ClassObjects.Gameobj.HomeOut[PlayerGroup];
        multiplyFactor = 1;
        updateStep = 0.05f;
        
        //  Wait for seconds before second update
        blinkVelocity = 0.02f;
        objMaterial = obj.GetComponent<Renderer>().material;
        origColor = objMaterial.color;

        while (true)
        {
            Color newColor = origColor * multiplyFactor;
            
            // objMaterial.SetColor("_Color", value: objMaterial.GetColor() * multiplyFactor);
            objMaterial.SetColor("_Color", newColor);

            if (multiplyFactor > 1.9)
            {
                updateStep *= -1;
            }
            else if (multiplyFactor < 1)
            {
                updateStep *= -1;
            }

            multiplyFactor += updateStep; 
            yield return new WaitForSeconds(blinkVelocity);
        }
    }

    public static void Reset()
    {
        objMaterial.SetColor("_Color", origColor);
    }
}
