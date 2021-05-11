using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class CheckHoleAvailability : MonoBehaviour
    {
        
        private static CheckHoleAvailability instance;
        public static CheckHoleAvailability Instance => instance;
        private bool[] holeOccupied;
        
        [SerializeField] private GameObject[] spawnPoints;
        private Dictionary<GameObject, bool> holesOccupied;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        
            holesOccupied = new Dictionary<GameObject, bool>();
            foreach (var t in spawnPoints)
            {
                holesOccupied.Add(t, false);
            }
                
            holeOccupied = new bool[7];
        }
        
        public void OccupyHole(int holeNumber)
        {
            holeOccupied[holeNumber] = true;
        }
        
        public bool IsOccupied(int holeNumber)
        {
            return holeOccupied[holeNumber];
        }
        
        public void LiberateHole(int holeNumber)
        {
            holeOccupied[holeNumber] = false;
        }
        
        public bool AllOccupied()
        {
            bool allOccupied = true;
        
            for(int i = 0; i < holeOccupied.Length; i++)
            {
                if (!holeOccupied[i])
                {
                    allOccupied = false;
                }
            }
        
            return allOccupied;
        }
            
        public void OccupyHoleSpawn(GameObject spawnPoint)
        {
            holesOccupied[spawnPoint] = true;
        }
        
        public bool IsOccupiedSpawn(GameObject spawnPoint)
        {
            return holesOccupied[spawnPoint];
        }
        
        public void LiberateHoleSpawn(GameObject spawnPoint)
        {
            holesOccupied[spawnPoint] = false;
        }
        
        public bool AllOccupiedSpawn()
        {
            bool allOccupied = true;
        
            foreach (var t in holesOccupied)
            {
                if (!t.Value)
                {
                    allOccupied = false;
                }
            }
                
            return allOccupied;
        }
    }
}
