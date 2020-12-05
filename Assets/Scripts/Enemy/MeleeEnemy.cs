using UnityEngine;

public class MeleeEnemy : EnemyType
{

    public override void Patrol()
    {
        //Debug.Log(player.position);
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

        if ((Vector2.Distance(transform.position, player.position) < 5f))
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

        if (System.Math.Abs(GetComponent<Rigidbody2D>().velocity.x) < 1 && Slowable && touchingTree)
        {
            state = State.Stuck;
        }

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    public override void KnockedBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, -knockedBackSpeed * Time.deltaTime);

        if (knockedBackTime <= 0)
        {
            state = State.Chase;
            knockedBackTime = startKnockedBackTime;

        }
        else
        {
            knockedBackTime -= Time.deltaTime;
        }
    }

    public override void Stuck()
    {
        if (upOrDown == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        }
        //Debug.Log(animator.transform.position);
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y - 1), speed * Time.deltaTime);
        }

        if (!touchingTree)
        {
            state = State.Chase;
        }
        
    }

    public override void Attack()
    {

    }

    public override void Retreat()
    {

    }


}
