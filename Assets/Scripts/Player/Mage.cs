using System.Collections;
using UnityEngine;

public class Mage : PlayerType
{
    [SerializeField] private GameObject manaBarObject;
    [SerializeField] private GameObject shield;
    public GameObject mageBar;
    private int shieldHp;
    //private float shieldTime;
    private int maxMana = 100;
    private int mana = 100;

    public string shieldCtrl;
    public string poisonCtrl;
    private Coroutine routine;


    private void Start()
    {
        playerStats = new float[,]{{100, 100, 1.1f},
                                    {5, 5, 1.05f},
                                    {1, 1, 1 },
                                    {5, 5, 1.1f }};

        SetHealthDifficulty();

        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        manaBarObject.SetActive(true);
        routine = StartCoroutine(Routine());
    }


    public void CustomStart()
    {
        if(routine != null) StopCoroutine(routine);
        StartCoroutine(Routine());
        mageBar.SetActive(true);
    }


    private void Update()
    {
        if (Input.GetKeyDown(shieldCtrl) && mana >= 20)
        {
            IceShield();
            mana -= 20;
        }

        if (Input.GetKeyDown(poisonCtrl) && mana >= 30)
        {
            PoisonBlast();
            mana -= 30;
        }

    }


    public override void Attack()
    {
        if (mana >= 5)
        {
            GameObject flame = pool.SpawnObject("MageFlame");
            flame.transform.position = attackPosition.position;
            flame.transform.rotation = attackPosition.rotation;
            flame.GetComponent<MageFlame>().Start();
            mana -= 5;
        }
    }


    private void IceShield()
    {
        shieldHp = 5;
        //shieldTime = 5;
        shield.SetActive(true);
    }


    private void PoisonBlast()
    {
        GameObject flame = pool.SpawnObject("PoisonFlame");
        flame.transform.position = attackPosition.position;
        flame.transform.rotation = attackPosition.rotation;
        flame.GetComponent<MagePoison>().Start();
    }


    private IEnumerator Routine()
    {
        Bar manaBar = manaBarObject.GetComponent<Bar>();
        float updateTime = 0.2f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(updateTime);
        while (true)
        {
            yield return waitForSeconds;

            if (mana < maxMana) mana++;

            manaBar.SetSize((float)mana / maxMana);

            //Debug.Log("This coroutine is running");

            if (shieldHp <= 0)
            {
                shield.SetActive(false);
            }

            //if (shieldTime > 0)
            //{
            //    shieldTime -= updateTime;
            //}

        }

    }

    public void ShieldHit()
    {
        shieldHp--;
        shield.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 105/255f);
        Invoke("DefaultColour", .1f);
    }

    private void DefaultColour()
    {
        shield.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 105/255f);
    }


}