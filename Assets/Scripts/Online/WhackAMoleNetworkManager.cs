using System;
using Mirror;
using Online.WhackAMole;
using UnityEngine;

namespace Online
{
    public class WhackAMoleNetworkManager : NetworkManager
    {
        [Header("Custom variables")]
        [SerializeField] private TimerUIOnline timerUI;

        [SerializeField] private GameObject[] enemySpawners;
    
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.GetComponent<HammerSpawnerOnline>().PlayerNumber = numPlayers + 1;
            NetworkServer.AddPlayerForConnection(conn, player);

            if (numPlayers == 2)
            {
                timerUI.InitializeTimer();
                StartCoroutine(timerUI.StartTimer());
                EnableSpawners();
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
        }

        private void EnableSpawners()
        {
            foreach (var spawner in enemySpawners)
            {
                spawner.SetActive(true);
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
