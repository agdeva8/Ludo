using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Threading;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine.Experimental.XR;

public class MyPlayer : MonoBehaviourPun
{
    public PhotonView PV;
    public int myTeam;

    void Start()
    {
        // Asking for team number via rpc
        if (PV.IsMine)
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        
        // hc1 = GameObject.Find("StartHomeCell00");
        // hc2 = GameObject.Find("StartHomeCell10");
        
        // if (photonView.IsMine)
        // {
        //     if (PhotonNetwork.IsMasterClient)
        //         homeCell = hc1;
        //     else
        //         homeCell = hc2;
        // }
        // else
        // {
        //     if (PhotonNetwork.IsMasterClient)
        //         homeCell = hc2;
        //     else
        //         homeCell = hc1;
        // }

        // // Debug.Log("Group Number is " + groupNum);
        //
    }

    void MoveToBase()
    {
        Debug.Log("Adding player to home cell");
        Debug.Log($"Team number is {myTeam}");
        GameObject homeCell = ClassObjects.Gameobj.homeCells[myTeam].objects[0];
        homeCell.GetComponent<CellMetaData>().players.Clear();
        homeCell.GetComponent<CellMetaData>().AddPlayer(gameObject);
        
        PlayerMetaData playerMetaData = GetComponent<PlayerMetaData>();
        playerMetaData.homeCell = homeCell;
        playerMetaData.currCell = homeCell;
    }


    // RPC Methads
    // This RPC will only be called on master client
    [PunRPC]
    void RPC_GetTeam()
    {
        Debug.Log("in RPC Get Team");
        int team = GameManager.GM.nextPlayerTeam;
        GameManager.GM.UpdateNextPlayerTeam();
        PV.RPC("RPC_SentTeam", RpcTarget.AllBuffered, team);
    }
    
    
    // Team Number will be broadcasted via this 
    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        Debug.Log("in RPC Sent Team");
        myTeam = whichTeam;

        if (PV.IsMine)
        {
            Debug.Log("my team is " + myTeam);
            TurnManager.TM.myTeam = myTeam;
        }
        
        MoveToBase();
    }
}
