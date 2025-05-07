using System.Collections.Generic;
using UnityEngine;

public class ThrowGenerator : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private float baseForce = 5f;
    [SerializeField] private float maxForceMultiplier = 2f;

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownDuration = 0.5f;
    [SerializeField] private bool showCooldownDebug;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip spawnSound;

    private float _nextSpawnTime;
    private Camera _mainCamera;
    private AudioSource _audioSource;

    private void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError("[ThrowGenerator] Main camera not found!");
            enabled = false;
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("[ThrowGenerator] Missing AudioSource component!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (CanSpawn() && Input.GetMouseButtonDown(0) && !IsMouseOverInteractiveObject())
        {
            SpawnPrefab();
            _nextSpawnTime = Time.time + cooldownDuration;
        }

        if (showCooldownDebug && Time.time < _nextSpawnTime)
        {
            Debug.Log($"Cooldown: {_nextSpawnTime - Time.time:F2}s remaining");
        }
    }

    private bool IsMouseOverInteractiveObject()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            return hit.collider.GetComponent<InteractiveObject>() != null;
        }

        return false;
    }

    private bool CanSpawn()
    {
        return Time.time >= _nextSpawnTime;
    }

    private void SpawnPrefab()
    {
        if (prefabs == null || prefabs.Count == 0)
        {
            Debug.LogError("[ThrowGenerator] prefabs list is empty or not assigned.");
            return;
        }

        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 spawnPosition = transform.position;

        Vector2 direction = (mousePosition - spawnPosition).normalized;

        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject spawnObject = Instantiate(prefabs[randomIndex], spawnPosition, Quaternion.identity);

        if (spawnObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            float forceMultiplier = Random.Range(1f, maxForceMultiplier);
            rb.AddForce(direction * baseForce * forceMultiplier, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("[ThrowGenerator] prefab missing Rigidbody2D component!");
            Destroy(spawnObject);
        }

        PlaySpawnSound(); // Play spawn sound
    }

    private void PlaySpawnSound()
    {
        if (spawnSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(spawnSound);
        }
    }

    private void OnValidate()
    {
        if (cooldownDuration < 0)
        {
            cooldownDuration = 0;
            Debug.LogWarning("[ThrowGenerator] Cooldown duration cannot be negative.");
        }
    }
}
