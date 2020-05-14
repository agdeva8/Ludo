using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CellMetaData : MonoBehaviour {
   // GameObj of the next cell (Direct Game Objects pointers and no ids) 
   // private GameObject nextObj = null;
   public GameObject nextObjSamePlayer;
   public GameObject nextObjOtherPlayer;
   
   // GameObj ID of previous cell 
   private GameObject _prevObj = null;
   
   // Current Player Rect Portion
   public int PlayerPortion = 0;
   
   // Check if Curr Cell is Stop
   public bool isStop;
   
   // TODO 
   // Add power variables at later stages 

   public void SetNextGameObj(GameObject otherPlayer, GameObject samePlayer) {
       nextObjOtherPlayer = otherPlayer;
       nextObjSamePlayer = samePlayer;
   }
   
   public void SetNextGameObj(GameObject otherPlayer) {
        SetNextGameObj(otherPlayer, otherPlayer);
   }
   
   public GameObject GetNextGameObj(int player) {
       return player == PlayerPortion ? nextObjSamePlayer : nextObjOtherPlayer;
   }
   
   public GameObject GetNextGameObj()
   {
       return nextObjOtherPlayer;
   }
}
