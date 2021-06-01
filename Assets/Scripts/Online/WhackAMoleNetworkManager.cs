using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Online.WhackAMole;

public class WhackAMoleNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<HammerSpawnerOnline>().PlayerNumber = numPlayers;
        NetworkServer.AddPlayerForConnection(conn, player);

        if (numPlayers == 2)
        {

        }
    }
}
