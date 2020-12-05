using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class BaseWorldCanvas : Canvas
{
#pragma warning disable 0649

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject slowed2;

    private GameObject nextWaveMenu;
    private Text nextWaveText;

    [SerializeField] private GameObject speedBtn;
    [SerializeField] private GameObject healthBtn;
    [SerializeField] private GameObject powerupBtn;

    [SerializeField] private GameObject speedBtn2;
    [SerializeField] private GameObject healthBtn2;
    [SerializeField] private GameObject powerupBtn2;

    private int isUpgraded = 0;
    private int numPlayers;


    private GameObject player2;
    private GameObject player2UI;
    private Bar healthBar2;
    private PlayerType playerType2;

    private PlayerType playerType;
    private Text highScoreText;
    private Text scoreText;
    private Bar healthBar;

    private GameObject oWaveBar;
    private Text waveText;
    private Bar waveBar;
    private GameObject newWaveText;


    private GameObject waitingToRespawn = null;
    private Text respawnText;

    private bool tutNotRunning = true;
    private bool isPaused = false;
    private bool isMultiplayer = false; // this does the same thing as numPlayers



    private void Start()
    {
        player2 = GameObject.Find("Player2");
        player2UI = GameObject.Find("Player2UI");
        isMultiplayer = PlayerPrefs.GetInt("isMultiplayer", 0) == 1;

        nextWaveMenu = GameObject.Find("UpgradeMenu");
        nextWaveText = GameObject.Find("New Wave").GetComponent<Text>();

        if (!isMultiplayer)
        {
            player2.SetActive(false);
            player2UI.SetActive(false);
            numPlayers = 1;
            GameObject.Find("Player2Upgrade").SetActive(false);
            slowed2.SetActive(false);

        }
        else
        {
            healthBar2 = GameObject.Find("HealthBar2").GetComponent<Bar>();
            numPlayers = 2;

            // move the player 1 upgrade left and shrink 
            GameObject.Find("Player1Upgrade").transform.localScale = new Vector2(.75f, .75f);
            GameObject.Find("Player1Upgrade").transform.position = GameObject.Find("Player1NewLoc").GetComponent<Transform>().position;
        }


        nextWaveMenu.SetActive(false);


        playerType = GameObject.Find("Player").GetComponentInChildren<PlayerType>();
        highScoreText = GameObject.Find("HighScore").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Bar>();

        oWaveBar = GameObject.Find("Wave");
        waveText = oWaveBar.GetComponent<Text>();
        waveBar = GameObject.Find("WaveBar").GetComponent<Bar>();
        newWaveText = GameObject.Find("NewWave");

        respawnText = GameObject.Find("RespawnText").GetComponent<Text>();
        respawnText.enabled = false;

        StartCoroutine(ShowWave1());
        StartCoroutine(UpdateUI(.1f));
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseButton();
    }


    // Pause Menu -----------------------------------------------
    public void PauseButton()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }


    private void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }


    private void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        Text restartButtonText = GameObject.Find("RestartText").GetComponent<Text>();
        if (tutNotRunning) restartButtonText.text = "Restart";
        else restartButtonText.text = "Quit Tutorial";
    }


    public void StartGame()
    {
        if (!tutNotRunning) PlayerPrefs.SetInt("TutorialID", 99);

        base.StartGame();
    }


    // Upgrade Menu -------------------------------------------
    public IEnumerator NextWave()
    {
        yield return new WaitForSeconds(.3f);

        nextWaveMenu.SetActive(true);

        speedBtn.GetComponent<Button>().enabled = true;
        healthBtn.GetComponent<Button>().enabled = true;
        powerupBtn.GetComponent<Button>().enabled = true;
        //nextWaveText.text = "New Wave: " + GameManager.Instance.GetWave().ToString();

        if (isMultiplayer)
        {
            speedBtn2.GetComponent<Button>().enabled = true;
            healthBtn2.GetComponent<Button>().enabled = true;
            powerupBtn2.GetComponent<Button>().enabled = true;

        }

        Time.timeScale = 0;
        StartCoroutine(DisplayUpgradeUI());
    }


    private IEnumerator DisplayUpgradeUI()
    {
        yield return new WaitForSeconds(.1f);

        if (this.waitingToRespawn != null)
        {
            this.waitingToRespawn.SetActive(true);

            this.waitingToRespawn.GetComponent<PlayerController>().CustomStart();

            if (this.waitingToRespawn.GetComponentInChildren<Mage>() != null)
                this.waitingToRespawn.GetComponentInChildren<Mage>().CustomStart();

            this.waitingToRespawn = null;
        }

        respawnText.enabled = false;

        nextWaveMenu.SetActive(false);

        isUpgraded = 0;
    }


    private void Player1Upgraded()
    {
        speedBtn.GetComponent<Button>().enabled = false;
        healthBtn.GetComponent<Button>().enabled = false;
        powerupBtn.GetComponent<Button>().enabled = false;
        isUpgraded++;
        if (isUpgraded >= numPlayers) Time.timeScale = 1;
    }


    public void UpgradeSpeed()
    {
        Player1Upgraded();
        playerType.UpgradeSpeed();
        speedBtn.GetComponentInChildren<Text>().text += "I";
    }


    public void UpgradeHealth()
    {
        Player1Upgraded();
        playerType.UpgradeMaxHealthPercent();
        healthBtn.GetComponentInChildren<Text>().text += "I";
    }


    public void UpgradePowerUpDuration()
    {
        Player1Upgraded();
        playerType.UpgradePowerUpDuration();
        powerupBtn.GetComponentInChildren<Text>().text += "I";
    }


    private void Player2Upgraded()
    {
        speedBtn2.GetComponent<Button>().enabled = false;
        healthBtn2.GetComponent<Button>().enabled = false;
        powerupBtn2.GetComponent<Button>().enabled = false;
        isUpgraded++;
        if (isUpgraded > 1) Time.timeScale = 1;
    }


    public void UpgradeSpeed2()
    {
        Player2Upgraded();
        playerType2.UpgradeSpeed();
        speedBtn2.GetComponentInChildren<Text>().text += "I";
    }


    public void UpgradeHealth2()
    {
        Player2Upgraded();
        playerType2.UpgradeMaxHealthPercent();
        healthBtn2.GetComponentInChildren<Text>().text += "I";
    }


    public void UpgradePowerUpDuration2()
    {
        Player2Upgraded();
        playerType2.UpgradePowerUpDuration();
        powerupBtn2.GetComponentInChildren<Text>().text += "I";
    }


    public IEnumerator ShowWave1()
    {
        yield return new WaitForSeconds(2);

        newWaveText.SetActive(false);
    }


    // private methods ----------------------------------------
    private IEnumerator UpdateUI(float refreshRate)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(refreshRate);
        while (true)
        {
            yield return waitForSeconds;

            if (tutNotRunning)
            {
                waveText.text = "Wave " + GameManager.Instance.GetWave().ToString();
                highScoreText.text = "High Score: " + GameManager.Instance.GetHighScore().ToString();
                waveBar.SetSize(GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().GetWavePercentage());
            }
            scoreText.text = "Score: " + GameManager.Instance.GetScore().ToString();

            if (isMultiplayer && playerType2 == null) playerType2 = player2.GetComponentInChildren<PlayerType>();

            if (healthBar != null) healthBar.SetSize(playerType.GetHealthPercentage());

            if (healthBar2 != null) healthBar2.SetSize(playerType2.GetHealthPercentage());

        }
    }


    public void IsTutorialRunning(bool b)
    {
        waveText.enabled = b;
        oWaveBar.SetActive(b);
        highScoreText.enabled = b;
        tutNotRunning = b;
    }


    public void EndGame(GameObject playerToRespawn)
    {
        if (this.waitingToRespawn != null || !this.isMultiplayer)
        {
            GameManager.Instance.EndGame();
        }
        else
        {
            this.waitingToRespawn = playerToRespawn;
            respawnText.enabled = true;
            Debug.Log("Player is dead");

        }
    }


}