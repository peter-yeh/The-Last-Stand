using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{


    private WaitForSeconds shortWait = new WaitForSeconds(.5f); // was ..5f
    private WaitForSeconds longWait = new WaitForSeconds(3);
    private WaitForSeconds longerWait = new WaitForSeconds(5);
    private Canvas canvas;
    private EnemySpawner enemySpawner;
    private PowerUpSpawner powerUpSpawner;
    private PlayerController playerController;
    private ObjectPool pool;
    private PlayerType player;
    private Text tutorialText;
    private Coroutine coroutine;
    private GameObject tutorialBackground;
    private int tutorialID;


    private IEnumerator Start()
    {
        tutorialID = PlayerPrefs.GetInt("TutorialID", 0);
        tutorialBackground = GameObject.Find("TutorialTextBackground");
        tutorialBackground.SetActive(false);

        if (tutorialID >= 99)
        {
            //Debug.Log("The tutorial id is more than 99, exiting tutorial");
            yield break;
        }

        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        //enemySpawner.spawnRate = float.MaxValue;
        enemySpawner.maxEnemiesPerWave = int.MaxValue;

        powerUpSpawner = GameObject.Find("PowerUpSpawner").GetComponent<PowerUpSpawner>();
        powerUpSpawner.start = false;

        tutorialBackground.SetActive(true);

        yield return shortWait; // This is important these codes must only run after their start() is called

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
        player = playerController.GetComponentInChildren<PlayerType>();
        tutorialText = GameObject.Find("Tutorial").GetComponent<Text>();

        tutorialText.enabled = true;
        canvas.GetComponent<BaseWorldCanvas>().IsTutorialRunning(false);


        if (tutorialID > 0)
        {
            tutorialText.text = "Continuing from the last tutorial";
            yield return longWait;
        }

        NextTutorial(tutorialID);

        coroutine = StartCoroutine(CheckHealthRoutine());
    }


    private void NextTutorial(int id)
    {
        PlayerPrefs.SetInt("TutorialID", id);
        StartCoroutine("Tutorial" + id);
    }


    private IEnumerator Tutorial0()
    {
        tutorialText.text = "Welcome to The Last Stand Tutorial!";

        yield return longWait;

        NextTutorial(1);
    }


    private IEnumerator Tutorial1()
    {
        tutorialText.text = "Press A/D to move left/right";

        while (true)
        {
            yield return shortWait;

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal_P1")) > 0.5f) break;
        }
        NextTutorial(2);
    }


    private IEnumerator Tutorial2()
    {
        tutorialText.text = "Press W/S to move up/down" + 
            "\n (Arrow Keys to move for player 2)";

        while (true)
        {
            yield return shortWait;
            if (Mathf.Abs(Input.GetAxisRaw("Vertical_P1")) > 0.5f) break;
        }
        NextTutorial(3);
    }


    private IEnumerator Tutorial3()
    {
        tutorialText.text = "Press Spacebar to use the primary attack of the character" +
            "\n (, for player 2)";

        while (true)
        {
            yield return shortWait;

            if (Input.GetButton("Fire1_P1")) break;
        }
        NextTutorial(4);
    }


    private IEnumerator Tutorial4()
    {
        tutorialText.text = "Press ESC button to pause the game" +
            "\nYou can quit this tutorial from the pause menu at anytime";

        yield return longWait;

        NextTutorial(6);
    }


    private IEnumerator Tutorial6()
    {
        tutorialText.text = "The difficulty settings decide the players starting health";

        yield return longWait;

        tutorialText.text = "Choose harder difficulties for a greater challenge!";

        yield return longWait;

        NextTutorial(9);
    }


    private IEnumerator Tutorial9()
    {
        yield return longWait;

        if (player.GetComponent<Mage>() != null)
        {
            tutorialText.text = "Now moving on to the special player: Mage";
            NextTutorial(10);
        }
        else
        {
            tutorialText.text = "Now moving on to the enemy";
            NextTutorial(20);
        }


    }


    private IEnumerator Tutorial10()
    {
        tutorialText.text = "You would have noticed only the Mage has a blue bar";

        yield return longWait;

        tutorialText.text = "It represents his mana, and starts with 100 maximum mana.";

        yield return longWait;

        tutorialText.text = "He has 3 spells and casting any of his spells consumes mana";

        yield return longWait;

        tutorialText.text = "His basic spell consumes 5 mana";

        NextTutorial(11);
    }


    private IEnumerator Tutorial11()
    {
        tutorialText.text = "Press F to generate a shield which consumes 20 mana" +
            "\n (. for player 2)";

        while (true)
        {
            yield return shortWait;

            if (Input.GetKey(KeyCode.F)) break;
        }
        NextTutorial(12);
    }


    private IEnumerator Tutorial12()
    {
        tutorialText.text = "Press G to shoot out a poison blast which would deal damage to anything in it's path" +
            "\nIt consumes 30 mana " +
            "(/ for player 2)";

        while (true)
        {
            yield return shortWait;

            if (Input.GetKey(KeyCode.G)) break;
        }
        NextTutorial(20);
    }


    private IEnumerator Tutorial20()
    {
        tutorialText.text = "Enemies spawn in waves" + 
            "\nClear all the enemies to proceed onto the next wave";

        yield return longerWait;

        tutorialText.text = "Beware! A boss will spawn after you have defeated enough waves";

        yield return longWait;


        tutorialText.text = "There are 4 basic types of enemy";

        yield return longWait;

        NextTutorial(21);
    }


    private IEnumerator Tutorial21()
    {
        tutorialText.text = "First is the Blue Slime, it has average health and speed" +
            "\nTake him down to proceed";
        int currScore = GameManager.Instance.GetScore();
        GameObject enemy;

        while (GameManager.Instance.GetScore() == currScore)
        {
            enemy = pool.SpawnObject("BlueSlimeEnemy");
            enemy.transform.position = SpawnLoc();
            enemy.GetComponent<EnemyType>().Start();


            yield return longWait;
        }


        NextTutorial(22);
    }


    private IEnumerator Tutorial22()
    {
        tutorialText.text = "This is the Orange Slime, it has high health but low speed" +
            "\nTake him down to proceed";
        int currScore = GameManager.Instance.GetScore();
        GameObject enemy;

        while (GameManager.Instance.GetScore() == currScore)
        {
            enemy = pool.SpawnObject("OrangeSlimeEnemy");
            enemy.transform.position = SpawnLoc();
            enemy.GetComponent<EnemyType>().Start();


            yield return longWait;
        }


        NextTutorial(23);
    }


    private IEnumerator Tutorial23()
    {
        tutorialText.text = "This is the Green Slime, it is ranged but has low health" +
            "\nTake him down to proceed";
        int currScore = GameManager.Instance.GetScore();

        GameObject enemy = pool.SpawnObject("GreenSlimeEnemy");
        enemy.transform.position = SpawnLoc();
        enemy.GetComponent<EnemyType>().Start();


        while (GameManager.Instance.GetScore() == currScore)
            yield return longWait;


        NextTutorial(24);
    }


    private IEnumerator Tutorial24()
    {
        tutorialText.text = "Finally, this is the Cobra, it is fast but frail" +
            "\nTake him down to proceed";
        int currScore = GameManager.Instance.GetScore();
        GameObject enemy;

        while (GameManager.Instance.GetScore() == currScore)
        {
            enemy = pool.SpawnObject("CobraEnemy");
            enemy.transform.position = SpawnLoc();
            enemy.GetComponent<EnemyType>().Start();

            yield return longWait;
        }


        NextTutorial(25);
    }


    private IEnumerator Tutorial25()
    {
        tutorialText.text = "Moving on to powerups, these are life savers";

        yield return longWait;

        NextTutorial(30);
    }


    private IEnumerator Tutorial30()
    {
        tutorialText.text = "There are 5 different powerups which are spawned randomly across the map";

        yield return longWait;

        NextTutorial(31);
    }


    private IEnumerator Tutorial31()
    {
        tutorialText.text = "Only 5 power ups max can be spawned at any point" +
            "\nWe will try these on the enemies now!";


        yield return longWait;

        NextTutorial(32);
    }


    private IEnumerator Tutorial32()
    {

        enemySpawner.spawnRate = 5;
        enemySpawner.StartCo();

        tutorialText.text = "Drinking this red potion restores your health, try it!";
        player.LoseHealth(10);

        float healthPercent = player.GetHealthPercentage();

        pool.SpawnObject("RecoverHealth").transform.position = SpawnLoc();

        while (true)
        {
            yield return shortWait;

            if (player.GetHealthPercentage() > healthPercent)
                break;

        }

        NextTutorial(33);
    }


    private IEnumerator Tutorial33()
    {
        tutorialText.text = "The shield makes you invincible for 5 seconds" +
            "\n Enemies that killed this way won't add to your score, try it!";
        pool.SpawnObject("Invincibility").transform.position = SpawnLoc();

        while (true)
        {
            yield return longWait;

            if (playerController.IsInvincible()) break;
        }

        NextTutorial(34);
    }


    private IEnumerator Tutorial34()
    {
        tutorialText.text = "This blue potion drastically increases your speed, try it!";
        pool.SpawnObject("Speedy").transform.position = SpawnLoc();

        while (true)
        {
            yield return longWait;

            if (player.GetCurrSpeed() > 5) break;
        }

        NextTutorial(35);
    }

    private IEnumerator Tutorial35()
    {
        tutorialText.text = "Eating this delicious meat increases your attack damage for 5 seconds, try it!";
        pool.SpawnObject("IncreaseAttack").transform.position = SpawnLoc();

        PlayerType player = playerController.characterChoosen.GetComponent<PlayerType>();

        while (true)
        {
            yield return longWait;

            if (player.GetCurrAttack() > player.GetDefaultAttack()) break;
        }

        NextTutorial(36);
    }


    private IEnumerator Tutorial36()
    {
        tutorialText.text = "The snowflake chills and slows all enemies for 5 seconds, try it!";
        pool.SpawnObject("Slowed").transform.position = SpawnLoc();


        while (true)
        {
            yield return longWait;

            if (enemySpawner.IsSlowed()) break;
        }

        NextTutorial(37);
    }


    private IEnumerator Tutorial37()
    {
        tutorialText.text = "That's all for powerups!";

        yield return longWait;

        NextTutorial(99);
    }


    private IEnumerator Tutorial99()
    {
        tutorialText.text = "You have learned everything you need to survive, now it's time for The Last Stand!";

        yield return longWait;


        tutorialText.enabled = false;
        powerUpSpawner.start = true;
        enemySpawner.ResetStats();
        tutorialBackground.SetActive(false);

        canvas.GetComponent<BaseWorldCanvas>().IsTutorialRunning(true);
        StartCoroutine(canvas.GetComponent<BaseWorldCanvas>().ShowWave1());

        StopCoroutine(coroutine);
    }


    private IEnumerator CheckHealthRoutine()
    {
        while (true)
        {
            if (player.GetHealthPercentage() < .4f)
            {
                player.RecoverHealthPercent(.7f);
                tutorialText.text += "\n~~~~~~~~~~~~~~~~~~~~Stop dying!~~~~~~~~~~~~~~~~~~~~";
                Debug.Log("Health is recovered by 70 percent");
            }

            if (tutorialText.text.Length > 200)
                tutorialText.text = "Stop trying to break the game!";


            yield return shortWait;
        }
    }


    private readonly Vector2[] vectors = new Vector2[] {
        new Vector2(3, 0), new Vector2(0, -3), new Vector2(-3, 0), new Vector2(0, 3) };

    private Vector2 SpawnLoc() // spawn the enemy within 3 units from the player
    {
        Vector2 playerLocation = playerController.transform.position;

        foreach (Vector2 vec in vectors)
        {
            if (Utility.IsWithinBoundary(vec + playerLocation))
                return vec + playerLocation;
        }

        return vectors[1]; // all location is not suitable, default to right side of player
    }


}