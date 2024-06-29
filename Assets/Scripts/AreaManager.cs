using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    private Dictionary<string, int> cardCounts = new Dictionary<string, int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsResourceCard(collision.tag))
        {
            string cardType = collision.tag;

            if (cardCounts.ContainsKey(cardType))
            {
                cardCounts[cardType]++;
            }
            else
            {
                cardCounts[cardType] = 1;
            }

            Debug.Log($"Card entered: {cardType}. Count: {cardCounts[cardType]}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsResourceCard(collision.tag))
        {
            string cardType = collision.tag;

            if (cardCounts.ContainsKey(cardType))
            {
                cardCounts[cardType]--;

                if (cardCounts[cardType] <= 0)
                {
                    cardCounts.Remove(cardType);
                }

                Debug.Log($"Card exited: {cardType}. Count: {(cardCounts.ContainsKey(cardType) ? cardCounts[cardType] : 0)}");
            }
        }
    }

    public int GetCardCount(string cardType)
    {
        if (cardCounts.ContainsKey(cardType))
        {
            return cardCounts[cardType];
        }

        return 0;
    }

    private bool IsResourceCard(string tag)
    {
        // Add all possible resource tags here
        return tag == "Stamina" || tag == "Strength" || tag == "Soul" || tag == "Sin" || tag == "Scraps" || tag == "Sanity"; // Add more tags as needed
    }
}
