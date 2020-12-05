using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField, Range(0, 5)] private float spawnRate = 3;
    [SerializeField] private int totalPowerUp = 5;

    private Transform player;
    private ObjectPool pool;
    private int numPowerUp;
    [HideInInspector]public bool start = true;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        numPowerUp = 0;

        StartCoroutine(SpawnPowerUp());
    }


    private IEnumerator SpawnPowerUp()
    {
        while(start)
        {
            yield return new WaitForSeconds(spawnRate);

            if (numPowerUp < totalPowerUp)
            {
                float randNum = Random.Range(0f, 1f);
                GameObject powerUp;
                numPowerUp++;
                Vector2 spawnLoc = Utility.RandomVector2D();

                while (true)
                {
                    if (Vector2.Distance(spawnLoc, player.position) > 5f) break;
                    else spawnLoc = Utility.RandomVector2D();
                }

                if (randNum <= 0.2)
                    powerUp = pool.SpawnObject("RecoverHealth");

                else if (randNum <= 0.4)
                    powerUp = pool.SpawnObject("Invincibility");

                else if (randNum <= 0.6)
                    powerUp = pool.SpawnObject("Speedy");

                else if (randNum <= 0.8)
                    powerUp = pool.SpawnObject("IncreaseAttack");

                else
                    powerUp = pool.SpawnObject("Slowed");

                powerUp.transform.position = spawnLoc;


            }
        }

    }


    public void RemovePowerUp(GameObject gameObject)
    {
        pool.PutBackInPool(gameObject);
        numPowerUp--;
    }


}