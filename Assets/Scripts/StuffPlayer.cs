using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StuffPlayer 
{
    // [MenuItem("Tools/Stuff Player")]
    public static void Stuff()
    {
        var p0 = GameObject.Find("Player00");
        var p1 = GameObject.Find("Player01");
        var p2 = GameObject.Find("Player02"); 
        var p3 = GameObject.Find("Player03"); 
        var p4 = GameObject.Find("Player10"); 

        GameObject cell = GameObject.Find("demoCell");
        CellMetaData cellMetaData = cell.GetComponent<CellMetaData>();

        cellMetaData.players.Clear();
        cellMetaData.AddPlayer(p0);
        cellMetaData.AddPlayer(p1);
        cellMetaData.AddPlayer(p2);
        cellMetaData.AddPlayer(p3);
        cellMetaData.AddPlayer(p4);
    }
}
