using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMetaData : MonoBehaviour {
   // GameObj of the next cell (Direct Game Objects pointers and no ids) 
   // private GameObject nextObj = null;
   private static GameObject nextObjSamePlayer;
   private static GameObject nextObjOtherPlayer;
   
   // GameObj ID of previous cell 
   private GameObject prevObj = null;
   
   // Current Player Rect Portion
   public static int CurrPlayerPortion = 0;
   
   // Check if Curr Cell is Stop
   public bool isStop;
   
   // TODO 
   // Add power variables at later stages 

   public static void SetNextGameObj(GameObject otherPlayer, GameObject samePlayer) {
       nextObjOtherPlayer = otherPlayer;
       nextObjSamePlayer = samePlayer;
   }
   
   public void SetNextGameObj(GameObject otherPlayer) {
       SetNextGameObj(otherPlayer, otherPlayer);
   }
   
   public static GameObject GetNextGameObj(int player) {
       return player == CurrPlayerPortion ? nextObjSamePlayer : nextObjOtherPlayer;
   }
}
