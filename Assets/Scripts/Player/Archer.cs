using UnityEngine;

public class Archer : PlayerType
{
    public string ArrowName;
    private void Start()
    {
        playerStats = new float[,]{{100, 100, 1.1f},
                                    {5, 5, 1.05f},
                                    {1, 1, 1 },
                                    {5, 5, 1.1f }};

        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>(); SetHealthDifficulty();

    }


    public override void Attack()
    {
        GameObject arrow = pool.SpawnObject(ArrowName);
        arrow.transform.position = attackPosition.position;
        arrow.transform.rotation = attackPosition.rotation;
        arrow.GetComponent<Arrow>().Start();
    }


}