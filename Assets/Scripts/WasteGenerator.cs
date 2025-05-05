using System.Collections.Generic;
using UnityEngine;

public class WasteGenerator : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> wastePrefabs;
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
            SpawnWaste();
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

    private void SpawnWaste()
    {
        if (wastePrefabs == null || wastePrefabs.Count == 0)
        {
            Debug.LogError("[WasteGenerator] Waste prefabs list is empty or not assigned.");
            return;
        }

        // Get mouse position in world space
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure we're in 2D space

        // Spawn at transform position
        Vector3 spawnPosition = transform.position;
        
        // Calculate direction towards mouse
        Vector2 direction = (mousePosition - spawnPosition).normalized;
        
        // Spawn the waste object
        int randomIndex = Random.Range(0, wastePrefabs.Count);
        GameObject wasteObject = Instantiate(wastePrefabs[randomIndex], spawnPosition, Quaternion.identity);
        
        if (wasteObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            // Calculate force with random multiplier
            float forceMultiplier = Random.Range(1f, maxForceMultiplier);
            rb.AddForce(direction * baseForce * forceMultiplier, ForceMode2D.Impulse);
        }
        else 
        {
            Debug.LogError("[WasteGenerator] Waste prefab missing Rigidbody2D component!");
            Destroy(wasteObject);
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