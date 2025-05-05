using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> interactions;
    [SerializeField] private float chancesForNextInteraction = 0.5f; 
    [SerializeField] private bool diminishChances = true; 

    [SerializeField] private float interactionProtectionTimer = 0.1f;
    [SerializeField] private float interactionLastTimer = 2f;
    [SerializeField] private float interactionCooldown = 0.5f;

    [SerializeField] private float minRespondDelay = 0.5f;
    [SerializeField] private float maxRespondDelay = 2f;

    private float originalChance;
    private bool canShout = true;

    private Dictionary<GameObject, Coroutine> activeDeactivationCoroutines = new Dictionary<GameObject, Coroutine>();

    private void Start()
    {
        originalChance = chancesForNextInteraction;
    }

    public void Shout()
    {
        if (canShout)
        {
            StartCoroutine(ShoutCoroutine());
        }
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
        // Reset chance for next time
        chancesForNextInteraction = originalChance;
    }
    
    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(interactionCooldown);
        canShout = true;
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
        
        // Start new deactivation coroutine
        Coroutine deactivationCoroutine = StartCoroutine(DeactivateAfterDelay(interaction));
        activeDeactivationCoroutines[interaction] = deactivationCoroutine;
    }

    private IEnumerator DeactivateAfterDelay(GameObject interaction)
    {
        yield return new WaitForSeconds(interactionLastTimer);
        interaction.SetActive(false);
        activeDeactivationCoroutines.Remove(interaction);
    }

    IEnumerator RandomDelay()
    {
        float randomDelay = Random.Range(minRespondDelay, maxRespondDelay);
        yield return new WaitForSeconds(randomDelay);
    }
}