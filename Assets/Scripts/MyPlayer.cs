using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{

    public PhotonView photonView;
    public PlayerMetaData playerMetaData;

    private int numMovesR;
    private int numMovesW;

    private int distanceFromHome;

    public GameObject hc1;
    public GameObject hc2;

    void Start()
    {
        GameObject homeCell;
        hc1 = GameObject.Find("StartHomeCell00");
        hc2 = GameObject.Find("StartHomeCell10");
        
        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
                homeCell = hc1;
            else
                homeCell = hc2;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                homeCell = hc2;
            else
                homeCell = hc1;
        }

        Debug.Log("Adding player to home cell");
        homeCell.GetComponent<CellMetaData>().AddPlayer(gameObject);
        playerMetaData.homeCell = homeCell;
        playerMetaData.currCell = homeCell;
    }

    private void OnMouseDown()
    {
        if (photonView.IsMine)
        {
            numMovesW = 1;
            distanceFromHome += numMovesW;
            // playerMetaData.distanceFromHome = distanceFromHome;
            MoveRoutine();
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInputs();
        }
        else {
            smoothMovement();
        }
    }

    private void smoothMovement()
    {
        if (numMovesR > 0)
            MoveRoutine();
        numMovesR = 0;

        correctPlayerPos();
    }

    private void correctPlayerPos()
    {
        if (playerMetaData.distanceFromHome == distanceFromHome)
            return;

        GameObject currCell = playerMetaData.currCell;
        GameObject newCell = currCell;
        while (playerMetaData.distanceFromHome > distanceFromHome)
        {
            newCell = newCell.GetComponent<CellMetaData>().GetPrevGameObj();
            playerMetaData.distanceFromHome--;
        }
        while (playerMetaData.distanceFromHome < distanceFromHome)
        {
            newCell = newCell.GetComponent<CellMetaData>().GetNextGameObj();
            playerMetaData.distanceFromHome++;
        }
       
        currCell.GetComponent<CellMetaData>().RemovePlayer(gameObject);
        newCell.GetComponent<CellMetaData>().AddPlayer(gameObject);
    }

    private void ProcessInputs()
    {
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info )	
    {
        Debug.Log("on photon serialization");
        if (stream.IsWriting)
        {
            Debug.Log("writing to stream" + numMovesW);
            stream.SendNext(numMovesW);
            stream.SendNext(distanceFromHome);
            numMovesW = 0;
            // stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            numMovesR = (int) stream.ReceiveNext();
            distanceFromHome = (int) stream.ReceiveNext();
            Debug.Log("Reading from stream" + numMovesR);
            // smoothMove = (Vector3) stream.ReceiveNext();
        }
    }
    
    public void MoveRoutine()
    {
        GameObject currCell = playerMetaData.currCell;
        GameObject nextCell = currCell.GetComponent<CellMetaData>().GetNextGameObj(playerMetaData.playerGroup);

        if (currCell != null)
            currCell.GetComponent<CellMetaData>().RemovePlayer(gameObject);
        
        if (nextCell != null)
            nextCell.GetComponent<CellMetaData>().AddPlayer(gameObject);

        playerMetaData.distanceFromHome += 1;
    }
}
