using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnerMole : MonoBehaviour
{
    private readonly string POOL_MOLE = "Whack-a-mole Normal Mole";
    public static event Action<GameObject> OnEnemySpawn;

    [SerializeField]
    private GameObject[] spawnPoints;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float initialSpawnTime = 5f;

    private float spawnTime;
    
    [SerializeField]
    private float decreasingRate = 0.2f;

    private float time;
    private const float minimumSpawnTime = 0.75f;

    private ITimer chronometerService;

    private IObjectPooler objectPoolerService;

    private CheckHoleAvailability holeAvailability;


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
        objectPoolerService.DisableObjectsInPool(POOL_MOLE);
        RestartTimings();
    }

    private void Awake()
    {
        chronometerService = ServiceLocator.Instance.GetService<ITimer>();
        objectPoolerService = ServiceLocator.Instance.GetService<IObjectPooler>();
        RestartTimings();
    }

    private void Start()
    {
       //objectPoolerService.RemovePoolFromDictionary(SceneManager.GetActiveScene().name);
        objectPoolerService.InstanciatePool(POOL_MOLE);
        holeAvailability = CheckHoleAvailability.Instance;
        //SpawnEnemy(1); //Initial spawn
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

            if (spawnTime < minimumSpawnTime) spawnTime = minimumSpawnTime;
            time = spawnTime;

            float totalChronometerTime = chronometerService.GetTotalTime();
            float chronometerTime = chronometerService.GetCurrentTime();
            

            if (chronometerTime >= totalChronometerTime / 2)
            {
                int numberOfEnemies = 1;
                SpawnEnemy(numberOfEnemies);
            }
            else if (chronometerTime >= totalChronometerTime / 6)
            {
                int numberOfEnemies = UnityEngine.Random.Range(1,2);
                SpawnEnemy(numberOfEnemies);
            }
            else
            {
                int numberOfEnemies = UnityEngine.Random.Range(1,3);
                SpawnEnemy(numberOfEnemies);
            }
        }
    }

    private void SpawnEnemy(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomSpot = UnityEngine.Random.Range(0, spawnPoints.Length);
            if (!holeAvailability.AllOccupiedSpawn())
            {
                while (holeAvailability.IsOccupiedSpawn(spawnPoints[randomSpot]))
                {
                    randomSpot = UnityEngine.Random.Range(0, spawnPoints.Length);
                    Debug.Log("Mole " + randomSpot + " " + holeAvailability.IsOccupied(randomSpot));
                }
            }
            //GameObject enemy = Instantiate(enemyPrefab, spawnPoints[randomSpot].transform.position, Quaternion.identity);
            holeAvailability.OccupyHoleSpawn(spawnPoints[randomSpot]);
            GameObject enemy = objectPoolerService.SpawnFromPool(POOL_MOLE, spawnPoints[randomSpot].transform.position, Quaternion.identity);
            //enemy.GetComponent<Animator>().SetTrigger("MoleRestart");
            //OnEnemySpawn?.Invoke(enemy);
            StartCoroutine(LiberateHole(spawnPoints, randomSpot)); 
        }
    }

    private IEnumerator LiberateHole(GameObject[] spawnPoints, int randomSpot)
    {
        yield return new WaitForSeconds(2f);
        holeAvailability.LiberateHoleSpawn(spawnPoints[randomSpot]);
    }
    private void RestartTimings()
    {
        spawnTime = initialSpawnTime;
        time = spawnTime;
    }
}
