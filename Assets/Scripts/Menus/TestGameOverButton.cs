using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestGameOverButton : MonoBehaviour
{
    public Button testGameOverButton; // Reference to the UI Button

    private void Start()
    {
        // Add listener to the button's onClick event
        testGameOverButton.onClick.AddListener(OnTestGameOverButtonClick);
    }

    private void OnTestGameOverButtonClick()
    {
        SceneManager.LoadScene("Game Over");
    }
}
