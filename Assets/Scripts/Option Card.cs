using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionCard : MonoBehaviour
{
    public GameObject dropArea;
    public int requiredTypesCount = 1; // Number of different types required to highlight
    public Color highlightColor = Color.yellow; // Color to highlight the OptionCard
    private Image optionCardImage; // Image component to change the color
    private AreaManager areaManager; // Reference to the AreaManager script in the DropArea

    private void Start()
    {
        if (dropArea == null)
        {
            Debug.LogError("DropArea is not assigned!");
            return;
        }

        areaManager = dropArea.GetComponent<AreaManager>();
        if (areaManager == null)
        {
            Debug.LogError("DropArea does not have an AreaManager component!");
            return;
        }

        optionCardImage = GetComponent<Image>();
        if (optionCardImage == null)
        {
            Debug.LogError("OptionCard does not have an Image component!");
            return;
        }
    }

    private void Update()
    {
        if (areaManager != null)
        {
            CheckHighlightCondition();
        }
    }

    private void CheckHighlightCondition()
    {
        int typesCount = 0;
        List<string> resourceTypes = new List<string> { "Stamina", "Strength", "Sin", "Soul", "Scraps", "Sanity" }; // List of resource types

        foreach (string resourceType in resourceTypes)
        {
            if (areaManager.GetCardCount(resourceType) > 0)
            {
                typesCount++;
            }
        }

        if (typesCount >= requiredTypesCount)
        {
            HighlightOptionCard();
        }
        else
        {
            RemoveHighlight();
        }
    }

    private void HighlightOptionCard()
    {
        optionCardImage.color = highlightColor;
    }

    private void RemoveHighlight()
    {
        optionCardImage.color = Color.white; // Default color
    }
}
