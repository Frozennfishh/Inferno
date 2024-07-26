using UnityEngine;
using TMPro;

public class DrawnDeckCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI drawnDeckCountText; // Reference to the UI Text component
    public DrawCards gameManager; // Reference to the GameManager

    void Start()
    {
        UpdateDrawnDeckCount(); // Initial update
    }

    private void Update()
    {
        UpdateDrawnDeckCount();
    }

    public void UpdateDrawnDeckCount()
    {
        if (drawnDeckCountText != null && gameManager != null)
        {
            drawnDeckCountText.text = "Drawn Cards: " + gameManager.GetDrawnStoryCardsCount();
        }
    }
}