using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuScreen : MonoBehaviour
{
    public void creditsMenu()
    {
        gameObject.SetActive(true);
    }

    public void returnToGame()
    {
        gameObject.SetActive(false);
    }
}
