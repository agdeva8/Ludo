using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

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
    public GameObject DiceNum;
    // Main Mesh of the DiceNum object
    public TextMeshProUGUI DiceScore;
        // DiceNum.GetComponent<TextMeshProUGUI>();
    
    // Dice Game Object
    public GameObject Dice;
    public Transform DiceTransform;
    public Rigidbody DiceRb;
         
    // MonoBehaviour Object
    // Empty GameObject for MonoBehaviour
    // public static readonly GameObject MonoB = GameObject.Find("MonoB");
    // public static MonoBehaviour MB = MonoB.AddComponent<MonoBehaviour>();
    public MonoBehaviour MB;
    
    // Player to check if the code is running 
    public GameObject DebugPlayer;
    
    public GameObject DebugPlayer2;
    
    // Player Pieces Array
    // public GameObject[,] Players = new GameObject[8,4];

    public int NumPlayers;
    public GameObject[,] Players;

    public GameObject[] HomeOut;
    
    void Start()
    {
        Players = new GameObject[NumPlayers,4];

        DebugPlayer2 = GameObject.Find("Player10");
        
        if (DebugPlayer2 == null)
            Debug.Log("Cant find Player");
        
        // Assigning Player Variables using Find
        for (int i = 0; i < NumPlayers; i++)
            for (int j = 0; j < 4; j++)
                Players[i, j] = GameObject.Find($"Player{i}{j}"); 
                
    }
    // public static void AssignPlayers()
    // {
    //     for (int i = 0; i < NumPlayers; i++)
    //         for (int j = 0; j < 4; j++)
    //             Players[i, j] = GameObject.Find($"Player{i}{j}"); 
    // }
}
