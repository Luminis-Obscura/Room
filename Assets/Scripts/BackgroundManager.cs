using UnityEngine;

[RequireComponent(typeof(InteractiveItem))]
public class BackgroundManager : MonoBehaviour
{
    [Header("Background Settings")]
    [SerializeField] private SpriteRenderer[] backgrounds;
    [SerializeField] private AudioClip[] audioClips; // Array of audio clips for each background

    [Header("Click Sound Settings")]
    [SerializeField] private AudioClip clickSound; // Sound to play on each click

    private SpriteRenderer currentBackground;
    private InteractiveItem interactiveItem;
    private AudioSource audioSource;

    void Start()
    {
        interactiveItem = GetComponent<InteractiveItem>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("[BackgroundManager] Missing AudioSource component!");
            enabled = false;
            return;
        }

        if (backgrounds.Length == 0)
        {
            Debug.LogError("No backgrounds assigned to BackgroundManager on " + gameObject.name);
            return;
        }

        if (audioClips.Length != backgrounds.Length)
        {
            Debug.LogError("The number of audio clips must match the number of backgrounds in BackgroundManager!");
            return;
        }

        // Set the default volume to 0.5
        audioSource.volume = 0.5f;
    }

    public void RandomizeBackground()
    {
        // Check if there are enough backgrounds to randomize
        if (backgrounds.Length <= 1)
        {
            Debug.LogWarning("Not enough backgrounds to randomize!");
            return;
        }

        // Play the click sound
        PlayClickSound();

        // Deactivate the current background
        if (currentBackground != null)
        {
            Debug.Log($"Deactivating current background: {currentBackground.name}");
            currentBackground.gameObject.SetActive(false);
        }

        // Ensure the new background is different from the current one
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, backgrounds.Length);
        } while (backgrounds[randomIndex] == currentBackground);

        // Activate the new background
        currentBackground = backgrounds[randomIndex];
        Debug.Log($"Activating new background: {currentBackground.name}");
        currentBackground.gameObject.SetActive(true);

        // Play the corresponding audio clip
        PlayBackgroundSound(randomIndex);

        // Update the interactive item sprite renderer
        interactiveItem.SetSpriteRenderer(currentBackground);
    }


    private void PlayBackgroundSound(int index)
    {
        if (audioClips[index] != null && audioSource != null)
        {
            Debug.Log($"Playing sound for background {index}: {audioClips[index].name}");
            audioSource.PlayOneShot(audioClips[index]);
        }
        else
        {
            Debug.LogWarning($"No audio clip assigned for background {index} or AudioSource is missing.");
        }
    }

    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            Debug.Log("Playing click sound.");
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Click sound is not assigned or AudioSource is missing.");
        }
    }
}

