using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class ObjectPoolerService : IObjectPooler
    {
        private List<ObjectPool> pools;
        private readonly Dictionary<string, Queue<GameObject>> poolDictionary;

        public ObjectPoolerService(List<ObjectPool> pools)
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            this.pools = new List<ObjectPool>();
            this.pools.AddRange(pools);
        }

        public void InstanciatePools()
        {
            poolDictionary.Clear();

            foreach (ObjectPool pool in pools)
            {
                Queue<GameObject> objectPoolQueue = new Queue<GameObject>();

                for (int i = 0; i < pool.Size; i++)
                {
                    GameObject objectInstantiated = GameObject.Instantiate(pool.Prefab);
                    objectInstantiated.SetActive(false);

                    objectInstantiated.name += i;

                    objectPoolQueue.Enqueue(objectInstantiated);
                }

                poolDictionary.Add(pool.Tag, objectPoolQueue);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public void DisableObjectsInPool(string poolTag)
        {
            Queue<GameObject> poolQueue = poolDictionary[poolTag];

            foreach (GameObject objectPool in poolQueue)
            {
                objectPool.SetActive(false);
            }
        }

        public void DisableObject(string objectName, string poolTag)
        {
            Queue<GameObject> poolQueue = poolDictionary[poolTag];

            foreach (GameObject objectPool in poolQueue)
            {
                if (objectPool.name == objectName)
                {
                    objectPool.SetActive(false);
                }
            }
        }
    }
}