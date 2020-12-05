using System.Collections;
using UnityEngine;

public class Swordsman : PlayerType
{
    public string swordslashName;

    private void Start()
    {
        playerStats = new float[,]{{100, 100, 1.1f},
                                    {5, 5, 1.05f},
                                    {2, 2, 1 },
                                    {5, 5, 1.1f }};

        SetHealthDifficulty();

        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
    }



    public override void Attack()
    {
        //StartCoroutine(DealDamage());

        GameObject swordslash = pool.SpawnObject(swordslashName);
        swordslash.transform.position = attackPosition.position;
        swordslash.transform.rotation = attackPosition.rotation;

        swordslash.GetComponent<Swordslash>().Start();

    }


    //private IEnumerator DealDamage()
    //{
    //    GameObject swordslash = pool.SpawnObject("Swordslash");
    //    swordslash.transform.position = attackPosition.position;
    //    swordslash.transform.rotation = attackPosition.rotation;
    //    swordslash.GetComponent<Swordslash>().Start();

    //    yield return null;

    //    //pool.PutBackInPool(sword);
    //}


}