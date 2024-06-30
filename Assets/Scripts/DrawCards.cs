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

    private List<GameObject> playerDeck = new List<GameObject>();
    public List<GameObject> storyDeck = new List<GameObject>();
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
            GameObject playerCard = Instantiate(playerDeck[k], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(handArea.transform, false);
        }

        GameObject playerCardExtra = Instantiate(playerDeck[5], new Vector3(0, 0, 0), Quaternion.identity);
        playerCardExtra.transform.SetParent(handArea.transform, false);

        // Draw the initial story card
        DrawStoryCard();
    }

    public void onClick()
    {
        DrawAdditionalStoryCard();
        GameObject playerCardExtra = Instantiate(playerDeck[5], new Vector3(0, 0, 0), Quaternion.identity);
        playerCardExtra.transform.SetParent(handArea.transform, false);
    }

    private void DrawStoryCard()
    {
        if (remainingStoryDeck.Count == 0)
        {
            Debug.Log("No more cards in the story deck! Replenishing RemainingStoryDeck");
            // Replenish remainingStoryDeck with drawnStoryCards and reset drawnStoryCards
            remainingStoryDeck.AddRange(drawnStoryCards);
            drawnStoryCards.Clear();
        }

        if (remainingStoryDeck.Count > 0)
        {
            int randomIndex = Random.Range(0, remainingStoryDeck.Count);
            GameObject storyCard = Instantiate(remainingStoryDeck[randomIndex], new Vector3(0, 0, 0), Quaternion.identity);
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

            // Add the drawn card to the drawnStoryCards list
            drawnStoryCards.Add(remainingStoryDeck[randomIndex]);

            // Remove the drawn card from the remaining deck
            remainingStoryDeck.RemoveAt(randomIndex);
        }
        else
        {
            Debug.Log("No more cards in the story deck! BROK");
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

        // Draw a new story card
        DrawStoryCard();
    }

    public List<GameObject> GetRemainingStoryDeck()
    {
        return new List<GameObject>(remainingStoryDeck); // Return a copy of the remaining deck
    }

    public List<GameObject> GetDrawnStoryCards()
    {
        return new List<GameObject>(drawnStoryCards); // Return a copy of the drawn cards
    }
}
