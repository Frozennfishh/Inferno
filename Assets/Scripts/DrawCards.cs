using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour
{
    public GameObject storyCard1;
    public GameObject storyCard2;
    public GameObject card1;
    public GameObject handArea;
    public GameObject storyArea;

    List<GameObject> playerDeck = new List<GameObject>();
    List<GameObject> storyDeck = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        storyDeck.Add(storyCard1);
        storyDeck.Add(storyCard2);

        playerDeck.Add(card1);
    }

    public void onClick()
    {
        for (int i = 0; i < playerDeck.Count;  i++) 
        {
            for (int k = 0; k < 2; k++) {
                GameObject playerCard = Instantiate(playerDeck[i], new Vector3(0, 0, 0), Quaternion.identity);
                playerCard.transform.SetParent(handArea.transform, false);
            }
        }

        GameObject storyCard = Instantiate(storyDeck[Random.Range(0,storyDeck.Count)], new Vector3(0, 0, 0), Quaternion.identity);
        storyCard.transform.SetParent(storyArea.transform, false);
    }
}
