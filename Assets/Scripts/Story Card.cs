using UnityEngine;

public class StoryCard : MonoBehaviour
{
    public GameObject optionCard1;
    public GameObject optionCard2;
    public GameObject optionCard3;
    public GameObject optionArea;

    // This method is called when the StoryCard is instantiated
    private void Start()
    {
        SpawnOptionCards();
    }

    private void SpawnOptionCards()
    {
        if (optionArea != null)
        {
            // Instantiate the option cards and set their parent to the option area
            Instantiate(optionCard1, optionArea.transform);
            Instantiate(optionCard2, optionArea.transform);
            Instantiate(optionCard3, optionArea.transform);
        }
        else
        {
            Debug.LogError("OptionArea is not assigned!");
        }
    }
}
