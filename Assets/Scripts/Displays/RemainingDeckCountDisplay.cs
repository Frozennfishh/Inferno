using UnityEngine;
using TMPro;

public class DisplayRemainingDeckCount : MonoBehaviour
{
    public TextMeshProUGUI remainingDeckCountText; // Reference to the UI Text component
    public DrawCards gameManager; // Reference to the GameManager

    void Start()
    {
        UpdateRemainingDeckCount(); // Initial update
    }

    private void Update()
    {
        UpdateRemainingDeckCount();
    }

    public void UpdateRemainingDeckCount()
    {
        if (remainingDeckCountText != null && gameManager != null)
        {
            remainingDeckCountText.text = "" + gameManager.GetRemainingStoryDeckCount();
        }
    }
}