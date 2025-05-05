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
    
    private float _nextSpawnTime;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError("[WasteGenerator] Main camera not found!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (CanSpawn() && Input.GetMouseButtonDown(0))
        {
            SpawnPrefab();
            _nextSpawnTime = Time.time + cooldownDuration;
        }

        if (showCooldownDebug && Time.time < _nextSpawnTime)
        {
            Debug.Log($"Cooldown: {_nextSpawnTime - Time.time:F2}s remaining");
        }
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
        mousePosition.z = 0; // Ensure we're in 2D space

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
    }

    private void OnValidate()
    {
        if (cooldownDuration < 0)
        {
            cooldownDuration = 0;
            Debug.LogWarning("[WasteGenerator] Cooldown duration cannot be negative.");
        }
    }
}