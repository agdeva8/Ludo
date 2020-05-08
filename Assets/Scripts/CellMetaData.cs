using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMetaData : MonoBehaviour {
   // GameObj of the next cell (Direct Game Objects pointers and no ids) 
   // private GameObject nextObj = null;
   private GameObject _nextObjSamePlayer;
   public GameObject _nextObjOtherPlayer;
   
   // GameObj ID of previous cell 
   private GameObject _prevObj = null;
   
   // Current Player Rect Portion
   public static int CurrPlayerPortion = 0;
   
   // Check if Curr Cell is Stop
   public bool isStop;
   
   // TODO 
   // Add power variables at later stages 

   public void SetNextGameObj(GameObject otherPlayer, GameObject samePlayer) {
       _nextObjOtherPlayer = otherPlayer;
       _nextObjSamePlayer = samePlayer;
   }
   
   public void SetNextGameObj(GameObject otherPlayer) {
        SetNextGameObj(otherPlayer, otherPlayer);
   }
   
   public GameObject GetNextGameObj(int player) {
       return player == CurrPlayerPortion ? _nextObjSamePlayer : _nextObjOtherPlayer;
   }
   
   public GameObject GetNextGameObj()
   {
       return _nextObjOtherPlayer;
   }
}
