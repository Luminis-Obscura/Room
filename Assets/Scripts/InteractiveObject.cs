using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Base interface for all interactive objects
public interface IInteractable
{
    void OnInteract();
    bool CanInteract();
    string GetInteractionText();
    void OnStopInteract();
}

// Base class for all clickable objects in the game
public class InteractiveObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] protected string interactionText = "Interact";
    [SerializeField] protected bool isInteractable = true;
    [SerializeField] protected bool highlightOnHover = true;
    
    [Header("Events")]
    [SerializeField] protected UnityEvent onInteractEvent;
    [SerializeField] protected UnityEvent onEnterEvent;
    [SerializeField] protected UnityEvent onExitEvent;

    // Optional component references
    [Header("Optional References")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected Color originalColor;

    // State tracking
    protected bool isHovered = false;
    
    protected virtual void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        } else
        {
            Debug.LogWarning("SpriteRenderer not found on " + gameObject.name);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (CanInteract())
        {
            OnInteract();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (CanInteract())
        {
            if (highlightOnHover && spriteRenderer != null)
            {
                spriteRenderer.color = new Color(originalColor.r * 1.2f, originalColor.g * 1.2f, originalColor.b * 1.2f);
            }
            
            InteractiveManager.Instance.SetInteractiveCursor();
            
            if (GameManager.Instance.UIManager != null)
            {
                GameManager.Instance.UIManager.ShowInteractionText(interactionText);
            }
            onEnterEvent?.Invoke();
        }
    }

    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        
        InteractiveManager.Instance.SetDefaultCursor();
        
        if (GameManager.Instance.UIManager != null)
        {
            GameManager.Instance.UIManager.HideInteractionText();
        }

        onExitEvent?.Invoke();
    }

    public virtual void OnInteract()
    {
        onInteractEvent?.Invoke();
        GameEvents.TriggerInteraction(this);
    }

    public virtual bool CanInteract()
    {
        return isInteractable && InteractiveManager.Instance.AreInteractionsEnabled();
    }

    public virtual string GetInteractionText()
    {
        return interactionText;
    }

    public virtual void OnStopInteract()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        
        if (GameManager.Instance.UIManager != null)
        {
            GameManager.Instance.UIManager.HideInteractionText();
        }
    }

    public virtual void SetInteractable(bool state)
    {
        isInteractable = state;
        if (!isInteractable && isHovered && spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
            GameManager.Instance.UIManager.HideInteractionText();
        }
    }

    public void SetSpriteRenderer(SpriteRenderer newSprite)
    {
        if (newSprite != null)
        {
            spriteRenderer = newSprite;
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning("New SpriteRenderer is null");
        }
    }
}