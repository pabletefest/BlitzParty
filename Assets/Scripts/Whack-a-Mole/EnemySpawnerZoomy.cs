using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnerZoomy : MonoBehaviour
{
    public static event Action<GameObject> OnEnemySpawn;

    [SerializeField]
    private GameObject[] spawnPoints;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnTime = 5f;
    
    [SerializeField]
    private float decreasingRate = 0.2f;

    private float time;
    private const float minimumSpawnTime = 2f;

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

    private void SceneRestarted(string activeScene)
    {
        objectPoolerService.DisableObjectsInPool(activeScene);
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
        objectPoolerService.InstanciatePools();
        holeAvailability = CheckHoleAvailability.Instance;
        SpawnEnemy(1); //Initial spawn
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

            float totalChronometerTime = chronometerService.GetTotalTime();
            float chronometerTime = chronometerService.GetCurrentTime();
            
            Debug.Log($"Total chronometer time: {totalChronometerTime}");
            Debug.Log($"Current chronometer time: {chronometerTime}");

            if (chronometerTime >= totalChronometerTime / 2)
            {
                int numberOfEnemies = 1;
                SpawnEnemy(numberOfEnemies);
            }
            else if (chronometerTime >= totalChronometerTime / 6)
            {
                int numberOfEnemies = 1;
                SpawnEnemy(numberOfEnemies);
            }
            else
            {
                int numberOfEnemies = UnityEngine.Random.Range(1,2);
                SpawnEnemy(numberOfEnemies);
            }
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
                    Debug.Log("Zoomy " + randomSpot + " " + holeAvailability.isOccupied(randomSpot));
                }
            }
            //GameObject enemy = Instantiate(enemyPrefab, spawnPoints[randomSpot].transform.position, Quaternion.identity);
            holeAvailability.occupyHole(randomSpot);
            GameObject enemy = objectPoolerService.SpawnFromPool("Whack-a-mole Zoomy", spawnPoints[randomSpot].transform.position, Quaternion.identity);
            //enemy.GetComponent<Animator>().SetTrigger("ZoomyRestart");
            OnEnemySpawn?.Invoke(enemy);
            StartCoroutine(liberateHole(randomSpot));
        }
    }

    private IEnumerator liberateHole(int holeNumber)
    {
        yield return new WaitForSeconds(2f);
        holeAvailability.liberateHole(holeNumber);
    }
}
