using UnityEngine;

public class ChatController : MonoBehaviour
{
    private Animator animator;
    private AudioSource _audioSource;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip animationStartSound;
    [SerializeField] private AudioClip animationEndSound;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (animator == null)
        {
            Debug.LogError("[ChatController] Animator component not found on " + gameObject.name);
        }

        if (_audioSource == null)
        {
            Debug.LogError("[ChatController] AudioSource component not found on " + gameObject.name);
        }
    }

    public virtual void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            PlayAnimationStartSound(); // Play sound when animation starts
            animator.Play(animationName);
        }
        else
        {
            Debug.LogError("[ChatController] Animator component not found on " + gameObject.name);
        }
    }

    // This method can be called via an Animation Event at the end of the animation
    public void OnAnimationEnd()
    {
        PlayAnimationEndSound();
    }

    private void PlayAnimationStartSound()
    {
        if (animationStartSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(animationStartSound);
        }
    }

    private void PlayAnimationEndSound()
    {
        if (animationEndSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(animationEndSound);
        }
    }
}
