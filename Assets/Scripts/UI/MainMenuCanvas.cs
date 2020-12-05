using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MainMenuCanvas : Canvas
{

    [SerializeField] private Sprite swordsman, archer, mage;
    private Text difficulty;
    private Text highScoreText;

    private Transform player1OldLoc;
    private Transform player1NewLoc;
    private GameObject player1;
    private GameObject player2;
    private bool isMultiplayer = false;


    private Text playerIntroText;
    private GameObject tutorialPanel;


    private string swordsmanText = "Swordsman chosen: \nTanky melee class";
    private string archerText = "Archer chosen: \nFire long ranged attacks";
    private string mageText = "Mage chosen: \nCast powerful spells";



    private void Awake()
    {

        difficulty = GameObject.Find("Difficulty").GetComponentInChildren<Text>();
        highScoreText = GameObject.Find("HighScore").GetComponent<Text>();

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        player2.SetActive(false);
        //player1OldLoc = player1.GetComponent<Transform>();
        player1OldLoc = GameObject.Find("Player1OldLoc").GetComponent<Transform>();
        player1NewLoc = GameObject.Find("Player1NewLoc").GetComponent<Transform>();

        playerIntroText = GameObject.Find("PlayerText").GetComponent<Text>();
        tutorialPanel = GameObject.Find("TutorialPanel");
        tutorialPanel.SetActive(false);

        DisplayCharacters(PlayerPrefs.GetInt("Character", 1));
        DisplayDifficulty(PlayerPrefs.GetInt("Difficulty", 2));
        StartCoroutine(UpdateUI());
        PlayerPrefs.SetInt("isMultiplayer", 0);

        //Debug.Log("The Old location is: " + player1OldLoc);

    }


    // Methods called by the buttons
    public void ShowTutorials()
    {
        if (tutorialPanel.activeSelf) tutorialPanel.SetActive(false);
        else tutorialPanel.SetActive(true);
    }


    public void StartTutorial(int num)
    {
        PlayerPrefs.SetInt("TutorialID", num);
        base.StartGame();

    }


    public void ResetPrefs()
    {
        GameManager.Instance.ResetPlayerPrefs();
        StartCoroutine(UpdateUI());
    }


    private IEnumerator UpdateUI()
    {
        yield return null;

        highScoreText.text = "High Score: " + GameManager.Instance.GetHighScore().ToString();
    }


    // For character 2 and multiplayer feature
    public void Mutliplayer()
    {
        if (isMultiplayer) // the player wants single player
        {
            player1.transform.position = player1OldLoc.position;
            player2.SetActive(false);
            isMultiplayer = false;
            player1.GetComponentInChildren<Text>().enabled = true;
            PlayerPrefs.SetInt("isMultiplayer", 0);
            GameObject.Find("MultiplayerButton").GetComponentInChildren<Text>().text = "Multiplayer";
        }
        else
        {
            player1.transform.position = player1NewLoc.position;
            player2.SetActive(true);
            isMultiplayer = true;
            DisplayCharacters2(PlayerPrefs.GetInt("Character2", 1));
            player1.GetComponentInChildren<Text>().enabled = false;
            PlayerPrefs.SetInt("isMultiplayer", 1);
            GameObject.Find("MultiplayerButton").GetComponentInChildren<Text>().text = "Singleplayer";

        }
    }


    // Difficulty --------------------------------------------------------------------------------------
    public void NextDifficulty()
    {
        DisplayDifficulty(PlayerPrefs.GetInt("Difficulty", 2) + 1);
    }


    public void PrevDifficulty()
    {
        DisplayDifficulty(PlayerPrefs.GetInt("Difficulty", 2) - 1);
    }


    private void DisplayDifficulty(int choice)
    {
        switch (choice)
        {
            case 4:
            case 1:
                difficulty.text = "Easy";
                PlayerPrefs.SetInt("Difficulty", 1);

                break;

            case 2:
                difficulty.text = "Normal";
                PlayerPrefs.SetInt("Difficulty", 2);

                break;

            case 0:
            case 3:
                difficulty.text = "Hard";
                PlayerPrefs.SetInt("Difficulty", 3);

                break;
        }
    }


    // Changing characters --------------------------------------------------------------------------------
    public void NextCharacter()
    {
        DisplayCharacters(PlayerPrefs.GetInt("Character", 1) + 1);
    }


    public void PrevCharacter()
    {
        DisplayCharacters(PlayerPrefs.GetInt("Character", 1) - 1);
    }


    private void DisplayCharacters(int choice)
    {

        switch (choice)
        {
            case 4: // next char of mage is swordsman
            case 1:
                player1.GetComponentInChildren<Image>().sprite = swordsman;
                playerIntroText.text = swordsmanText;
                PlayerPrefs.SetInt("Character", 1);

                break;

            case 2:
                player1.GetComponentInChildren<Image>().sprite = archer;
                playerIntroText.text = archerText;
                PlayerPrefs.SetInt("Character", 2);

                break;


            case 0: // prev char of swordsman is mage
            case 3:
                player1.GetComponentInChildren<Image>().sprite = mage;
                playerIntroText.text = mageText;
                PlayerPrefs.SetInt("Character", 3);

                break;
        }
    }


    // Changing character for player 2 ----------------------------------------------------------------------------------------
    public void NextCharacter2()
    {
        DisplayCharacters2(PlayerPrefs.GetInt("Character2", 1) + 1);
    }


    public void PrevCharacter2()
    {
        DisplayCharacters2(PlayerPrefs.GetInt("Character2", 1) - 1);
    }


    private void DisplayCharacters2(int choice)
    {
        switch (choice)
        {
            case 4: // next char of mage is swordsman
            case 1:
                player2.GetComponentInChildren<Image>().sprite = swordsman;
                PlayerPrefs.SetInt("Character2", 1);

                break;

            case 2:
                player2.GetComponentInChildren<Image>().sprite = archer;
                PlayerPrefs.SetInt("Character2", 2);
                break;


            case 0: // prev char of swordsman is mage
            case 3:
                player2.GetComponentInChildren<Image>().sprite = mage;
                PlayerPrefs.SetInt("Character2", 3);
                break;
        }
    }


}