using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardCounterDisplay : MonoBehaviour
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
        int sum = 0;
        foreach (var entry in cardCounts)
        {
            sum += entry.Value;
        }

        if (sum >= 15)
        {
            cardCountText.text = sum.ToString() + "/15!";
        } else
        {
            cardCountText.text = sum.ToString() + "/15";
        }
    }
}
