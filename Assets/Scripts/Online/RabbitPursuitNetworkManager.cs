using System.Collections.Generic;
using Mirror;
using Online.BinkyPursuit;
using Online.WhackAMole;
using UnityEngine;

namespace Online
{
    public class RabbitPursuitNetworkManager : NetworkManager
    {
        [Header("Custom variables")]
        [SerializeField] private TimerUIOnline timerUI;
        [SerializeField] private PlayerIndicatorUI playerIndicatorUI;

        //[SerializeField] private GameObject[] enemySpawners;

        private List<GameObject> clients;
        private GameObject spawner;

        public Dictionary<int, NetworkConnection> PlayersConnections { get; private set; }

        public override void Awake()
        {
            base.Awake();
            clients = new List<GameObject>();
            PlayersConnections = new Dictionary<int, NetworkConnection>();
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if (numPlayers == 0)
            {
                GameObject player = Instantiate(playerPrefab, Vector3.zero + new Vector3(-2,0,0), Quaternion.identity);
                player.GetComponent<PlayerMovementOnline>().PlayerNumber = numPlayers + 1;
                clients.Add(player);
                //NetworkServer.AddPlayerForConnection(conn, player);
            }
            else if (numPlayers == 1)
            {
                GameObject player = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Player2"), Vector3.zero + new Vector3(2,0,0), Quaternion.identity);
                player.GetComponent<PlayerMovementOnline>().PlayerNumber = numPlayers + 1;
                clients.Add(player);
                //NetworkServer.AddPlayerForConnection(conn, player);
            }
            
            PlayersConnections.Add(numPlayers + 1, conn);

            
            int i = 0;
            
            foreach (var playerConn in PlayersConnections)
            {
                GameObject player = clients[i];
                NetworkServer.AddPlayerForConnection(playerConn.Value, player);
                i++;
            }
            
            foreach (var playerConn in PlayersConnections)
            {
                playerIndicatorUI.StartAnimationIndicator(playerConn.Value, playerConn.Key);
            }
            
            timerUI.InitializeTimer();
            StartCoroutine(timerUI.StartTimer());
            CreateSpawner();
            
        }

        private void OnEnable()
        {
            timerUI.OnTimerEnd += TimerEnded;
        }


        private void OnDisable()
        {
            timerUI.OnTimerEnd -= TimerEnded;
        }
        
        private void TimerEnded()
        {
            Debug.Log("Server recieved OnTimerEnd event!!");
            UnSpawnPlayers();
            UnSpawnSpawner();
        }

        private void CreateSpawner()
        {
            spawner = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "EnemySpawner"));
            NetworkServer.Spawn(spawner);
        }

        private void UnSpawnPlayers()
        {
            foreach (var player in clients)
            {
                NetworkServer.UnSpawn(player);
            }
        }

        private void UnSpawnSpawner()
        {

            NetworkServer.UnSpawn(spawner);

        }
    }
}
