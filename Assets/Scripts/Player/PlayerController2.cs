using UnityEngine;

public class PlayerController2 : PlayerController
{
    private void Start()
    {

        switch (PlayerPrefs.GetInt("Character2", 1))
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
        dmgSound = GetComponent<AudioSource>();
        defaultColor = sprite.color;

        InitialisePowerUP();

        StartCoroutine(RoutineCheck());
    }


    private void InitialisePowerUP()
    {
        increaseAttack = GameObject.Find("IncreaseAttack2");
        increaseAttack.SetActive(false);

        speedy = GameObject.Find("Speedy2");
        speedy.SetActive(false);

        invincibility = GameObject.Find("Invincibility2");
        invincibility.SetActive(false);
    }




}