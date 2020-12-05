using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehaviour : StateMachineBehaviour
{
    private float timer;
    public float maxTime;
    public float minTime;
    private int rand;

    private GameObject[] players;
    private Transform playerPos;
    public float slowedSpeed;
    public float defaultSpeed;
    private float speed;
    protected SpriteRenderer thisSprite;
    private int upOrDown;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
        //playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        thisSprite = animator.GetComponent<SpriteRenderer>();
        rand = Random.Range(0, 2);
        upOrDown = Random.Range(0, 2);

        players = GameObject.FindGameObjectsWithTag("Player");
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
                else if ((playerPos.position - animator.transform.position).sqrMagnitude > (x.transform.position - animator.transform.position).sqrMagnitude)
                {
                    playerPos = x.GetComponent<Transform>();
                }
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().IsSlowed()) speed = slowedSpeed;
        else speed = defaultSpeed;

        if (timer <= 0)
        {
            if (rand == 0)
            {
                animator.SetTrigger("Jump");
                //Debug.Log("Jump");
            } else if (rand == 1)
            {
                animator.SetTrigger("Attack");
                //Debug.Log("Attack");
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        Vector2 target = new Vector2(playerPos.position.x, playerPos.position.y);
        if (animator.transform.position.x < playerPos.position.x)
        {
            thisSprite.flipX = false;
        }
        else
        {
            thisSprite.flipX = true;
        }

        if (System.Math.Abs(animator.velocity.x) < 1 && Boss.touchingTree)
        {
            //Debug.Log("YAY");
            if (upOrDown == 0)
            {
                animator.transform.position = Vector2.MoveTowards(animator.transform.position, new Vector2(animator.transform.position.x, animator.transform.position.y + 1), speed * Time.deltaTime);
            }
            //Debug.Log(animator.transform.position);
            else
            {
                animator.transform.position = Vector2.MoveTowards(animator.transform.position, new Vector2(animator.transform.position.x, animator.transform.position.y - 1), speed * Time.deltaTime);
            }
        }
        else
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, new Vector2(target.x, animator.transform.position.y) , speed * Time.deltaTime);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
