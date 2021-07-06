using System;
using Mirror;
using Services;
using UnityEngine;
using WhackAMole;

namespace Online.WhackAMole
{
    public class ResetWhackAMoleOnline : NetworkBehaviour
    {
        [SerializeField]
        private HammerSpawnerOnline[] hammerSpawners;

        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public override void OnStartClient()
        {
            hammerSpawners = FindObjectsOfType<HammerSpawnerOnline>();
        }

        [TargetRpc]
        public void TargetGetCharacterControllers(NetworkConnection target)
        {
            hammerSpawners = FindObjectsOfType<HammerSpawnerOnline>();
        }
        
        public void Reset()
        {
            if (!isLocalPlayer) return;
            
            foreach (var hammerSpawner in hammerSpawners)
            {
                hammerSpawner.enabled = false;
            }
        }
    }
}

