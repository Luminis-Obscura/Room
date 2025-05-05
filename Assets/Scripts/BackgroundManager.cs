using UnityEngine;

[RequireComponent(typeof(InteractiveItem))]
public class BackgroundManager : MonoBehaviour
{

    [SerializeField] private SpriteRenderer[] backgrounds;
    private SpriteRenderer currentBackground;
    private InteractiveItem interactiveItem;

    void Start()
    {
        interactiveItem = GetComponent<InteractiveItem>();
        if (backgrounds.Length == 0)
        {
            Debug.LogError("No backgrounds assigned to BackgroundManager on " + gameObject.name);
            return;
        }
    }

    public void RandomizeBackground()
    {
        // Deactivate the current background
        if (currentBackground != null) currentBackground.gameObject.SetActive(false);
        
        // Randomly select a new background
        int randomIndex = Random.Range(0, backgrounds.Length);
        while (backgrounds[randomIndex] == currentBackground)
        {
            randomIndex = Random.Range(0, backgrounds.Length);
        }
        
        // Activate the new background
        currentBackground = backgrounds[randomIndex];
        currentBackground.gameObject.SetActive(true);
        
        // Update the interactive item sprite renderer
        interactiveItem.SetSpriteRenderer(currentBackground);
    }
}
