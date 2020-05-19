using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkHome
{
    public  float MultiplyFactor;
    public  float UpdateStep;
    public  float BlinkVelocity;
    public  Material ObjMaterial;
    public  Color OrigColor;
    public  Color NewColor;
    public  float Duration = 1;
    public  float Smoothness = 0.02f;
    // public  float increment = smoothness / duration;

    public IEnumerator Routine(GameObject obj)
    {
        // GameObject obj = ClassObjects.Gameobj.HomeOut[PlayerGroup];
        MultiplyFactor = 1;
        UpdateStep = 0.04f;

        //  Wait for seconds before second update
        BlinkVelocity = 0.02f;
        ObjMaterial = obj.GetComponent<Renderer>().material;
        OrigColor = ObjMaterial.color;
        NewColor = OrigColor * 1.9f;

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
            Color newColor = OrigColor * MultiplyFactor;
            
            // objMaterial.SetColor("_Color", value: objMaterial.GetColor() * multiplyFactor);
            ObjMaterial.SetColor("_Color", newColor);
            
            if (MultiplyFactor > 1.9)
            {
                UpdateStep *= -1;
            }
            else if (MultiplyFactor < 0.8)
            {
                UpdateStep *= -1;
            }
            
            MultiplyFactor += UpdateStep; 
            yield return new WaitForSeconds(BlinkVelocity);
        }
    }

    public void Reset()
    {
        ObjMaterial.SetColor("_Color", OrigColor);
    }
}
