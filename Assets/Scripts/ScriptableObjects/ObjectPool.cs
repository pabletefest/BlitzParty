using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPool", menuName = "BlitzParty/ObjectPoolOS", order = 0)]
public class ObjectPool : ScriptableObject
{
    public string Tag;
    public GameObject Prefab;
    public int Size;
}