using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject boss;

    [SerializeField, Range(1, 10)] private int wavesPerBoss = 4;
    [SerializeField, Range(0.001f, 10)] private float defaultSpawnRate = 1;
    [SerializeField] private int defaultMaxEnemiesPerWave;
    [SerializeField] private int enemiesIncrementPerWave;

    [HideInInspector] public int maxEnemiesPerWave;
    [HideInInspector] public float spawnRate;
    private int spawnedEnemies = 0;
    private int killedEnemies = 0;
    private int currentWave = 1;
    private bool isBossSpawned = false;

    private Transform player;
    private ObjectPool pool;
    private GameObject slowed;
    private bool isSlowed = false;
    private float slowedDuration = 0f;



    public void ResetStats()
    {
        spawnRate = defaultSpawnRate;
        maxEnemiesPerWave = defaultMaxEnemiesPerWave;
        spawnedEnemies = 0;
        killedEnemies = 0;
    }


    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialID", 0) >= 99)
        {
            spawnRate = defaultSpawnRate;
            maxEnemiesPerWave = defaultMaxEnemiesPerWave;
            StartCoroutine(SpawnEnemyMechanics());
        }


        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        slowed = GameObject.Find("Slowed");
        slowed.SetActive(false);

        StartCoroutine(SlowedRoutine());
    }


    public void StartCo()
    {
        StartCoroutine(SpawnEnemyMechanics());
    }


    private IEnumerator SpawnEnemyMechanics()
    {
        while (true)
        {
            //Debug.Log("spanwing" + spawnRate);

            yield return new WaitForSeconds(spawnRate);

            if (currentWave % wavesPerBoss == 0)
            {
                if (!isBossSpawned) SpawnBoss();
            }
            else if (spawnedEnemies < maxEnemiesPerWave)
            {
                GameObject enemy;
                Vector2 spawnLoc = Utility.RandomVector2D();
                float randNum = Random.Range(0.0f, 1.0f);

                while (true)
                {
                    if (Vector2.Distance(spawnLoc, player.position) > 5f) break;
                    else spawnLoc = Utility.RandomVector2D();
                }

                if (randNum < .4f)
                    enemy = pool.SpawnObject("BlueSlimeEnemy");

                else if (randNum < .6f)
                    enemy = pool.SpawnObject("OrangeSlimeEnemy");

                else if (randNum < .8f)
                    enemy = pool.SpawnObject("GreenSlimeEnemy");

                else
                    enemy = pool.SpawnObject("CobraEnemy");


                enemy.transform.position = spawnLoc;
                enemy.GetComponent<EnemyType>().Start();
                spawnedEnemies++;

            }

            if (currentWave > 8)
            {
                defaultSpawnRate = 0.5f;
            }

            if (killedEnemies >= maxEnemiesPerWave)
            {
                incrementWave();
            }

        }
    }


    public void SpawnBoss()
    {
        isBossSpawned = true;
        boss.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<Music>().playBossMusic();
        boss.GetComponent<EnemyType>().Start();
        boss.GetComponent<Boss>().CustomStart();
        spawnedEnemies++;
    }


    public void incrementWave()
    {
        isBossSpawned = false;
        currentWave++;
        spawnedEnemies = 0;
        killedEnemies = 0;
        maxEnemiesPerWave += enemiesIncrementPerWave;

        PlayerPrefs.SetInt("Wave", currentWave);
        StartCoroutine(GameObject.Find("Canvas").GetComponent<BaseWorldCanvas>().NextWave());
    }



    private IEnumerator SlowedRoutine()
    {
        while (true)
        {
            float updateTime = 0.5f;
            WaitForSeconds waitForSeconds = new WaitForSeconds(updateTime);
            while (true)
            {
                yield return waitForSeconds;

                if (slowedDuration <= 0)
                {
                    isSlowed = false;
                    slowed.SetActive(false);
                }
                else slowedDuration -= updateTime;
            }
        }
    }


    public void SlowEnemies(float slowMultiplier)
    {
        slowed.SetActive(true);
        isSlowed = true;
        slowedDuration += player.GetComponentInChildren<PlayerType>().GetPowerUpDuration();
    }


    public bool IsSlowed()
    {
        return isSlowed;
    }


    public float GetWavePercentage()
    {
        if (killedEnemies <= 0) return 0;
        else return (float)killedEnemies / maxEnemiesPerWave;

    }


    public void killEnemy()
    {
        killedEnemies++;
    }

}