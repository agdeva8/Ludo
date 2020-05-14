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
   public GameObject prevObj;
   
   // Current Player Rect Portion
   public int playerPortion = 0;
   
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
   
   // Setting Next GameObject along with previous
   public void SetNextPrevGameObj(GameObject otherPlayer, GameObject samePlayer) {
       nextObjOtherPlayer = otherPlayer;
       nextObjSamePlayer = samePlayer;
       otherPlayer.GetComponent<CellMetaData>().prevObj = this.gameObject;
       samePlayer.GetComponent<CellMetaData>().prevObj = this.gameObject;
   }
   public void SetNextPrevGameObj(GameObject otherPlayer)
   {
       SetNextPrevGameObj(otherPlayer, otherPlayer);
   }
   
   
   public GameObject GetNextGameObj(int player) {
       return player == playerPortion ? nextObjSamePlayer : nextObjOtherPlayer;
   }
   
   public GameObject GetNextGameObj()
   {
       return nextObjOtherPlayer;
   }
}
