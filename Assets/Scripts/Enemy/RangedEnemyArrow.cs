using UnityEngine;

public class RangedEnemyArrow : MonoBehaviour
{
    public Rigidbody2D rb;

    public float arrowSpeed;
    public int arrowDamage;

    private GameObject[] players;
    private Transform playerPos;
    private ObjectPool pool;

    private float travelTime = 7;

    public void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        players = GameObject.FindGameObjectsWithTag("Player");
        travelTime = 7;
        //To find closest player
        if (players.Length == 1)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        else
        {
            foreach (GameObject x in players)
            {
                if (playerPos == null)
                {
                    playerPos = x.GetComponent<Transform>();
                }
                else if ((playerPos.position - this.transform.position).sqrMagnitude > (x.transform.position - this.transform.position).sqrMagnitude)
                {
                    playerPos = x.GetComponent<Transform>();
                }
            }
        }

        var heading = playerPos.position - transform.position;
        heading = (1f / heading.magnitude) * heading;
        rb.velocity = heading * arrowSpeed;
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
    }

    void Update()
    {
        if (travelTime <= 0)
        {
            pool.PutBackInPool(gameObject);
        }
        else
        {
            travelTime -= Time.deltaTime;
        }
    }


    // Deleting of arrows when it touches enemy or the water -------------------
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(arrowDamage);
            pool.PutBackInPool(gameObject);

        }
        if (other.gameObject.CompareTag("Shield"))
        {
            other.gameObject.GetComponentInParent<Mage>().ShieldHit();
            pool.PutBackInPool(gameObject);
        }
        else if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("EnemyArrow"))
        {
            pool.PutBackInPool(gameObject);
        }

    }


}