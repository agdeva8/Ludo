using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CellMetaData : MonoBehaviour
{
    // GameObj of the next cell (Direct Game Objects pointers and no ids) 
    // private GameObject nextObj = null;
    public GameObject nextObjSamePlayer;
    public GameObject nextObjOtherPlayer;
    public Vector3 fullScale = new Vector3(84, 84,84);
    // GameObj ID of previous cell 
    public GameObject prevObj;

    // Current Player Rect Portion
    public int playerPortion = 0;

    // Check if Curr Cell is Stop
    public bool isStop;

    [FormerlySerializedAs("Players")] public List<GameObject> players;

    public void SetNextGameObj(GameObject otherPlayer, GameObject samePlayer)
    {
        nextObjOtherPlayer = otherPlayer;
        nextObjSamePlayer = samePlayer;
    }

    public void SetNextGameObj(GameObject otherPlayer)
    {
        SetNextGameObj(otherPlayer, otherPlayer);
    }

    // Setting Next GameObject along with previous
    public void SetNextPrevGameObj(GameObject otherPlayer, GameObject samePlayer)
    {
        nextObjOtherPlayer = otherPlayer;
        nextObjSamePlayer = samePlayer;
        otherPlayer.GetComponent<CellMetaData>().prevObj = this.gameObject;
        samePlayer.GetComponent<CellMetaData>().prevObj = this.gameObject;
    }

    public void SetNextPrevGameObj(GameObject otherPlayer)
    {
        SetNextPrevGameObj(otherPlayer, otherPlayer);
    }


    public GameObject GetNextGameObj(int player)
    {
        return player == playerPortion ? nextObjSamePlayer : nextObjOtherPlayer;
    }

    public GameObject GetNextGameObj()
    {
        return nextObjOtherPlayer;
    }

    // TODO 
    // Add power variables at later stages 

    public void AddPlayer(GameObject player)
    {
        player.transform.parent = gameObject.transform;
        players.Add(player);
        // player.transform.localPosition = new Vector3(0, 0.5f, 0); 
        player.transform.rotation = gameObject.transform.rotation;
        player.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        
        RecalculateScalesPos();
    }

    public void RemovePlayer(GameObject player)
    {
        players.Remove(player);
        
        RecalculateScalesPos();
    }

    public void RecalculateScalesPos()
    {
        // z is the height of peice
        int numPlayers = players.Count;
        
        if (numPlayers == 0)
            return;
        
        Vector3 newScale = new Vector3(84, 84, 84);
        Vector2 rc = FindRc(numPlayers);
        int r = (int) rc.x;
        int c = (int) rc.y;

        // Debug.Log($"Num players is {numPlayers}");
        // Debug.Log(message: $"r, c is {r} , {c}");

        switch (numPlayers)
        {
            default:
                newScale /= Mathf.Max(r, c);
                // newScale.y /= Mathf.Min(r, c);
                break;
        }

        float rUnitTranslate = 1.0f / (2 * r);
        float cUnitTranslate = 1.0f / (2 * c);

        List<int> rPostionIndices = PositionIndices(r);
        List<int> cPositionIndices = PositionIndices(c);

        // for (int i = 0; i < rPostionIndices.Count; i++)
        //     Debug.Log($"RPos Indices is {rPostionIndices[i]}");

        int playerIdx = 0;

        bool done = false;
        
        // Debug.Log($"rtranslate {rUnitTranslate} , cTranslate {cUnitTranslate}");
        
        foreach (int rPIndex in rPostionIndices)
        {
            if (done)
                break;
            foreach (int cPIndex in cPositionIndices)
            {
                if (playerIdx == numPlayers)
                {
                    done = true;
                    break;
                }

                // Debug.Log(message: $"rIndex {rPIndex} , cIndex {cPIndex}");
                GameObject player = players[playerIdx];
                // Scaling Players accordingly
                players[playerIdx].transform.localScale = newScale;

                // translating players
                players[playerIdx].transform.localPosition = new Vector3( rPIndex * rUnitTranslate, 0.5f + 0.02f, 
                                                cPIndex * cUnitTranslate);
                // Players[PlayerIdx].transform.Translate( rPIndex * rUnitTranslate,
                //                                 cPIndex * cUnitTranslate, 0);

                playerIdx++;
            }
        }
    }

    // It converts a number(n) into row, col pair
    // such that r*c >= n
    private Vector2 FindRc(int n)
    {
        int r = 1;
        int c = 1;

        if (n >= 1 && n <= 2)
            c = 1;
        else if (n >= 3 && n <= 8)
            c = 2;
        else if (n >= 9 && n <= 15)
            c = 3;
        else if (n >= 16 && n <= 20)
            c = 4;

        r = (int) Math.Ceiling((double) n / (double) c);
        
        return new Vector2(r, c);
    }
    
    // Convert given r or c value to a list 
    // for example for given value: 4
    // the list is -3, -1,  1, 3
    // Using This, we can translate the player piece by multiplying the number with scaling value

    private List<int> PositionIndices(int val)
    {
        // ans is final list to return
        // a and b are temporary list
        List<int> ans = new List<int>();
        List<int> a = new List<int>();
        List<int> b = new List<int>();

        int startVal = 1;
        if (val % 2 != 0)
            startVal = 2;
        
        for (int i = 0; i < val / 2; i++)
        {
            a.Add(startVal);
            startVal += 2;
        }

        for (int i = a.Count - 1; i >= 0; i--)
            b.Add(-1 * a[i]);

        if (val % 2 != 0)
            b.Add(0);
        
        ans.AddRange(b);
        ans.AddRange(a);

        return ans;
    }
    
}
