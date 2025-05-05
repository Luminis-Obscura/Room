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
        // Radomly select a background from the array to be active deacivate all others
        int randomIndex = Random.Range(0, backgrounds.Length);
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].gameObject.SetActive(i == randomIndex);
        }
        
        currentBackground = backgrounds[randomIndex];
        // interactiveItem.spriteRenderer = currentBackground;
        interactiveItem.SetSpriteRenderer(currentBackground);
    }
}
