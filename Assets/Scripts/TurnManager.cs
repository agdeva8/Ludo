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
    
    private int prevTeam;
    private bool firstTurn;
    
    // Start is called before the first frame update
    void Start()
    {
        if (TM == null || TM != this)
            TM = this;

        prevTeam = -1;
        UpdateNumOnlinePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!firstTurn)
        // {
        //     Debug.Log("it is a first turn");
        //     firstTurn = true;
        //     UpdateNumOnlinePlayers();
        //     // CheckTurn();
        // }
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

    void UpdateNumOnlinePlayers()
    {
        if (PhotonNetwork.InRoom)
            numberOnlinePlayers = PhotonNetwork.CurrentRoom.PlayerCount;
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
