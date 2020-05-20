using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// It will contain All GameObjects and many of its main components used in various scripts
/// GameObjects will be readonly
/// Components will be read and write
///
/// GameObjects will be declared firs
/// All components will be declared just below the GameObjects
/// </summary>
public class GameObjects : MonoBehaviour
{
    // TODO: change name from DiceNUm to DiceNum
    // DiceNum GameObject
    [FormerlySerializedAs("DiceNum")] public GameObject diceNum;
    // Main Mesh of the DiceNum object
    [FormerlySerializedAs("DiceScore")] public TextMeshProUGUI diceScore;
        // DiceNum.GetComponent<TextMeshProUGUI>();
    
    // Dice Game Object
    [FormerlySerializedAs("Dice")] public GameObject dice;
    [FormerlySerializedAs("DiceTransform")] public Transform diceTransform;
    [FormerlySerializedAs("DiceRb")] public Rigidbody diceRb;
         
    // MonoBehaviour Object
    // Empty GameObject for MonoBehaviour
    // public static readonly GameObject MonoB = GameObject.Find("MonoB");
    // public static MonoBehaviour MB = MonoB.AddComponent<MonoBehaviour>();
    [FormerlySerializedAs("MB")] public MonoBehaviour mb;
    
    // Player to check if the code is running 
    [FormerlySerializedAs("DebugPlayer")] public GameObject debugPlayer;
    
    [FormerlySerializedAs("DebugPlayer2")] public GameObject debugPlayer2;
    
    // Player Pieces Array
    // public GameObject[,] Players = new GameObject[8,4];

    [FormerlySerializedAs("NumPlayers")] public int numPlayers;
    [FormerlySerializedAs("Players")] public GameObject[,] players;

    [FormerlySerializedAs("HomeOut")] public GameObject[] homeOut;
    
    void Start()
    {
        players = new GameObject[numPlayers,4];

        debugPlayer2 = GameObject.Find("Player10");
        
        if (debugPlayer2 == null)
            Debug.Log("Cant find Player");
        
        // Assigning Player Variables using Find
        for (int i = 0; i < numPlayers; i++)
            for (int j = 0; j < 4; j++)
                players[i, j] = GameObject.Find($"Player{i}{j}"); 
        
    }
    // public static void AssignPlayers()
    // {
    //     for (int i = 0; i < NumPlayers; i++)
    //         for (int j = 0; j < 4; j++)
    //             Players[i, j] = GameObject.Find($"Player{i}{j}"); 
    // }
}
