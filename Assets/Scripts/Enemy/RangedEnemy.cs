using UnityEngine;

public class RangedEnemy : EnemyType
{
    public Transform attackPosition;
    public float startTimeBtwShots;
    private float timeBtwnShots = 0;
    public GameObject chicken;

    public override void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, randomPos) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomPos = Utility.RandomVector2D();
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if ((Vector2.Distance(transform.position, player.position) < 7f))
        {
            state = State.Chase;
        }

    }

    public override void Chase()
    {
        if (transform.position.x < player.position.x)
        {
            thisSprite.flipX = false;
        }
        else
        {
            thisSprite.flipX = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if ((Vector2.Distance(transform.position, player.position) < 5f))
        {
            state = State.Attack;

        }
        //else if ((Vector2.Distance(transform.position, player.position) > 7f))
        //{
        //    state = State.Patrol;
        //}
    }

    public override void KnockedBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, -knockedBackSpeed * Time.deltaTime);

        if (knockedBackTime <= 0)
        {
            state = State.Attack;
            knockedBackTime = startKnockedBackTime;

        }
        else
        {
            knockedBackTime -= Time.deltaTime;
        }
    }

    public override void Attack()
    {
        if (transform.position.x < player.position.x)
        {
            thisSprite.flipX = false;
        }
        else
        {
            thisSprite.flipX = true;
        }

        if (timeBtwnShots <= 0)
        {
            GameObject bullet = pool.SpawnObject("RangedEnemyArrow");
            //pool.SpawnObject("RangedEnemyArrow", attackPosition.position, Quaternion.identity);
            //Instantiate(chicken, attackPosition.position, attackPosition.rotation);
            bullet.transform.position = attackPosition.position;
            bullet.transform.rotation = attackPosition.rotation;
            bullet.GetComponent<RangedEnemyArrow>().Start();
            
            timeBtwnShots = startTimeBtwShots;
        }
        else
        {
            timeBtwnShots -= Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, player.position) < 3f)
        {
            state = State.Retreat;
        }
        else if (Vector2.Distance(transform.position, player.position) > 7f)
        {
            state = State.Chase;
        }
    }

    public override void Retreat()
    {
        if (Vector2.Distance(transform.position, player.position) > 3f)
        {
            state = State.Attack;
        }

        transform.position = Vector2.MoveTowards(transform.position, player.position, -(speed/2) * Time.deltaTime);
    }

    public override void Stuck() { }
}
