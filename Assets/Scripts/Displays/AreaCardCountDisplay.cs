using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaCardCountDisplay : MonoBehaviour
{
    public AreaManager areaManager;  // Reference to the AreaManager
    public TextMeshProUGUI cardCountText;  // Reference to the TextMeshProUGUI component
    public float updateInterval = 1.0f; // Interval in seconds to update the text

    private void Start()
    {
        if (areaManager != null && cardCountText != null)
        {
            InvokeRepeating(nameof(UpdateCardCountText), 0, updateInterval);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(UpdateCardCountText));
    }

    private void UpdateCardCountText()
    {
        Dictionary<string, int> cardCounts = areaManager.GetCardCounts();
        string displayText = "";
        foreach (var entry in cardCounts)
        {
            displayText += $"{entry.Key}: {entry.Value}\n";
        }
        cardCountText.text = displayText;
    }
}
