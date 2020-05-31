/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager GM;
        public GameObject winnerUI;

        public int nextPlayerTeam;
        
        [SerializeField] private GameObjects gameObjs;

        public MyArray[] homeCells;
        public GameObject[] players;

        [NonSerialized] public int groupNum;

        private int numOnlinePlayers;
        private int[] pawnNums;

        // Start Method
        void Start()
        {
            // initializing GM;
            if (GM == null)
                GM = this;
            
            if (!PhotonNetwork.IsConnected) // 1
            {
                SceneManager.LoadScene("Launcher");
                return;
            }
            
            players = new GameObject[4];

            Debug.Log("Instantiating Player");

            // Instantiating four pieces
            // One per pawn;
            for (int i = 0; i < 4; i++)
                players[i] = InstPlayerPiece();

            if (PhotonNetwork.IsMasterClient) // 2
            {

                // players[0, 0] = InstPlayerPiece(homeCells[0].objects[0]);
                // InstPlayerPiece(homeCells[0].objects[0]);
                // _player = InstPlayerPiece();
                // _homeCell = GameObject.Find("StartHomeCell00");
            }
            else // 5
            {
                // Debug.Log("instantiating player 2");
                // players[1, 0] = InstPlayerPiece(homeCells[1].objects[0]);
                // InstPlayerPiece(homeCells[1].objects[0]);
                // _player = InstPlayerPiece();
                // _homeCell = GameObject.Find("StartHomeCell10");
            }
            // _player.GetComponent<CellMetaData>().AddPlayer(_player);

            // Starting the game
                
            Application.targetFrameRate = 30;
            // StartCoroutine(RollDice.Routine());
        }

        GameObject InstPlayerPiece()
        {
            Debug.Log("instantiating player");
            GameObject player;
            player = PhotonNetwork.Instantiate("PlayerPeice", Vector3.zero, Quaternion.identity, 0);

            // PlayerMetaData playerMetaData = player.GetComponent<PlayerMetaData>();
            //
            // Debug.Log($"player group num is {groupNum}");
            // playerMetaData.homeCell = homeCells[groupNum].objects[0];
            return player;
        }
      
        // Update Method
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //1
            {
                Application.Quit();
            }
        }

        // Photon Methods
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Launcher");
            }
        }


        //Helper Methods
        public void QuitRoom()
        {
            Application.Quit();
        }

        public void UpdateNextPlayerTeam()
        {
            nextPlayerTeam++;
        }

        public int GetPawn(int teamNum)
        {
            if (numOnlinePlayers == 0)
                numOnlinePlayers = TurnManager.TM.GetNumOnlinePlayers();
            
            if (pawnNums == null)
                pawnNums = new int[numOnlinePlayers];
            
            if (teamNum < 0 || teamNum >= numOnlinePlayers)
            {
                Debug.LogError("GM: GetPawn: Team number is sent incorrect " + teamNum);
                return - 1;
            }

            pawnNums[teamNum]++;
            return pawnNums[teamNum] - 1;
        }
    }
}
