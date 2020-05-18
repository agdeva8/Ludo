using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkHome
{
    public  float multiplyFactor;
    public  float updateStep;
    public  float blinkVelocity;
    public  Material objMaterial;
    public  Color origColor;
    public  Color newColor;
    public  float duration = 1;
    public  float smoothness = 0.02f;
    // public  float increment = smoothness / duration;

    public IEnumerator Routine(GameObject obj)
    {
        // GameObject obj = ClassObjects.Gameobj.HomeOut[PlayerGroup];
        multiplyFactor = 1;
        updateStep = 0.04f;

        //  Wait for seconds before second update
        blinkVelocity = 0.02f;
        objMaterial = obj.GetComponent<Renderer>().material;
        origColor = objMaterial.color;
        newColor = origColor * 1.9f;

        // while (true
        // {
        //     float progress = 0;
        //     while (progress < 1)
        //     {
        //         Color.Lerp(origColor, newColor, progress);
        //         progress += increment;
        //         yield return new WaitForSeconds(smoothness);
        //     }
        //
        //     progress = 0;
        //     while (progress < 1)
        //     {
        //         Color.Lerp(origColor, newColor, progress);
        //         progress += increment;
        //         yield return new WaitForSeconds(smoothness);
        //     }

        // yield return null;
        // }

        while (true)
        {
            Color newColor = origColor * multiplyFactor;
            
            // objMaterial.SetColor("_Color", value: objMaterial.GetColor() * multiplyFactor);
            objMaterial.SetColor("_Color", newColor);
            
            if (multiplyFactor > 1.9)
            {
                updateStep *= -1;
            }
            else if (multiplyFactor < 0.8)
            {
                updateStep *= -1;
            }
            
            multiplyFactor += updateStep; 
            yield return new WaitForSeconds(blinkVelocity);
        }
    }

    public void Reset()
    {
        objMaterial.SetColor("_Color", origColor);
    }
}
