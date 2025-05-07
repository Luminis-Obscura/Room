using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

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

    [Header("Audio Clips")]
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;
    [SerializeField] private AudioClip chewingClip;

    private int _health = 3;
    private bool _isMouseOpen;
    private AudioSource _audioSource;           // 主 AudioSource，用于 PlayOneShot
    private AudioSource _chewingAudioSource;    // chewing 专属 AudioSource
    private Animator animator;
    private Coroutine _chewingCoroutine;

    public GloveState CurrentState
    {
        get => (GloveState)_health;
        private set
        {
            _health = (int)value;
            PlayHurtAudio();
            PlayHurtAnimation();
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

                if (_isMouseOpen)
                {
                    PlayOpenAudio();
                    StopChewingAudio();
                }
                else
                {
                    PlayCloseAudio();
                    StartChewingAudio();
                }

                UpdateVisuals();
            }
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("[GloveController] Missing main AudioSource component!");
        }
        else
        {
            _audioSource.volume = 0.7f;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("[GloveController] Missing Animator component!");
        }


        // 创建 chewing 专用 AudioSource
        _chewingAudioSource = gameObject.AddComponent<AudioSource>();
        _chewingAudioSource.playOnAwake = false;
        _chewingAudioSource.volume = 0.7f;
        _chewingAudioSource.loop = false;

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
            IsMouseOpen = false;
            Debug.Log("Glove is dead");
        }

        CurrentState = newState;
        Debug.Log($"Glove state changed to: {CurrentState}");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Opponent") && CurrentState != GloveState.Dead && !IsMouseOpen)
        {
            TakeDamage(1);
        }
    }

    private void UpdateVisuals()
    {
        if (openMouseSprite != null && closedMouseSprite != null)
        {
            openMouseSprite.enabled = IsMouseOpen;
            closedMouseSprite.enabled = !IsMouseOpen;
        }

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
        if (eyeSprites != null && eyeSprites.Length != 4)
        {
            Debug.LogWarning("Eye sprites array should have exactly 4 elements (one for each state)");
        }
    }

    private void PlayHurtAnimation()
    {
        if (animator != null)
        {
            animator.Play("Knockback");
        }
    }

    private void PlayHurtAudio()
    {
        if (hurtClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(hurtClip);
        }
    }

    private void PlayOpenAudio()
    {
        if (openClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(openClip);
        }
    }

    private void PlayCloseAudio()
    {
        if (closeClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(closeClip);
        }
    }

    private void StartChewingAudio()
    {
        if (chewingClip != null && _chewingCoroutine == null)
        {
            _chewingCoroutine = StartCoroutine(PlayChewingAfterDelay());
        }
    }

    private void StopChewingAudio()
    {
        if (_chewingCoroutine != null)
        {
            StopCoroutine(_chewingCoroutine);
            _chewingCoroutine = null;
        }

        if (_chewingAudioSource != null && _chewingAudioSource.isPlaying)
        {
            _chewingAudioSource.Stop();
        }
    }

    private IEnumerator PlayChewingAfterDelay()
    {
        yield return new WaitForSeconds(closeClip != null ? closeClip.length : 0.5f);

        if (chewingClip != null && _chewingAudioSource != null)
        {
            _chewingAudioSource.clip = chewingClip;
            _chewingAudioSource.volume = 0.7f;
            _chewingAudioSource.loop = false;
            _chewingAudioSource.Play();
        }

        _chewingCoroutine = null;
    }
}
