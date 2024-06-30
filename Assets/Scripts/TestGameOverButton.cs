using UnityEngine;
using UnityEngine.UI;

public class TestGameOverButton : MonoBehaviour
{
    public Button testGameOverButton; // Reference to the UI Button
    public GameOverScreen gameOverScreen; // Reference to the GameOverScreen script

    private void Start()
    {
        // Add listener to the button's onClick event
        testGameOverButton.onClick.AddListener(OnTestGameOverButtonClick);
    }

    private void OnTestGameOverButtonClick()
    {
        // Call the gameOver function from the GameOverScreen script
        if (gameOverScreen != null)
        {
            Debug.Log("herlp");
            gameOverScreen.gameOver();
        }
        else
        {
            Debug.LogError("GameOverScreen script not assigned!");
        }
    }
}
