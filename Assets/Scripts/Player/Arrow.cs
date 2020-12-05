using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb;

    private float arrowSpeed = 6f;
    private int arrowDamage;

    //public AudioSource dmgSound;


    public void Start()
    {
        rb.velocity = transform.right * arrowSpeed;
        arrowDamage = (int)GameObject.Find("Archer").GetComponent<Archer>().GetCurrAttack();
    }


    // Deleting of arrows when it touched enemy or the water -------------------
    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        EnemyType enemy = hitInfo.gameObject.GetComponent<EnemyType>();
        if (enemy != null)
        {
            enemy.TakeDamage(arrowDamage);
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);

        }

        else if (hitInfo.gameObject.CompareTag("Obstacle") || hitInfo.gameObject.CompareTag("EnemyArrow"))
        {
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);
        }

    }

}