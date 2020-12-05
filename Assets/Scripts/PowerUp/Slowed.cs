using UnityEngine;

public class Slowed : PowerUp
{

    protected override void PickUp(Collision2D player)
    {
        GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().SlowEnemies(0.5f);
    }

}