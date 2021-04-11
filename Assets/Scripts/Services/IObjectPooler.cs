using UnityEngine;

public interface IObjectPooler
{
    GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation);
    void InstanciatePools();
    void DisableObjectInPool(string tagPool);
}