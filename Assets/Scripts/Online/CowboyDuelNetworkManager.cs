using System.Collections.Generic;
using Mirror;
using Online.CowboyDuel;
using UnityEngine;

namespace Online
{
	public class CowboyDuelNetworkManager : NetworkManager
	{
		[Header("Custom variables")]
		[SerializeField] private PlayerIndicatorUI playerIndicatorUI;
		[SerializeField] private PanelHandlerOnline panelHandler;
		[SerializeField] private GameFinisherOnline gameFinisher;
		
		private List<GameObject> clients;

		private int clientNumber;
		
		public Dictionary<int, NetworkConnection> PlayersConnections { get; private set; }
		
		public override void Awake()
		{
			base.Awake();
			clients = new List<GameObject>();
			PlayersConnections = new Dictionary<int, NetworkConnection>();
		}
		
		public override void OnServerAddPlayer(NetworkConnection conn)
		{
			if (clientNumber == 0)
			{
				GameObject player = Instantiate(playerPrefab, Vector3.zero + new Vector3(-2,-2.7f,0), Quaternion.identity);
				player.GetComponent<PlayerShootOnline>().playerNumber = ++clientNumber;
				player.name = $"Player {clientNumber}";
				clients.Add(player);
				PlayersConnections.Add(clientNumber, conn);
				//NetworkServer.AddPlayerForConnection(conn, player);
			}
			else if (clientNumber == 1)
			{
				GameObject player = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Player 2"), Vector3.zero + new Vector3(2,-2.7f,0), Quaternion.identity);
				player.GetComponent<PlayerShootOnline>().playerNumber = ++clientNumber;
				player.name = $"Player {clientNumber}";
				clients.Add(player);
				PlayersConnections.Add(clientNumber, conn);
				//NetworkServer.AddPlayerForConnection(conn, player);
			}

			if (clientNumber == 2)
			{
				int i = 0;

				foreach (var playerConn in PlayersConnections)
				{
					Debug.Log($"Client: {clients[i]}");
					GameObject player = clients[i];
					NetworkServer.AddPlayerForConnection(playerConn.Value, player);
					Debug.Log($"Player {player.GetComponent<PlayerShootOnline>().PlayerNumber} was given authority");
					i++;
				}
				
				panelHandler.RpcDisableWaitingPlayersPanel();
				
				foreach (var playerConn in PlayersConnections)
				{
					playerIndicatorUI.StartAnimationIndicator(playerConn.Value, playerConn.Key);
				}

				foreach (var player in clients)
				{
					player.GetComponent<PlayerShootOnline>().RpcEnablePlayerAnimator();
				}
				
				panelHandler.RpcActivateCowboyDuelVisualElements();
			}
		}
		
		private void OnEnable()
		{
			gameFinisher.OnGameEnd += GameEnded;
		}


		private void OnDisable()
		{
			gameFinisher.OnGameEnd -= GameEnded;
		}
        
		private void GameEnded()
		{
			Debug.Log("Server recieved OnTimerEnd event!!");
			//UnSpawnPlayers();
		}
		
		private void UnSpawnPlayers()
		{
			foreach (var player in clients)
			{
				NetworkServer.UnSpawn(player);
				Destroy(player);
			}
            
			clients.Clear();
		}
	}
}
