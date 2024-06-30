using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionCardButton : MonoBehaviour
{
    public string dropAreaTag = "DropArea";  // Tag of the DropArea GameObject
    public string drawCardsTag = "DrawCards"; // Tag of the DrawCards GameObject
    public Button optionButton;              // Reference to the button component
    public float checkInterval = 0.5f;       // Interval in seconds to check for updates
    private GameOverScreen gameOverScreen = new GameOverScreen();
    public int handCardLimit = 15;           // Maximum number of cards allowed in hand
    public Color borderColor = Color.black; // Color for the border when highlighted

    [System.Serializable]
    public class CardRequirement
    {
        public string cardType;
        public int requiredCount;
    }

    public List<CardRequirement> cardRequirements = new List<CardRequirement>();
    public List<GameObject> rewardList = new List<GameObject>();  // List of reward cards to be instantiated

    private bool requirementsMetLastCheck = false;
    private AreaManager dropAreaManager;
    private DrawCards drawCards;  // Reference to the DrawCards component

    private void Start()
    {
        // Find the DropArea GameObject by tag and get its AreaManager component
        GameObject dropArea = GameObject.FindGameObjectWithTag(dropAreaTag);
        if (dropArea != null)
        {
            dropAreaManager = dropArea.GetComponent<AreaManager>();
        }

        // Find the DrawCards GameObject by tag and get its DrawCards component
        GameObject drawCardsObject = GameObject.FindGameObjectWithTag(drawCardsTag);
        if (drawCardsObject != null)
        {
            drawCards = drawCardsObject.GetComponent<DrawCards>();
        }

        if (dropAreaManager != null)
        {
            // Set the button to its default state
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

            // Create a set of required card types for easy lookup
            HashSet<string> requiredCardTypes = new HashSet<string>();
            foreach (CardRequirement requirement in cardRequirements)
            {
                requiredCardTypes.Add(requirement.cardType);
                if (!cardCounts.ContainsKey(requirement.cardType) || cardCounts[requirement.cardType] != requirement.requiredCount)
                {
                    allRequirementsMet = false;
                }
            }

            // Ensure no additional cards are present
            foreach (string cardType in cardCounts.Keys)
            {
                if (!requiredCardTypes.Contains(cardType))
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
        Image buttonImage = optionButton.GetComponent<Image>();

        if (tint)
        {
            // Set to fully opaque
            buttonImage.color = new Color(1f, 1f, 1f, 1f); // RGBA(1, 1, 1, 1) - Fully opaque
            // Remove border
            RemoveButtonBorder(buttonImage);
        }
        else
        {
            // Set to fully opaque and add a border
            buttonImage.color = new Color(1f, 1f, 1f, 1f); // RGBA(1, 1, 1, 1) - Fully opaque
            AddButtonBorder(buttonImage);
        }
    }

    private void AddButtonBorder(Image buttonImage)
    {
        Outline outline = optionButton.GetComponent<Outline>();
        if (outline == null)
        {
            outline = optionButton.gameObject.AddComponent<Outline>();
        }
        outline.effectColor = borderColor;
        outline.effectDistance = new Vector2(5, 5);
    }

    private void RemoveButtonBorder(Image buttonImage)
    {
        Outline outline = optionButton.GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline);
        }
    }

    private void OnButtonClick()
    {
        GameObject dropArea = GameObject.FindGameObjectWithTag(dropAreaTag);
        if (requirementsMetLastCheck)
        {
            Debug.Log("Good to go");

            // Instantiate reward cards in the player's hand
            if (drawCards != null)
            {
                foreach (GameObject rewardCard in rewardList)
                {
                    GameObject playerCard = Instantiate(rewardCard, new Vector3(0, 0, 0), Quaternion.identity);
                    playerCard.transform.SetParent(drawCards.handArea.transform, false);
                }

                // Check the number of cards in the hand area
                Transform handTransform = drawCards.handArea.transform;
                if (handTransform.childCount > handCardLimit)
                {
                    Debug.Log("Hand exceeds limit. Removing random cards.");

                    // Get all cards in the hand area
                    List<Transform> handCards = new List<Transform>();
                    foreach (Transform child in handTransform)
                    {
                        handCards.Add(child);
                    }

                    // Randomly select 5 cards to destroy
                    for (int i = 0; i < 5; i++)
                    {
                        int randomIndex = Random.Range(0, handCards.Count);
                        Destroy(handCards[randomIndex].gameObject);
                        handCards.RemoveAt(randomIndex);
                    }
                }

                // Draw additional story card
                drawCards.DrawAdditionalStoryCard();

                foreach (Transform child in dropArea.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            else
            {
                Debug.LogError("DrawCards component not found!");
            }
        }
        else
        {
            Debug.Log("Insufficient resources");
        }
    }
}
