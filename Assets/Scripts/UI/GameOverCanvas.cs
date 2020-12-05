using UnityEngine;
using UnityEngine.UI;


public class GameOverCanvas : Canvas
{
#pragma warning disable 0649
    [SerializeField] private Text title;
    [SerializeField] private Text scoreText;
    [SerializeField] private string gameOverText;
    [SerializeField] private string newHighScoreText;

    private void Awake()
    {
        scoreText.text = "Score: " + GameManager.Instance.GetScore().ToString();

        if (GameManager.Instance.GetScore() == GameManager.Instance.GetHighScore())
            title.text = newHighScoreText;

        else
            title.text = gameOverText;

    }


}