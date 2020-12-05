using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public void Start()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }


    // ----------------------- Score ----------------------

    public void AddScore(int amt)
    {
        int score = this.GetScore();
        score += amt;
        SetScore(score);
    }


    public int GetScore()
    {
        return PlayerPrefs.GetInt("CurrentScore", 0);
    }


    private void SetScore(int amt)
    {
        PlayerPrefs.SetInt("CurrentScore", amt);
        if (this.GetScore() > this.GetHighScore()) PlayerPrefs.SetInt("HighScore", amt);
            }


    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }


    // Others -------------------------------------------------------------------------
    public void ResetPlayerPrefs()
    {
        //PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.DeleteAll();
        Debug.Log("All player prefs are cleared");
    }


    public int GetWave()
    {
        return PlayerPrefs.GetInt("Wave", 1);
    }


    public void StartGame()
    {
        SetScore(0);
        PlayerPrefs.SetInt("Wave", 1);
        Time.timeScale = 1f;
    }


    public void EndGame()
    {
        SceneManager.LoadScene("GameOver");
    }



}