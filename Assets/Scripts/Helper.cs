using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static bool IsObjClicked(GameObject obj)
    {
        
        if (!Input.GetMouseButtonDown (0))
            return false;

        RaycastHit hitInfo = new RaycastHit ();
        if (Camera.main != null && 
                Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo))
            // Debug.Log ("Object Hit is " + hitInfo.collider.gameObject.name);

            //If you want it to only detect some certain game object it hits, you can do that here
            if (hitInfo.collider.gameObject == obj)
            {
                Debug.Log("obj hit");
                return true;
            }
            

        return false;
    }
}
