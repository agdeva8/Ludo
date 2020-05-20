using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StuffPlayer 
{
    [MenuItem("Tools/Stuff Player")]
    public static void Stuff()
    {
        var p0 = GameObject.Find("Player00");
        var p1 = GameObject.Find("Player10"); 

        GameObject dcell0 = GameObject.Find("dCell02");
        GameObject dcell1 = GameObject.Find("dCell12");
        
        dcell0.GetComponent<CellMetaData>().RemovePlayer(p0);
        dcell1.GetComponent<CellMetaData>().RemovePlayer(p1);
        
        dcell0.GetComponent<CellMetaData>().AddPlayer(p0);
        dcell1.GetComponent<CellMetaData>().AddPlayer(p1);
        
        ClassObjects.Gameobj.homeOut[0].GetComponent<CellMetaData>().RemovePlayer(p0);
        ClassObjects.Gameobj.homeOut[1].GetComponent<CellMetaData>().RemovePlayer(p1);
        
        // GameObject cell = GameObject.Find("demoCell");
        // CellMetaData cellMetaData = cell.GetComponent<CellMetaData>();
        //
        // cellMetaData.players.Clear();
        // cellMetaData.AddPlayer(p0);
        // cellMetaData.AddPlayer(p1);
        // cellMetaData.AddPlayer(p2);
        // cellMetaData.AddPlayer(p3);
        // cellMetaData.AddPlayer(p4);
    }
}
