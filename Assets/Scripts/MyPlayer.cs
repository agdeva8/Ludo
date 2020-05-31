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
    public int myPawn;

    void Start()
    {
        // Assigning Pawn Number and Team number
        if (PV.IsMine)
        {
            Debug.Log("Making Request for team number if not done");
            // Asking for team number via RPC
            if (!TurnManager.TM.teamNumRequested)
            {
                TurnManager.TM.teamNumRequested = true;
                PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
            }
        }
    }

    void MoveToBase()
    {
        Debug.Log("Adding player to home cell");
        Debug.Log($"Team number is {myTeam}");
        Debug.Log($"pawn number is {myPawn}");

        if (myPawn == -1)
            Debug.LogError("Pawn number for this instantiate is invalid: " +
                           "must be a problem with team number");
        
        // Referencing this object in gameobects script;
        GameObjects.GO.players[myTeam, myPawn] = gameObject;
        
        GameObject homeCell = ClassObjects.Gameobj.homeCells[myTeam].objects[myPawn];
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

        if (PV.IsMine)
        {
            TurnManager.TM.myTeam = whichTeam;
            for (int i = 0; i < 4; i++)
            {
                GameManager.GM.players[i].GetComponent<MyPlayer>().SetTeamNPawn(whichTeam, i);
            }
        }
    }

    void SetTeamNPawn(int teamNum, int pawnNum)
    {
        PV.RPC("RPC_SentTeamNPawn", RpcTarget.AllBuffered, teamNum, pawnNum);
    }
    
    // Now Getting Pawn number from RPC methods
    // This RPC will only be called on master client
    [PunRPC]
    void RPC_GetPawn(int teamNum)
    {
        Debug.Log("in RPC Get Pawn");
        int pawnNum = GameManager.GM.GetPawn(teamNum); 
        PV.RPC("RPC_SentPawn", RpcTarget.AllBuffered, pawnNum);
    }
    
    
    // Pawn Number will be broadcasted via this 
    [PunRPC]
    void RPC_SentPawn(int whichPawn)
    {
        Debug.Log("in RPC Sent Pawn");
        myPawn = whichPawn;
        MoveToBase();
    }
    
    // Team and Pawn Number will be broadcasted via this 
    [PunRPC]
    void RPC_SentTeamNPawn(int whichTeam, int whichPawn)
    {
        Debug.Log("in RPC Sent Pawn");
        myTeam = whichTeam;
        myPawn = whichPawn;
        MoveToBase();
    }
}
