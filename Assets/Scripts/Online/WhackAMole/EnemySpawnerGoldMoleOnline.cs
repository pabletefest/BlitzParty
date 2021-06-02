using System;
using System.Collections;
using Services;
using UnityEngine;
using WhackAMole;
using Mirror;

namespace Online.WhackAMole
{
    public class EnemySpawnerGoldMoleOnline : NetworkBehaviour
    {
        private readonly string POOL_GOLDMOLE = "Whack-a-mole Golden Mole";
        public static event Action<GameObject> OnEnemySpawn;

        [SerializeField] private GameObject[] spawnPoints;

        [SerializeField] private GameObject enemyPrefab;

        [SerializeField] private float initialSpawnTime = 5f;

        private float spawnTime;

        [SerializeField] private float decreasingRate = 0.2f;

        private CheckHoleAvailabilityOnline holeAvailability;

        private float time;
        private const float minimumSpawnTime = 2f;

        private ITimer chronometerService;

        private IObjectPooler objectPoolerService;


        private void OnEnable()
        {
            //MainMenu.OnRabbitPursuitLoaded += SceneLoaded;
            ResetWhackaMoleOnline.OnSceneRestarted += SceneRestarted;
        }


        private void OnDisable()
        {
            //MainMenu.OnRabbitPursuitLoaded -= SceneLoaded;
            ResetWhackaMoleOnline.OnSceneRestarted -= SceneRestarted;
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
            //objectPoolerService.DisableObjectsInPool(POOL_GOLDMOLE);
            RestartTimings();
        }

        private void Awake()
        {
            chronometerService = ServiceLocator.Instance.GetService<ITimer>();
            //objectPoolerService = ServiceLocator.Instance.GetService<IObjectPooler>();
            RestartTimings();
        }

        private void Start()
        {
            //objectPoolerService.RemovePoolFromDictionary(SceneManager.GetActiveScene().name);
            //objectPoolerService.InstanciatePool(POOL_GOLDMOLE);
            holeAvailability = CheckHoleAvailabilityOnline.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isServer) return;

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
                if (!holeAvailability.AllOccupiedSpawn())
                {
                    while (holeAvailability.IsOccupiedSpawn(spawnPoints[randomSpot]))
                    {
                        randomSpot = UnityEngine.Random.Range(0, spawnPoints.Length);
                        Debug.Log("GoldMole " + randomSpot + " " + holeAvailability.IsOccupied(randomSpot));
                    }

                    holeAvailability.OccupyHoleSpawn(spawnPoints[randomSpot]);
                    //GameObject enemy = objectPoolerService.SpawnFromPool(POOL_GOLDMOLE,
                    //spawnPoints[randomSpot].transform.position, Quaternion.identity);
                    GameObject enemy = Instantiate(enemyPrefab, spawnPoints[randomSpot].transform.position, Quaternion.identity);
                    NetworkServer.Spawn(enemy);
                    StartCoroutine(LiberateHole(spawnPoints, randomSpot));
                }
                //GameObject enemy = Instantiate(enemyPrefab, spawnPoints[randomSpot].transform.position, Quaternion.identity);

                //enemy.GetComponent<Animator>().SetTrigger("GoldenMoleRestart");
                //OnEnemySpawn?.Invoke(enemy);
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
}
