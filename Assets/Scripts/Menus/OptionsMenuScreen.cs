using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuScreen : MonoBehaviour
{
    public void optionsMenu()
    {
        gameObject.SetActive(true);
    }

    public void returnToGame()
    {
        gameObject.SetActive(false);
    }
}
