using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionCardButton : MonoBehaviour
{
    public AreaManager dropAreaManager;  // Reference to the AreaManager
    public Button optionButton;          // Reference to the button component
    public float checkInterval = 0.5f;   // Interval in seconds to check for updates

    [System.Serializable]
    public class CardRequirement
    {
        public string cardType;
        public int requiredCount;
    }

    public List<CardRequirement> cardRequirements = new List<CardRequirement>();

    private bool requirementsMetLastCheck = false;

    private void Start()
    {
        if (dropAreaManager != null)
        {
            // Set the button to its default tinted state
            SetButtonTint(true);
            InvokeRepeating(nameof(CheckCardCounts), 0, checkInterval);
        }

        // Add listener for button click
        optionButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(CheckCardCounts));
        optionButton.onClick.RemoveListener(OnButtonClick);
    }

    private void CheckCardCounts()
    {
        if (dropAreaManager != null)
        {
            Dictionary<string, int> cardCounts = dropAreaManager.GetCardCounts();
            bool allRequirementsMet = true;

            foreach (CardRequirement requirement in cardRequirements)
            {
                if (!cardCounts.ContainsKey(requirement.cardType) || cardCounts[requirement.cardType] < requirement.requiredCount)
                {
                    allRequirementsMet = false;
                    break;
                }
            }

            if (allRequirementsMet && !requirementsMetLastCheck)
            {
                Debug.Log("All requirements met for OptionCardButton!");
            }

            SetButtonTint(!allRequirementsMet);
            requirementsMetLastCheck = allRequirementsMet;
        }
    }

    private void SetButtonTint(bool tint)
    {
        ColorBlock colors = optionButton.colors;
        if (tint)
        {
            // Set to tinted transparent grey
            colors.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // RGBA(0.5, 0.5, 0.5, 0.5) - Transparent grey
            colors.highlightedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Same tint on highlight
            colors.pressedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Same tint when pressed
            colors.selectedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Same tint when selected
        }
        else
        {
            // Set to fully transparent
            colors.normalColor = new Color(1f, 1f, 1f, 0f); // RGBA(1, 1, 1, 0) - Fully transparent
            colors.highlightedColor = new Color(1f, 1f, 1f, 0f); // Fully transparent on highlight
            colors.pressedColor = new Color(1f, 1f, 1f, 0f); // Fully transparent when pressed
            colors.selectedColor = new Color(1f, 1f, 1f, 0f); // Fully transparent when selected
        }
        optionButton.colors = colors;
    }

    private void OnButtonClick()
    {
        if (requirementsMetLastCheck)
        {
            Debug.Log("Good to go");
        }
        else
        {
            Debug.Log("Insufficient resources");
        }
    }
}
