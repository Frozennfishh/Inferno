using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    private Dictionary<string, int> cardCounts = new Dictionary<string, int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger Enter: {collision.gameObject.name} with tag {collision.tag}");
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

            Debug.Log($"Card entered: {cardType}. Updated counts:");
            PrintCardCounts();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"Trigger Exit: {collision.gameObject.name} with tag {collision.tag}");
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

                Debug.Log($"Card exited: {cardType}. Updated counts:");
                PrintCardCounts();
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
        return tag == "Stamina" || tag == "Strength" || tag == "Sin" || tag == "Soul" || tag == "Scraps" || tag == "Sanity"; // Add more tags as needed
    }

    private void PrintCardCounts()
    {
        string message = "Current card counts in area:\n";
        foreach (var entry in cardCounts)
        {
            message += $"{entry.Key}: {entry.Value}\n";
        }
        Debug.Log(message);
    }
}
