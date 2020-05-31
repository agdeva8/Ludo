using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurnManager : MonoBehaviourPun
{
    public static TurnManager TM;
    public PhotonView PV;
    public int myTeam;
    public int currTeamTurn;
    public bool isMyTurn;
    public int numberOnlinePlayers;
    
    public bool running;
    
    public int prevTeam;
    private bool firstTurn;
    public bool teamNumRequested;

    // Start is called before the first frame update
    void Start()
    {
        if (TM == null || TM != this)
            TM = this;

        numberOnlinePlayers = GetNumOnlinePlayers();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool CheckTurn()
    {
        Debug.Log("Checking is my turn or not ");
        // Handle cases when by mistake more than once same information is conveyed
        Debug.Log("Number of online players is " + numberOnlinePlayers);
        if (numberOnlinePlayers > 1 && prevTeam == myTeam)
            return false;

        Debug.Log("Current team turn is " + currTeamTurn);
        Debug.Log("my team turn is " + myTeam);
            
        if (myTeam == currTeamTurn && !running)
        {
            return true;
            // Debug.Log("Starting turn");
            // StartTurn();
        }

        return false;
    }

    public void StartTurn()
    {
        running = true;
        isMyTurn = true;
    }

    public void EndTurn()
    {
        Debug.Log("ending my turn");
        // Check whether it was my turn or not
        if (!isMyTurn)
            return;
        
        // Setting running parameter to false
        isMyTurn = false;
        running = false;
        
        // Ask for updating the curr team;
        Debug.Log("updating turn");
        PV.RPC("RPC_UpdateTurn", RpcTarget.MasterClient);
    }

    public int GetNumOnlinePlayers()
    {
        if (PhotonNetwork.InRoom)
            return PhotonNetwork.CurrentRoom.PlayerCount;
        return -1;
    }

    // Only Master Client will be able to execute this 
    [PunRPC]
    void RPC_UpdateTurn()
    {
        // UpdateNumOnlinePlayers();
        // Debug.Log("number of online players is " + numberOnlinePlayers);
        int currTeam = (currTeamTurn + 1) % numberOnlinePlayers;
        PV.RPC("RPC_SetUpdatedTurn", RpcTarget.AllBuffered, currTeam);
    }

    // It will be executed for all;
    [PunRPC]
    void RPC_SetUpdatedTurn(int currTeam)
    {
        Debug.Log("Curr team is " + currTeamTurn);
        prevTeam = currTeamTurn;
        currTeamTurn = currTeam;

        // CheckTurn();
    }
}
