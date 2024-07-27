using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCards : MonoBehaviour
{
    public GameObject card1; //Soul
    public GameObject card2; //Stamina
    public GameObject card3; //Scraps
    public GameObject card4; //Sin
    public GameObject card5; //Strength
    public GameObject card6; //Sanity
    public GameObject handArea;
    public GameObject storyArea;
    public GameObject optionArea;
    private AreaManager areaManager;

    private List<GameObject> playerDeck = new List<GameObject>();
    public List<GameObject> storyDeck = new List<GameObject>();
    public List<GameObject> triggerDeck = new List<GameObject>();
    private List<GameObject> remainingStoryDeck = new List<GameObject>();
    private List<GameObject> drawnStoryCards = new List<GameObject>(); // List to keep track of drawn cards

    // Start is called before the first frame update
    void Start()
    {
        remainingStoryDeck.AddRange(storyDeck);

        playerDeck.Add(card1);
        playerDeck.Add(card2);
        playerDeck.Add(card3);
        playerDeck.Add(card4);
        playerDeck.Add(card5);
        playerDeck.Add(card6);

        // Instatiate player starting hand
        for (int k = 0; k < playerDeck.Count; k++)
        {
            GameObject playerCard = Instantiate(playerDeck[k], new Vector3(0, 0, 0),
                Quaternion.identity);
            playerCard.transform.SetParent(handArea.transform, false);
        }

        GameObject playerCardExtra = Instantiate(playerDeck[5], new Vector3(0, 0, 0), Quaternion.identity);
        playerCardExtra.transform.SetParent(handArea.transform, false);

        // Draw the initial story card
        DrawStoryCard();
    }

    public void onClick()
    {
        // Proceed to draw a new story card
        DrawAdditionalStoryCard();
    }

    // Helper method to convert the dictionary to a list of card prefabs
    private List<GameObject> ConvertDictionaryToCardList(Dictionary<string, int> cardCounts)
    {
        List<GameObject> cardList = new List<GameObject>();

        foreach (KeyValuePair<string, int> entry in cardCounts)
        {
            string cardTag = entry.Key;
            int count = entry.Value;

            GameObject cardPrefab = GetCardPrefabByTag(cardTag);

            for (int i = 0; i < count; i++)
            {
                cardList.Add(cardPrefab);
            }
        }

        return cardList;
    }

    // Helper method to clear the current hand area
    private void ClearHandArea()
    {
        foreach (Transform child in handArea.transform)
        {
            Destroy(child.gameObject);
        }

        //Debug.Log($"Hand area cleared. Remaining child count: {handArea.transform.childCount}");
    }

    private GameObject GetCardPrefabByTag(string tag)
    {
        switch (tag)
        {
            case "Soul":
                return card1;
            case "Stamina":
                return card2;
            case "Scraps":
                return card3;
            case "Sin":
                return card4;
            case "Strength":
                return card5;
            case "Sanity":
                return card6;
            default:
                Debug.LogError("Unknown card tag: " + tag);
                return null;
        }
    }

    public Dictionary<string, int> getCardCounts()
    {
        Dictionary<string, int> cardCounts = new Dictionary<string, int>();

        foreach (Transform card in handArea.transform)
        {
            string cardTag = card.tag;

            if (cardCounts.ContainsKey(cardTag))
            {
                cardCounts[cardTag]++;
            }
            else
            {
                cardCounts[cardTag] = 1;
            }
        }

        return cardCounts;
    }

    private void DrawStoryCard()
    {
        if (remainingStoryDeck.Count == 0)
        {
            
            Debug.Log("No more cards in the story deck! Replenishing RemainingStoryDeck");

            // Check if drawnStoryCards has cards to replenish
            if (drawnStoryCards.Count > 0)
            {
                // Replenish remainingStoryDeck with drawnStoryCards and reset drawnStoryCards
                remainingStoryDeck.AddRange(drawnStoryCards);
                drawnStoryCards.Clear();
            }
            else
            {
                Debug.LogWarning("drawnStoryCards is also empty. No cards to replenish!");
            }
        } 

        

        if (remainingStoryDeck.Count > 0)
            {
                int randomIndex = Random.Range(0, remainingStoryDeck.Count);
                GameObject cardPrefab = remainingStoryDeck[randomIndex];

                if (cardPrefab != null)
                {
                    GameObject storyCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    storyCard.transform.SetParent(storyArea.transform, false);

                    // Assign the option area to the instantiated story card
                    StoryCard storyCardScript = storyCard.GetComponent<StoryCard>();
                    if (storyCardScript != null)
                    {
                        storyCardScript.optionArea = optionArea;
                    }
                    else
                    {
                        Debug.LogError("StoryCard script not found on instantiated story card!");
                    }


                    if (storyCard.CompareTag("Once Only Card"))
                    {
                        deleteCardFromRemainingStoryDeck(cardPrefab);
                    }
                    else
                    {
                        // Add the prefab (not the instantiated object) to the drawnStoryCards list
                        addCardToDrawnStoryCards(cardPrefab);

                        // Remove the prefab from the remaining deck
                        deleteCardFromRemainingStoryDeck(cardPrefab);
                    }
                }
                else
                {
                    Debug.LogError("Card prefab is null!");
                }
            }
            else
            {
                Debug.Log("No more cards in the story deck! BROK");
            }

            //Reorg Hand

            // Retrieve the dictionary of cards in the hand area
            Dictionary<string, int> cardCounts = getCardCounts();

            // Convert the dictionary into a list of card prefabs
            List<GameObject> newHand = ConvertDictionaryToCardList(cardCounts);

            // Clear the current hand area
            ClearHandArea();

            // Re-instantiate all cards in the new list into the hand area
            foreach (GameObject cardPrefab in newHand)
            {
                GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
                card.transform.SetParent(handArea.transform, false);
            }
    }

    public void DrawAdditionalStoryCard()
    {
        // Clear the story area of the previous card
        foreach (Transform child in storyArea.transform)
        {
            Destroy(child.gameObject);
        }

        // Clear the option area of the previous cards
        foreach (Transform child in optionArea.transform)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, int> cardCounts = getCardCounts();

        int sum = 0;
        foreach (var entry in cardCounts)
        {
            sum += entry.Value;
        }

        //Hoarder Trigger: 15 Cards or more
        if (sum >= 15)
        {
            instantiateTriggerCard(0);
        }
        //Paranoia Trigger: 0 Sanity
        else if (!cardCounts.ContainsKey("Sanity"))
        {
            instantiateTriggerCard(1);
        }
        //Deal with the Devil Trigger: 3 Sin
        else if (cardCounts.ContainsKey("Sin") && cardCounts["Sin"] == 3)
        {
            instantiateTriggerCard(2);
        }
        //Repentance Trigger: 4 or more Sin
        else if (cardCounts.ContainsKey("Sin") && cardCounts["Sin"] >= 4)
        {
            instantiateTriggerCard(3);
        }
        //Last Breath Trigger: 0 Strength
        else if (!cardCounts.ContainsKey("Strength"))
        {
            instantiateTriggerCard(4);
        }
        //Greed Trigger: 5 or more Scraps
        else if (cardCounts.ContainsKey("Scraps") && cardCounts["Scraps"] >= 5)
        {
            instantiateTriggerCard(5);
        }

        /*if (cardCounts.ContainsKey("Soul") && cardCounts["Soul"] >= 3)
        {
            // Trigger event for having 3 or more Soul cards
        }*/

                // Draw a new story card
        else
        {
            DrawStoryCard();
        }
    }

    public int GetRemainingStoryDeckCount()
    {
        return remainingStoryDeck.Count; // Return a copy of the remaining deck
    }

    public int GetDrawnStoryCardsCount()
    {
        return drawnStoryCards.Count; // Return a copy of the drawn cards
    }

    public void addCardToDrawnStoryCards(GameObject card)
    {
        drawnStoryCards.Add(card);
    }

    public void deleteCardFromRemainingStoryDeck(GameObject card)
    {
        remainingStoryDeck.Remove(card);
    }

    public void deleteCardFromDrawnStoryDeck(GameObject card)
    {
        drawnStoryCards.Remove(card);
    }

    private void reorgHand()
    {
        //Reorg Hand

        // Retrieve the dictionary of cards in the hand area
        Dictionary<string, int> tempCardCounts = getCardCounts();

        // Convert the dictionary into a list of card prefabs
        List<GameObject> newHand = ConvertDictionaryToCardList(tempCardCounts);

        // Clear the current hand area
        ClearHandArea();

        // Re-instantiate all cards in the new list into the hand area
        foreach (GameObject cardPrefab in newHand)
        {
            GameObject card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
            card.transform.SetParent(handArea.transform, false);
        }
    }

    private void instantiateTriggerCard(int i)
    {
        reorgHand();

        GameObject storyCard = Instantiate(triggerDeck[i], new Vector3(0, 0, 0), Quaternion.identity);
        storyCard.transform.SetParent(storyArea.transform, false);

        // Assign the option area to the instantiated story card
        StoryCard storyCardScript = storyCard.GetComponent<StoryCard>();
        if (storyCardScript != null)
        {
            storyCardScript.optionArea = optionArea;
        }
        else
        {
            Debug.LogError("StoryCard script not found on instantiated story card!");
        }
    }
}
