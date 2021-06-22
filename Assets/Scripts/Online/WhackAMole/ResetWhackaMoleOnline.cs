using System;
using Mirror;
using Services;
using UnityEngine;
using WhackAMole;

namespace Online.WhackAMole
{
    public class ResetWhackaMoleOnline : NetworkBehaviour
    {
        [SerializeField]
        private HammerSpawnerOnline[] hammerSpawners;
        
        public override void OnStartClient()
        {
            hammerSpawners = FindObjectsOfType<HammerSpawnerOnline>();
        }

        public void Reset()
        {
            foreach (var hammerSpawner in hammerSpawners)
            {
                hammerSpawner.enabled = false;
            }
        }
    }
}

