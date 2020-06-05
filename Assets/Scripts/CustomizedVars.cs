using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomizedVars : MonoBehaviour
{
    public static CustomizedVars CV;
    
    public List<Color> playerColors = new List<Color>() {
        // Parrot Green 
        new Color(0.13f, 1f, 0.26f, 0.9f),
        
        // Sky Blue
        new Color(0.35f, 0.32f, 1f),
        // Yellow 
        new Color(1f, 0.77f, 0.13f),
        
        // Blood Red 
        new Color(1f, 0.17f, 0.18f),
        
        // Sky Blue
        new Color(0.43f, 0.99f, 1f),
        // Yellow 
        new Color(1f, 0.99f, 0.59f),
        
        // Blood Red 
        new Color(1f, 0.35f, 0.35f),
        
        // Parrot Green 
        new Color(0.51f, 1f, 0.41f),
        
        // Baby pink 
        new Color(1f, 0.74f, 0.99f),
        
        // Purple 
        new Color(0.63f, 0.31f, 1f),
       
        // Violet
        new Color(0.38f, 0.44f, 1f),
        
        // Grey 
        new Color(0.67f, 0.67f, 0.67f)
    };
    
    void Start()
    {
        if (CV == null || CV != this)
            CV = this;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
