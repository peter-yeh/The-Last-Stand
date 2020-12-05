using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordslash : MonoBehaviour
{
    public Rigidbody2D rb;

    private readonly float slashSpeed = 7f;
    private int slashDamage;

    public float travelTime = 0.2f;

    public void Start()
    {
        rb.velocity = transform.right * slashSpeed;
        slashDamage = (int)GameObject.Find("Swordsman").GetComponent<Swordsman>().GetCurrAttack();
    }

    void Update()
    {
        if (travelTime <= 0)
        {
            travelTime = 0.2f;
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);
        }
        else
        {
            travelTime -= Time.deltaTime;
        }
    }

    // Deleting of arrows when it touched enemy or the water -------------------
    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        EnemyType enemy = hitInfo.gameObject.GetComponent<EnemyType>();
        if (enemy != null)
        {
            enemy.TakeDamage(slashDamage);
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);

        }
        else if (hitInfo.gameObject.CompareTag("Obstacle") || hitInfo.gameObject.CompareTag("EnemyArrow"))
        {
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);
        }

    }
}
