using UnityEngine;

public class MagePoison : MonoBehaviour
{
    public Rigidbody2D rb;

    private float poisonSpeed = 4f;
    private int poisonDamage = 2;



    public void Start()
    {
        rb.velocity = transform.right * poisonSpeed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyType enemy = hitInfo.gameObject.GetComponent<EnemyType>();
        if (enemy != null)
        {
            enemy.TakeDamage(poisonDamage);
            //GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);

        }
        else if (hitInfo.gameObject.CompareTag("Obstacle"))
        //|| hitInfo.gameObject.CompareTag("EnemyArrow"))
        {
            GameObject.Find("ObjectPool").GetComponent<ObjectPool>().PutBackInPool(gameObject);
        }
    }


}