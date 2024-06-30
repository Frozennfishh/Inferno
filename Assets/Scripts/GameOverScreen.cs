using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
        SceneManager.LoadScene("SampleScene");
    }

    public void mainMenuButton()
    {
        Application.Quit();
    }
}
