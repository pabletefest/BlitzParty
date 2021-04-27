using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnerGoldMole : MonoBehaviour
{
    private readonly string POOL_GOLDMOLE = "Whack-a-mole Golden Mole";
    public static event Action<GameObject> OnEnemySpawn;

    [SerializeField]
    private GameObject[] spawnPoints;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnTime = 5f;
    
    [SerializeField]
    private float decreasingRate = 0.2f;

    private CheckHoleAvailability holeAvailability;

    private float time;
    private const float minimumSpawnTime = 2f;

    private ITimer chronometerService;

    private IObjectPooler objectPoolerService;

    
    private void OnEnable()
    {
        //MainMenu.OnRabbitPursuitLoaded += SceneLoaded;
        ResetWhackaMole.OnSceneRestarted += SceneRestarted;
    }


    private void OnDisable()
    {
        //MainMenu.OnRabbitPursuitLoaded -= SceneLoaded;
        ResetWhackaMole.OnSceneRestarted -= SceneRestarted;
    }
    
    /*
    private void SceneLoaded()
    {
        objectPoolerService.InstanciatePools();
        SpawnEnemy(1); //Initial spawn
    }
    */

    private void SceneRestarted()
    {
        objectPoolerService.DisableObjectsInPool(POOL_GOLDMOLE);
    }

    private void Awake()
    {
        chronometerService = ServiceLocator.Instance.GetService<ITimer>();
        objectPoolerService = ServiceLocator.Instance.GetService<IObjectPooler>();
        time = spawnTime;
    }

    private void Start()
    {
        //objectPoolerService.RemovePoolFromDictionary(SceneManager.GetActiveScene().name);
        objectPoolerService.InstanciatePool(POOL_GOLDMOLE);
        holeAvailability = CheckHoleAvailability.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            if (spawnTime > minimumSpawnTime)
            {
                spawnTime -= decreasingRate;
            }

            time = spawnTime;

            //float totalChronometerTime = chronometerService.GetTotalTime();
            //float chronometerTime = chronometerService.GetCurrentTime();

            SpawnEnemy(1);
        }
    }

    private void SpawnEnemy(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomSpot = UnityEngine.Random.Range(0, spawnPoints.Length);
            if (!holeAvailability.allOccupied())
            {
                while (holeAvailability.isOccupied(randomSpot))
                {
                    randomSpot = UnityEngine.Random.Range(0, spawnPoints.Length);
                    Debug.Log("GoldMole " + randomSpot + " " + holeAvailability.isOccupied(randomSpot));
                }
            }
            //GameObject enemy = Instantiate(enemyPrefab, spawnPoints[randomSpot].transform.position, Quaternion.identity);
            holeAvailability.occupyHole(randomSpot);
            GameObject enemy = objectPoolerService.SpawnFromPool(POOL_GOLDMOLE, spawnPoints[randomSpot].transform.position, Quaternion.identity);
            //enemy.GetComponent<Animator>().SetTrigger("GoldenMoleRestart");
            //OnEnemySpawn?.Invoke(enemy);
            StartCoroutine(LiberateHole(randomSpot));
        }
    }

    private IEnumerator LiberateHole(int holeNumber)
    {
        yield return new WaitForSeconds(2f);
        holeAvailability.liberateHole(holeNumber);
    }
}
