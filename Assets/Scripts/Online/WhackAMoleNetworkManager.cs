using System;
using System.Collections.Generic;
using Mirror;
using Online.WhackAMole;
using UnityEngine;

namespace Online
{
    public class WhackAMoleNetworkManager : NetworkManager
    {
        [Header("Custom variables")]
        [SerializeField] private TimerUIOnline timerUI;
        [SerializeField] private PlayerIndicatorUI playerIndicatorUI;

        //[SerializeField] private GameObject[] enemySpawners;

        private List<GameObject> clients;
        private List<GameObject> spawners;

        public Dictionary<int, NetworkConnection> playersConnections { get; private set; }

        public override void Awake()
        {
            base.Awake();
            clients = new List<GameObject>();
            spawners = new List<GameObject>();
            playersConnections = new Dictionary<int, NetworkConnection>();
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<HammerSpawnerOnline>().PlayerNumber = numPlayers + 1;
            clients.Add(player);

            NetworkServer.AddPlayerForConnection(conn, player);
            
            playersConnections.Add(numPlayers, conn);

            if (numPlayers == 2)
            {
                foreach (var playerConn in playersConnections)
                {
                    playerIndicatorUI.StartAnimationIndicator(playerConn.Value, playerConn.Key);
                }
                
                timerUI.InitializeTimer();
                StartCoroutine(timerUI.StartTimer());
                CreateSpawners();
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
            DestroyRemainingHammers();
            UnSpawnPlayers();
            UnSpawnSpawners();
        }

        private void CreateSpawners()
        {
            GameObject goldMole = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "EnemySpawnerGoldenMole"));
            NetworkServer.Spawn(goldMole);
            spawners.Add(goldMole);
            
            GameObject mole = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "EnemySpawnerMole"));
            NetworkServer.Spawn(mole);
            spawners.Add(mole);
            
            GameObject zoomy = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "EnemySpawnerZoomy"));
            NetworkServer.Spawn(zoomy);
            spawners.Add(zoomy);
        }

        private void UnSpawnPlayers()
        {
            foreach (var player in clients)
            {
                NetworkServer.UnSpawn(player);
            }
        }

        private void UnSpawnSpawners()
        {
            foreach (var spawner in spawners)
            {   
                NetworkServer.UnSpawn(spawner);
            }
        }
    
        private void DestroyRemainingHammers()
        {
            GameObject[] hammers = GameObject.FindGameObjectsWithTag("Hammer");

            foreach (var hammer in hammers)
            {
                NetworkServer.UnSpawn(hammer);
            }

            //hammerSpawner.enabled = false;
        }
    }
}
