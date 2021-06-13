using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Online
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkDiscovery))]
    public class NetworkLobbyController : MonoBehaviour
    {
        private readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

        [SerializeField] private NetworkDiscovery networkDiscovery;
        private NetworkTypeChecker networkTypeChecker;

        [SerializeField] private GameObject multiplayerMessagePanel;
            
#if UNITY_EDITOR
        void OnValidate()
        {
            if (networkDiscovery == null)
            {
                networkDiscovery = GetComponent<NetworkDiscovery>();
                UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
                UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
            }
        }
#endif
        
        private void Awake()
        {
            networkTypeChecker = GameObject.Find("NetworkTypeChecker").GetComponent<NetworkTypeChecker>();
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartGameConnection());
        }
        

        private IEnumerator StartGameConnection()
        {
            switch (networkTypeChecker.NetType)
            {
                case NetworkType.LAN:
                    StartServerDiscovery();
                    
                    multiplayerMessagePanel.SetActive(true);
                    
                    yield return new WaitForSeconds(3f);
                    
                    if (discoveredServers.Count > 0)
                    {
                        long matchKey = discoveredServers.First().Key;
                        ServerResponse response = discoveredServers.First().Value;
                        StartClient(response);
                        discoveredServers.Remove(matchKey);
                    }
                    else
                    {
                        StartHost();
                    }
                    
                    yield return new WaitForSeconds(3f);
                    
                    // discoveredServers.Clear();
                    
                    break;
                
                case NetworkType.WAN:
                    break;
            }
        }

        private void StartServerDiscovery()
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }

        private void StartClient(ServerResponse serverInfo)
        {
            Connect(serverInfo);
        }

        private void StartHost()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        private void StartServer()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartServer();
            networkDiscovery.AdvertiseServer();
        }
        
        private void Connect(ServerResponse info)
        {
            NetworkManager.singleton.StartClient(info.uri);
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
        }
    }
}
