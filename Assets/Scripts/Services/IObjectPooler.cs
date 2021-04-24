using UnityEngine;

namespace Services
{
    public interface IObjectPooler
    {
        GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation);
        void InstanciatePools();
        void DisableObjectsInPool(string tagPool);
        void DisableObject(string objectName, string poolTag);
        void ClearAllPools();
        void ClearPool(string poolTag);
        void RemovePoolFromDictionary(string poolTag);
    }
}