using UnityEngine;

public class MageFlame
    : MonoBehaviour
{
    public Rigidbody2D rb;

    private readonly float flameSpeed = 6f;
    private int flameDamage;

    //public AudioSource dmgSound;


    public void Start()
    {
        rb.velocity = transform.right * flameSpeed;
        flameDamage = (int)GameObject.Find("Mage").GetComponent<Mage>().GetCurrAttack();
    }


    // Deleting of arrows when it touched enemy or the water -------------------
    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        EnemyType enemy = hitInfo.gameObject.GetComponent<EnemyType>();
        if (enemy != null)
        {
            enemy.TakeDamage(flameDamage);
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);

        }
        else if (hitInfo.gameObject.CompareTag("Obstacle") || hitInfo.gameObject.CompareTag("EnemyArrow"))
        {
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);
        }

    }

}