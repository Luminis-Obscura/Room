using System;
using System.Collections;
using UnityEngine;

public enum GloveState
{
    Normal = 3, 
    Angry = 2,
    Sad = 1,
    Dead = 0
}

public class GloveController : InteractiveObject
{
    [SerializeField] private SpriteRenderer openMouseSprite;
    [SerializeField] private SpriteRenderer closedMouseSprite;
    [SerializeField] private SpriteRenderer[] eyeSprites;  // Array of eye sprites for different states
    [SerializeField] private float mouseReopenDelay = 1f;

    private int _health = 3;
    private bool _isMouseOpen;

    public GloveState CurrentState
    {
        get => (GloveState)_health;
        private set
        {
            _health = (int)value;
            UpdateVisuals();
        }
    }

    public bool IsMouseOpen
    {
        get => _isMouseOpen;
        private set
        {
            if (_isMouseOpen != value)
            {
                _isMouseOpen = value;
                UpdateVisuals();
            }
        }
    }

    private void Start()
    {
        UpdateVisuals();
    }

    public override void OnInteract()
    {
        base.OnInteract();
        Debug.Log("Interacted with glove");

        if (!IsMouseOpen && CurrentState != GloveState.Dead)
        {
            IsMouseOpen = true;
            StartCoroutine(ReopenMouseAfterDelay(mouseReopenDelay));
        }
    }

    private IEnumerator ReopenMouseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (CurrentState != GloveState.Dead)
        {
            IsMouseOpen = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (CurrentState == GloveState.Dead) return;

        _health = Mathf.Max(0, _health - damage);
        var newState = (GloveState)_health;
        
        if (newState == GloveState.Dead)
        {
            IsMouseOpen = false;  // Force mouse closed when dead
            Debug.Log("Glove is dead");
        }
        
        CurrentState = newState;
        Debug.Log($"Glove state changed to: {CurrentState}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsMouseOpen && CurrentState != GloveState.Dead)
        {
            Debug.Log("Mouse is not open");
            TakeDamage(1);
        }
    }

    private void UpdateVisuals()
    {
        // Update mouse sprites
        if (openMouseSprite != null && closedMouseSprite != null)
        {
            openMouseSprite.enabled = IsMouseOpen;
            closedMouseSprite.enabled = !IsMouseOpen;
        }

        // Update eye sprites based on current state
        if (eyeSprites != null && eyeSprites.Length > 0)
        {
            for (int i = 0; i < eyeSprites.Length; i++)
            {
                if (eyeSprites[i] != null)
                {
                    eyeSprites[i].enabled = (i == (int)CurrentState);
                }
            }
        }
    }

    private void OnValidate()
    {
        // Editor-only validation
        if (eyeSprites != null && eyeSprites.Length != 4)
        {
            Debug.LogWarning("Eye sprites array should have exactly 4 elements (one for each state)");
        }
    }
}