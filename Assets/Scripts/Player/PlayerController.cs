using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Not named yet ----------------------------------------
    public GameObject swordsMan, archer, mage;
    public GameObject healthBar;


    // Attributes of player ----------------------------------
    protected bool facingRight = true;
    protected float increaseAttackDuration = 0f;
    protected float speedyDuration = 0f;
    protected float invincibilityDuration = 0f;


    // Components under the GameObject ---------------------
    [HideInInspector] public GameObject characterChoosen;
    protected Animator anim;
    protected SpriteRenderer sprite;
    protected PlayerType playerType;
    protected GameObject speedy, increaseAttack, invincibility;
    protected bool isInvincible = false;
    protected ObjectPool pool;
    public AudioSource dmgSound;
    protected bool attack;
    public string horizontalCtrl;
    public string verticalCtrl;
    public string attackCtrl;
    protected Color defaultColor;
    private Coroutine coroutine;
    public AudioSource pickUpSound;


    private void Start()
    {
        switch (PlayerPrefs.GetInt("Character", 1))
        {
            case 1:
                characterChoosen = swordsMan;
                break;

            case 2:
                characterChoosen = archer;
                break;

            case 3:
                characterChoosen = mage;
                break;
        }

        characterChoosen.gameObject.SetActive(true);
        anim = characterChoosen.GetComponent<Animator>();
        sprite = characterChoosen.GetComponent<SpriteRenderer>();
        playerType = characterChoosen.GetComponent<PlayerType>();
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        //dmgSound = GetComponent<AudioSource>();
        defaultColor = sprite.color;

        InitialisePowerUP();

        coroutine = StartCoroutine(RoutineCheck());
    }


    public void CustomStart()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(RoutineCheck());

        healthBar.SetActive(true);
        Debug.Log("Coroutine is custom started");
    }


    private void Update()
    {

        if (Input.GetAxisRaw(horizontalCtrl) > 0.5f)
        {
            if (!facingRight) Flip();    
            Move(Input.GetAxisRaw(horizontalCtrl), 0);

        }
        else if (Input.GetAxisRaw(horizontalCtrl) < -0.5f)
        {
            if (facingRight) Flip();
            Move(-Input.GetAxisRaw(horizontalCtrl), 0);

        }


        if (Mathf.Abs(Input.GetAxisRaw(verticalCtrl)) > 0.5f)
        {
            Move(0, Input.GetAxisRaw(verticalCtrl));
        }


        if (Mathf.Abs(Input.GetAxisRaw(verticalCtrl)) < 0.5f
            && Mathf.Abs(Input.GetAxisRaw(horizontalCtrl)) < 0.5f)
        {
            anim.SetBool("Running", false);
        }
        else anim.SetBool("Running", true);


        if (Input.GetButton(attackCtrl))
        {
            attack = true;
            //anim.SetTrigger("Attack");

            //Debug.Log("The player is pressing attack");
        }
        else attack = false;
            

    }




    // ------------------------------public classess--------------------------------------

    public void TakeDamage(int dmg)
    {
        FlashRed();
        GameObject damageBurst = pool.SpawnObject("DamageBurst");
        damageBurst.transform.position = transform.position;
        damageBurst.transform.rotation = transform.rotation;

        if (isInvincible) return;
        else dmgSound.Play();

        playerType.LoseHealth(dmg);

    }

    // --------------------------- Power Ups -----------------------------------


    
    private void InitialisePowerUP()
    {
        increaseAttack = GameObject.Find("IncreaseAttack");
        increaseAttack.SetActive(false);

        speedy = GameObject.Find("Speedy");
        speedy.SetActive(false);

        invincibility = GameObject.Find("Invincibility");
        invincibility.SetActive(false);
    }


    public void RecoverHealth()
    {
        playerType.RecoverHealthPercent(.2f);
    }


    public void IncreaseAttack()
    {
        increaseAttack.SetActive(true);
        playerType.IncreaseCurrAttack(2);
        increaseAttackDuration += playerType.GetPowerUpDuration();
    }


    public void Speedy()
    {
        speedy.SetActive(true);
        playerType.SetCurrSpeedPercent(1.5f);
        speedyDuration += playerType.GetPowerUpDuration();
    }


    public void Invincibility()
    {
        invincibility.SetActive(true);
        isInvincible = true;
        invincibilityDuration += playerType.GetPowerUpDuration();
    }


    // ------------------------------private classes --------------------------------------------

    // Called via Update() to move the player
    public void Move(float x, float y)
    {
        float magic = playerType.GetCurrSpeed() * Time.deltaTime;
        transform.Translate(new Vector2(x * magic, y * magic));
    }


    // Called to flip the character to face the other side
    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }


    protected IEnumerator RoutineCheck()
    {
        float updateTime = 0.2f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(updateTime);
        while (true)
        {
            //Debug.Log("Player Routine Check is runnning" + this.GetInstanceID());
            yield return waitForSeconds;

            //if (!Utility.IsWithinBoundary(transform.position))
            //    TakeDamage(5);


            if (increaseAttackDuration <= 0)
            {
                increaseAttack.SetActive(false);
                playerType.ResetCurrAttack();
            }
            else increaseAttackDuration -= updateTime;


            if (speedyDuration <= 0)
            {
                speedy.SetActive(false);
                playerType.ResetCurrSpeed();
            }
            else speedyDuration -= updateTime;


            if (invincibilityDuration <= 0)
            {
                invincibility.SetActive(false);
                isInvincible = false;
            }
            else invincibilityDuration -= updateTime;


            if (attack)
            {
                playerType.Attack();
                anim.SetTrigger("Attack");
            }

        }
    }

    private void FlashRed()
    {
        sprite.color = Color.red;
        Invoke("ResetColor", 0.1f);
    }


    private void ResetColor()
    {
        sprite.color = defaultColor;
    }


    public bool IsInvincible()
    {
        return isInvincible;
    }

    public void PlayPowerUpSound()
    {
        pickUpSound.Play();
    }

}