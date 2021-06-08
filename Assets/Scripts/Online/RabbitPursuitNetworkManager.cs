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
        [SerializeField] private PanelHandlerOnline panelHandler;

        private int clientNumber;

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
            if (clientNumber == 0)
            {
                GameObject player = Instantiate(playerPrefab, Vector3.zero + new Vector3(-2,0,0), Quaternion.identity);
                player.GetComponent<PlayerMovementOnline>().playerNumber = ++clientNumber;
                clients.Add(player);
                PlayersConnections.Add(clientNumber, conn);
                //NetworkServer.AddPlayerForConnection(conn, player);
            }
            else if (clientNumber == 1)
            {
                GameObject player = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Player2"), Vector3.zero + new Vector3(2,0,0), Quaternion.identity);
                player.GetComponent<PlayerMovementOnline>().playerNumber = ++clientNumber;
                clients.Add(player);
                PlayersConnections.Add(clientNumber, conn);
                //NetworkServer.AddPlayerForConnection(conn, player);
            }
            
            //Debug.Log($"Number of clients: {clients.Count}");

            if (clientNumber == 2)
            {
                int i = 0;

                foreach (var playerConn in PlayersConnections)
                {
                    Debug.Log($"Client: {clients[i]}");
                    GameObject player = clients[i];
                    NetworkServer.AddPlayerForConnection(playerConn.Value, player);
                    Debug.Log($"Player {player.GetComponent<PlayerMovementOnline>().PlayerNumber} was given authority");
                    i++;
                }
                panelHandler.RpcActivateBinkyPursuitVisualElements();
                
                foreach (var playerConn in PlayersConnections)
                {
                    panelHandler.RpcAnchorCatchButtonToPlayer(playerConn.Value);
                }
                
                foreach (var playerConn in PlayersConnections)
                {
                    playerIndicatorUI.StartAnimationIndicator(playerConn.Value, playerConn.Key);
                }
            
                timerUI.InitializeTimer();
                StartCoroutine(timerUI.StartTimer());
                CreateSpawner();
                
            }
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
            UnSpawnEnemies();
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
                //Debug.Log($"Unspawning player {player}");
                NetworkServer.UnSpawn(player);
                Destroy(player);
            }
            
            clients.Clear();
        }

        private void UnSpawnSpawner()
        {
            //Debug.Log($"Unspawning spawner {spawner}");
            //NetworkServer.UnSpawn(spawner);
            Destroy(spawner);
        }

        private void UnSpawnEnemies()
        {
            GameObject[] enemiesOnScene = GameObject.FindGameObjectsWithTag("Rabbit");

            foreach (var enemy in enemiesOnScene)
            {
                //Debug.Log($"Unspawning enemy {enemy}");
                NetworkServer.UnSpawn(enemy);
                Destroy(enemy);
            }
        }
    }
}
