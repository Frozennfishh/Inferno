using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void gameOver()
    {
        gameObject.SetActive(true);
    }

    public void restartButton()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
