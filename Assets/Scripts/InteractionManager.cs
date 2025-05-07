using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private List<GameObject> interactions;
    [SerializeField] private List<AudioClip> randomAudioClips; // List of 3 random audio clips
    [SerializeField] private List<AudioClip> interactionAudioClips; // List of audio clips corresponding to interactions
    [SerializeField] private float chancesForNextInteraction = 0.5f;
    [SerializeField] private bool diminishChances = true;

    [Header("Timing Settings")]
    [SerializeField] private float interactionProtectionTimer = 0.1f;
    [SerializeField] private float interactionLastTimer = 2f;
    [SerializeField] private float interactionCooldown = 0.5f;
    [SerializeField] private float minRespondDelay = 0.5f;
    [SerializeField] private float maxRespondDelay = 2f;

    private float originalChance;
    private bool canShout = true;

    private AudioSource _audioSource;
    private Dictionary<GameObject, Coroutine> activeDeactivationCoroutines = new Dictionary<GameObject, Coroutine>();
    private Dictionary<AudioClip, AudioSource> activeInteractionAudioSources = new Dictionary<AudioClip, AudioSource>();

    private void Start()
    {
        originalChance = chancesForNextInteraction;
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("[InteractionManager] Missing AudioSource component!");
        }
        else
        {
            _audioSource.volume = 0.7f; // 设置默认音量
        }
    }

    public void Shout()
    {
        if (canShout)
        {
            PlayRandomAudio(); // Play a random audio clip
            StartCoroutine(ShoutCoroutine());
        }
    }

    private void PlayRandomAudio()
    {
        if (randomAudioClips == null || randomAudioClips.Count == 0)
        {
            Debug.LogError("[InteractionManager] No random audio clips assigned!");
            return;
        }

        int randomIndex = Random.Range(0, randomAudioClips.Count);
        _audioSource.PlayOneShot(randomAudioClips[randomIndex]);
    }

    private IEnumerator ShoutCoroutine()
    {
        canShout = false;

        // Initial random delay
        yield return StartCoroutine(RandomDelay());

        do
        {
            // Activate a random interaction
            ActivateRandomInteraction();

            // Protection timer
            yield return new WaitForSeconds(interactionProtectionTimer);

            // Cooldown timer
            StartCoroutine(ResetCooldown());

            // Check if we should continue with diminishing chances
            if (Random.value > chancesForNextInteraction)
            {
                break;
            }

            if (diminishChances)
            {
                chancesForNextInteraction *= 0.5f;
            }

        } while (true);

        yield return new WaitForSeconds(interactionCooldown);
        chancesForNextInteraction = originalChance;
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(interactionCooldown);
        canShout = true;
    }

    private IEnumerator RandomDelay()
    {
        float randomDelay = Random.Range(minRespondDelay, maxRespondDelay);
        yield return new WaitForSeconds(randomDelay);
    }

    void ActivateRandomInteraction()
    {
        if (interactions == null || interactions.Count == 0)
        {
            Debug.LogError("No interactions available");
            return;
        }

        int randomIndex = Random.Range(0, interactions.Count);
        ActivateInteraction(randomIndex);
    }

    void ActivateInteraction(int index)
    {
        if (index < 0 || index >= interactions.Count)
        {
            Debug.LogError("Index out of range");
            return;
        }

        GameObject interaction = interactions[index];

        // Cancel existing deactivation coroutine if it exists
        if (activeDeactivationCoroutines.TryGetValue(interaction, out Coroutine existingCoroutine))
        {
            if (existingCoroutine != null)
                StopCoroutine(existingCoroutine);
        }

        // Activate the interaction
        interaction.SetActive(true);

        // Play the corresponding audio clip
        PlayInteractionAudio(index);

        // Start new deactivation coroutine
        Coroutine deactivationCoroutine = StartCoroutine(DeactivateAfterDelay(interaction));
        activeDeactivationCoroutines[interaction] = deactivationCoroutine;
    }

    private void PlayInteractionAudio(int index)
    {

        if (interactionAudioClips == null || index < 0 || index >= interactionAudioClips.Count)
        {
            Debug.LogError("[InteractionManager] No audio clip assigned for interaction at index " + index);
            return;
        }

        AudioClip clipToPlay = interactionAudioClips[index];

        // 如果已有该clip在播放，停止旧的并销毁其GameObject
        if (activeInteractionAudioSources.TryGetValue(clipToPlay, out AudioSource existingSource))
        {
            if (existingSource != null)
            {
                existingSource.Stop();
                Destroy(existingSource.gameObject);
            }
            activeInteractionAudioSources.Remove(clipToPlay);
        }

        // 创建临时AudioSource用于播放
        GameObject tempAudioGO = new GameObject("TempAudio_" + clipToPlay.name);
        tempAudioGO.transform.SetParent(this.transform); // 可选：父对象为Manager
        AudioSource newSource = tempAudioGO.AddComponent<AudioSource>();
        newSource.clip = clipToPlay;
        newSource.volume = 0.7f; 
        newSource.Play();

        activeInteractionAudioSources[clipToPlay] = newSource;

        // 播放完自动销毁
        StartCoroutine(DestroyAfterPlayback(tempAudioGO, clipToPlay.length));
    }

    private IEnumerator DestroyAfterPlayback(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }

    private IEnumerator DeactivateAfterDelay(GameObject interaction)
    {
        yield return new WaitForSeconds(interactionLastTimer);
        interaction.SetActive(false);
        activeDeactivationCoroutines.Remove(interaction);
    }
}
