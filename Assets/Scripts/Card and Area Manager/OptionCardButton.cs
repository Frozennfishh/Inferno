using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class OptionCardButton : MonoBehaviour
{
    public string dropAreaTag = "DropArea";  // Tag of the DropArea GameObject
    public string drawCardsTag = "DrawCards"; // Tag of the DrawCards GameObject
    public Button optionButton;              // Reference to the button component
    public float checkInterval = 0.1f;       // Interval in seconds to check for updates
    //private GameOverScreen gameOverScreen = new GameOverScreen();
    public int handCardLimit = 15;           // Maximum number of cards allowed in hand
    public Color borderColor = Color.black; // Color for the border when highlighted
    public List<GameObject> InsertCard = new List<GameObject>();
    public List<GameObject> RemoveCard = new List<GameObject>();
    public GameOverScreen gameOverScreen;
    public AudioSource soundEffect;

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
        GameObject dropArea = GameObject.FindGameObjectWithTag("DropArea");
        if (dropArea != null)
        {
            dropAreaManager = dropArea.GetComponent<AreaManager>();
        } else
        {
            Debug.Log("Cannot find drop area");
        }

        // Find the DrawCards GameObject by tag and get its DrawCards component
        GameObject drawCardsObject = GameObject.FindGameObjectWithTag("DrawCards");
        if (drawCardsObject != null)
        {
            drawCards = drawCardsObject.GetComponent<DrawCards>();
        }
        else
        {
            Debug.Log("Cannot find Draw Cards");
        }

        if (dropAreaManager != null)
        {
            // Set the button to its default state        
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
        if (requirementsMetLastCheck)
        {
            Debug.Log("Good to go");
            if (soundEffect != null)
            {
                PlayAndDetachAudio(soundEffect);
            }

            if (this.CompareTag("Win Card"))
            {
                Debug.Log("Win!");
                SceneManager.LoadScene("Win Scene");
            }

            if (this.CompareTag("Lose Card"))
            {
                SceneManager.LoadScene("Game Over");
            }

            // Instantiate reward cards in the player's hand
            if (drawCards != null)
            {
                foreach (GameObject rewardCard in rewardList)
                {
                    GameObject playerCard = Instantiate(rewardCard, Vector3.zero, Quaternion.identity);
                    playerCard.transform.SetParent(drawCards.handArea.transform, false);
                }

                /*
                // Check the number of cards in the hand area
                int totalCardsInHand = drawCards.handArea.transform.childCount;
                Debug.Log(totalCardsInHand);
                if (totalCardsInHand > handCardLimit)
                {
                    Debug.Log("Hand exceeds limit. Removing random cards.");

                    // Duplicate the current hand of cards
                    List<GameObject> duplicateHand = new List<GameObject>();
                    foreach (Transform child in drawCards.handArea.transform)
                    {
                        duplicateHand.Add(child.gameObject);
                    }

                    // Randomly remove cards from the duplicate list
                    int cardsToRemove = 5;
                    for (int i = 0; i < cardsToRemove; i++)
                    {
                        if (duplicateHand.Count > 0)
                        {
                            int randomIndex = Random.Range(0, duplicateHand.Count);
                            Debug.Log($"Removing card: {duplicateHand[randomIndex].name}");
                            duplicateHand.RemoveAt(randomIndex);
                        }
                    }

                    // Clear all cards in the current hand area
                    for (int i = drawCards.handArea.transform.childCount - 1; i >= 0; i--)
                    {
                        Transform child = drawCards.handArea.transform.GetChild(i);
                        Destroy(child.gameObject);
                    }

                    // Re-add the remaining cards from the duplicate list into the hand area
                    foreach (GameObject card in duplicateHand)
                    {
                        GameObject playerCard = Instantiate(card, Vector3.zero, Quaternion.identity);
                        playerCard.transform.SetParent(drawCards.handArea.transform, false);
                    } 
                }*/

                // Insert and remove specific cards in the drawn story cards list
                foreach (GameObject card in RemoveCard)
                {
                    drawCards.deleteCardFromDrawnStoryDeck(card);
                }

                foreach (GameObject card in InsertCard)
                {
                    drawCards.addCardToDrawnStoryCards(card);
                }

                // Clear the drop area
                GameObject dropArea = GameObject.FindGameObjectWithTag(dropAreaTag);
                if (dropArea != null)
                {
                    foreach (Transform child in dropArea.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }


                // Draw an additional story card
                drawCards.DrawAdditionalStoryCard();
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

    public void PlayAndDetachAudio(AudioSource audioSource)
    {
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogWarning("AudioSource or AudioClip is null.");
            return;
        }

        // Create a new GameObject to play the audio
        GameObject audioObject = new GameObject("TempAudio");
        AudioSource tempAudioSource = audioObject.AddComponent<AudioSource>();

        // Copy the settings from the original AudioSource
        tempAudioSource.clip = audioSource.clip;
        tempAudioSource.volume = audioSource.volume;
        tempAudioSource.pitch = audioSource.pitch;
        tempAudioSource.spatialBlend = audioSource.spatialBlend;
        tempAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

        // Ensure the audio does not loop and play the clip
        tempAudioSource.loop = false;
        tempAudioSource.Play();

        // Destroy the new GameObject after the clip has finished playing
        Destroy(audioObject, tempAudioSource.clip.length);
    }
}
