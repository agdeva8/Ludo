using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastCellMechanics
{

    private static GameObject player;
    private static GameObject lastCell;
    private static List<GameObject> currPlayers;
    private static CellMetaData cellMetaData;
    private static PlayerMetaData playerMetaData;
    
    public static void Main(GameObject _player, GameObject _lastCell)
    {
        player = _player;
        lastCell = _lastCell;

        cellMetaData = lastCell.GetComponent<CellMetaData>();
        currPlayers = cellMetaData.players;

        playerMetaData = player.GetComponent<PlayerMetaData>();
        playerMetaData.currCell = lastCell;

        // Handling cases where only need to be Added 
        if (cellMetaData.isStop || currPlayers.Count == 0)
        {
            cellMetaData.AddPlayer(player);
            return;
        }

        // Getting count of each player group
        // so as to decide whether to kick the player or not
        int[] playerGroupCount = new int[4];
        foreach (GameObject currPlayer in currPlayers)
            playerGroupCount[currPlayer.GetComponent<PlayerMetaData>().playerGroup]++;

        List<GameObject> defeatedPlayers = new List<GameObject>();
        foreach (GameObject currPlayer in currPlayers)
        {
            int currPlayerGroup = currPlayer.GetComponent<PlayerMetaData>().playerGroup;

            if (playerMetaData.playerGroup == currPlayerGroup)
                continue;
            if (playerGroupCount[currPlayerGroup] % 2 == 0)
                continue;
            
            playerGroupCount[currPlayerGroup]--;

            defeatedPlayers.Add(currPlayer);
        }
        
        foreach (GameObject defeatedPlayer in defeatedPlayers)
        {
            PushBack2Home(defeatedPlayer);
        }
        
        cellMetaData.AddPlayer(player);
    }

    public static void PushBack2Home(GameObject defeatedPlayer)
    {
        cellMetaData.RemovePlayer(defeatedPlayer);
        GameObject homeCell = defeatedPlayer.GetComponent<PlayerMetaData>().homeCell;
        homeCell.GetComponent<CellMetaData>().AddPlayer(defeatedPlayer);
    }

}

