using System.Collections;
using System.Collections.Generic;
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
public static class GameObjects
{
    // TODO: change name from DiceNUm to DiceNum
    // DiceNum GameObject
    public static readonly GameObject DiceNum = GameObject.Find("DiceNUm");
    // Main Mesh of the DiceNum object
    public static TextMeshProUGUI DiceScore =
        DiceNum.GetComponent<TextMeshProUGUI>();
    
    // Dice Game Object
    public static readonly GameObject Dice = GameObject.Find("Dice"); 
    // Dice Rotation Variable 
    public static Transform DiceTransform = Dice.transform;
    // Dice RigidBody
    public static Rigidbody DiceRb = Dice.GetComponent<Rigidbody>();
    
    // MonoBehaviour Object
    // Empty GameObject for MonoBehaviour
    // public static readonly GameObject MonoB = GameObject.Find("MonoB");
    // public static MonoBehaviour MB = MonoB.AddComponent<MonoBehaviour>();
    public static MonoBehaviour MB;
}
