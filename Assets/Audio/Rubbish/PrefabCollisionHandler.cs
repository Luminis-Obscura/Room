using UnityEngine;

public class PrefabCollisionHandler : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip collisionSound; // Sound for general collisions
    [SerializeField] private AudioClip areaEnterSound; // Sound for entering a specific area

    private AudioSource _audioSource;
    private float _lastSoundTime; // Tracks the last time the sound was played
    private const float SoundCooldown = 1f; // Cooldown duration in seconds

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("[PrefabCollisionHandler] Missing AudioSource component!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Play collision sound if enough time has passed
        if (Time.time - _lastSoundTime >= SoundCooldown)
        {
            PlaySound(collisionSound);
            _lastSoundTime = Time.time; // Update the last sound time
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entered the specific area
        if (other.CompareTag("SpecialArea"))
        {
            PlaySound(areaEnterSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}
