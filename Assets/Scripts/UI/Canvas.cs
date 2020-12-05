using UnityEngine;
using UnityEngine.SceneManagement;


public class Canvas : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
        SceneManager.LoadScene("BaseWorld");
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
