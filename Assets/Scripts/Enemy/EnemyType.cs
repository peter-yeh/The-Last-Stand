using UnityEngine;

public abstract class EnemyType : MonoBehaviour
{

    protected EnemySpawner enemySpawner;
    protected ObjectPool pool;

    //----------------------------------
    // Enemy stats
    //----------------------------------
    public float speed;
    public float defaultSpeed;
    private float slowedSpeed;

    protected float spawnTime;
    public float startSpawnTime;
    protected float waitTime;
    public float startWaitTime;
    protected int health;
    public int defaultHealth;
    public int attackDmg;
    protected float knockedBackTime;
    protected float knockedBackSpeed;
    public float startKnockedBackTime;
    protected bool Slowable = true;
    protected float slowedTime = 5;
    protected bool touchingTree;
    //----------------------------------
    // End of Enemy stats
    //----------------------------------

    //----------------------------------
    // Enemy state
    //----------------------------------
    public enum State
    {
        Spawn,
        Patrol,
        Chase,
        Retreat,
        Attack,
        knockedBack,
        Stuck
    }
    public State state;
    //----------------------------------
    // End of Enemy state
    //----------------------------------

    //----------------------------------
    // Enemy movement location 
    //----------------------------------
    protected Vector2 randomPos;
    protected GameObject[] players;
    protected Transform player;
    protected int upOrDown;
    //public float minX;
    //public float maxX;
    //public float minY;
    //public float maxY;
    //----------------------------------
    // End of Enemy movement location 
    //----------------------------------

    //public GameObject damageBurst;
    protected SpriteRenderer thisSprite;
    protected Color originalColor;
    protected AudioSource dmgSound;


    public void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();

        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();

        health = defaultHealth;

        spawnTime = startSpawnTime;

        waitTime = startWaitTime;

        randomPos = Utility.RandomVector2D();

        //player = GameObject.FindGameObjectWithTag("Player").transform;

        players = GameObject.FindGameObjectsWithTag("Player");

        state = State.Spawn;

        knockedBackTime = startKnockedBackTime;

        knockedBackSpeed = (speed / 2);

        thisSprite = GetComponent<SpriteRenderer>();

        originalColor = thisSprite.color;

        speed = defaultSpeed;

        slowedSpeed = defaultSpeed / 2;

        dmgSound = GetComponent<AudioSource>();

        upOrDown = Random.Range(0, 2);
    }

    public void Update()
    {
        if (enemySpawner.IsSlowed()) speed = slowedSpeed;
        else speed = defaultSpeed;

        players = GameObject.FindGameObjectsWithTag("Player");
        //To find closest player
        if (players.Length == 1)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            foreach (GameObject x in players)
            {
                if (player == null)
                {
                    player = x.transform;
                }
                else if ((player.position - transform.position).sqrMagnitude > (x.transform.position - transform.position).sqrMagnitude)
                {
                    player = x.transform;
                }
            }
        }

        switch (state)
        {
            case State.Spawn:
                Spawn();
                break;

            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Retreat:
                Retreat();
                break;

            case State.Attack:
                Attack();
                break;

            case State.knockedBack:
                KnockedBack();
                break;

            case State.Stuck:
                Stuck();
                break;

        }


    }

    public void Spawn()
    {
        //FlashWhite();
        //ResetColor();
        if (spawnTime <= 0)
        {
            state = State.Chase;
        }
        else
        {
            spawnTime -= Time.deltaTime;
        }
    }

    public abstract void Patrol();

    public abstract void Chase();

    public abstract void Retreat();

    public abstract void Attack();

    public abstract void KnockedBack();

    public abstract void Stuck();

    public void Die(bool addScore = true)
    {
        dmgSound.Play();
        StartCoroutine(GameObject.Find("ObjectPool").GetComponent<ObjectPool>().LatePutBackInPool(gameObject, 0.015f));
        enemySpawner.killEnemy();

        if (addScore)
            GameManager.Instance.AddScore(1);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //dmgSound.Play();
            other.gameObject.GetComponent<PlayerController>().TakeDamage(attackDmg);
            Die(false);
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            other.gameObject.GetComponentInParent<Mage>().ShieldHit();
            Die(false);
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            touchingTree = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            touchingTree = false;
        }
    }

    public void TakeDamage(int damage)
    {
        //Instantiate(damageBurst, transform.position, transform.rotation);
        GameObject damageBurst = pool.SpawnObject("DamageBurst");
        damageBurst.transform.position = transform.position;
        damageBurst.transform.rotation = transform.rotation;

        FlashRed();

        dmgSound.Play();

        health -= damage;

        if (health <= 0)
        {
            if (gameObject.GetComponent<Boss>() != null)
            {
                gameObject.GetComponent<Boss>().BossDie();
            }
            else
            {
                Die();
            }
        }

        state = State.knockedBack;
    }

    protected void FlashRed()
    {
        thisSprite.color = Color.red;
        Invoke("ResetColor", 0.1f);
    }

    //void FlashWhite()
    //{
    //    thisSprite.color = Color.white;
    //    Invoke("ResetColor", 0.25f);
    //}

    protected void ResetColor()
    {
        thisSprite.color = Color.white;
    }

    //public void ResetAttributes()
    //{
    //    this.health = defaultHealth;
    //    this.speed = defaultSpeed;
    //}


}