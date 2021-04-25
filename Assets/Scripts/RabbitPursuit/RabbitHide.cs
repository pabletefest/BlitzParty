using System;
using System.Collections;
using UnityEngine;
using Services;
using UnityEngine.SceneManagement;

public class RabbitHide : MonoBehaviour
{
    public static event Action<GameObject> OnEnemyHidden;
    private IObjectPooler objectPooler;
    private bool isSpawnHole;

    private void Awake()
    {
        objectPooler = ServiceLocator.Instance.GetService<IObjectPooler>();
        isSpawnHole = true;
    }

    private void OnEnable()
    {
        StartCoroutine(DisableColliderSpawnHole());        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hole") && !isSpawnHole)
        {
            OnEnemyHidden?.Invoke(gameObject);
            objectPooler.DisableObject(gameObject.name, SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator DisableColliderSpawnHole()
    {
        isSpawnHole = true;

        yield return new WaitForSeconds(1f);

        isSpawnHole = false;
    }
}
