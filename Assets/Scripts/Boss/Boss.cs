using System.Collections;
using UnityEngine;

public class Boss : EnemyType { 


    [SerializeField] private GameObject bossHealthBarObject;
    private Animator anim;
    public static bool touchingTree;


    public void CustomStart()
    {
        bossHealthBarObject.SetActive(true);
        StartCoroutine(Routine());
        anim = GetComponent<Animator>();
        health = defaultHealth;
        //Debug.Log(defaultHealth);
    }

    public new void Update()
    {
        if (GetHealthPercentage() <= 0.5)
        {
            anim.SetTrigger("Stage2");
            GameObject.Find("Main Camera").GetComponent<Music>().playBossMusicStage2();
        }
    }

    public override void Patrol() { }

    public override void Chase() { }

    public override void Retreat() { }

    public override void Attack() { }

    public override void KnockedBack() { }

    public override void Stuck() { }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(attackDmg);
            other.gameObject.transform.position = Vector2.MoveTowards(other.gameObject.transform.position, transform.position, -20 * Time.deltaTime);
        }
        if (other.gameObject.CompareTag("Shield"))
        {
            other.gameObject.GetComponentInParent<Mage>().ShieldHit();
            //other.gameObject.transform.position = Vector2.MoveTowards(player.position, transform.position, -20 * Time.deltaTime);
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

    public void BossDie(bool addScore = true)
    {
        dmgSound.Play();
        //StartCoroutine(ChangeMusic());
        //Debug.Log("Dead");
        bossHealthBarObject.SetActive(false);
        gameObject.SetActive(false);
        GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().incrementWave();
        defaultHealth += 10;

        if (addScore)
            GameManager.Instance.AddScore(20);


        GameObject.Find("Main Camera").GetComponent<Music>().playMainTheme();

    }

    public float GetHealthPercentage()
    {
        return (float) health / defaultHealth;
    }


    private IEnumerator Routine()
    {
        float updateTime = 0.2f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(updateTime);
        Bar bossHealthBar = bossHealthBarObject.GetComponent<Bar>();
        while (true)
        {
            yield return waitForSeconds;
            bossHealthBar.SetSize(GetHealthPercentage());
        }
    }

    //private IEnumerator ChangeMusic()
    //{
    //    yield return new WaitForSeconds(1);
    //    GameObject.Find("Main Camera").GetComponent<Music>().playMainTheme();
    //}

}