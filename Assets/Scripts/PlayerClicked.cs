using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerClicked : MonoBehaviour
{
    // Callback signature for on PlayerClicked
    public delegate void InformMoveScriptCallback(GameObject player);
    
    // Event Declaration
    public event InformMoveScriptCallback InformMoveScript;
    
    // Start is called before the first frame update
    void Start()
    {
        InformMoveScript += MovePlayer.PlayerToMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        InformMoveScript(this.gameObject);
    }
}
