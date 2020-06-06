using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMetaData : MonoBehaviour
{
    public GameObject currCell;
    public GameObject homeCell;

    public int myTeam;
    public int myPawn;
    public int pawnNum;
    public int distanceFromHome;
}
