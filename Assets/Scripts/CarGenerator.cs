using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private List<GameObject> carPrefabs;
    [SerializeField] private Transform endPoint;

    [Header("Timing (in seconds)")]
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private float initialDelay = 2f;

    private float _nextSpawnTime;

    private void Start()
    {
        ValidateComponents();
        // Set initial spawn time after delay
        _nextSpawnTime = Time.time + initialDelay;
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            GenerateCar();
            // Calculate next spawn time
            _nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void GenerateCar()
    {
        if (carPrefabs == null || carPrefabs.Count == 0) return;

        int randomIndex = Random.Range(0, carPrefabs.Count);
        GameObject carPrefab = carPrefabs[randomIndex];
        Vector3 spawnPosition = transform.position;
        
        GameObject carObject = Instantiate(carPrefab, spawnPosition, Quaternion.identity);
        if (carObject.TryGetComponent<Car>(out Car car))
        {
            car.SetTarget(endPoint);
        }
        else
        {
            Debug.LogError($"Car prefab {carPrefab.name} does not have a Car component.");
            Destroy(carObject);
        }
    }

    private void ValidateComponents()
    {
        if (carPrefabs == null || carPrefabs.Count == 0)
        {
            Debug.LogError("Car prefabs list is empty or not assigned.");
        }

        if (endPoint == null)
        {
            Debug.LogError("End point transform is not assigned.");
        }
    }

    private void OnValidate()
    {
        // Validate timing values
        if (minSpawnInterval < 0)
        {
            minSpawnInterval = 0;
            Debug.LogWarning("Min spawn interval cannot be negative.");
        }

        if (maxSpawnInterval < minSpawnInterval)
        {
            maxSpawnInterval = minSpawnInterval;
            Debug.LogWarning("Max spawn interval cannot be less than min spawn interval.");
        }

        if (initialDelay < 0)
        {
            initialDelay = 0;
            Debug.LogWarning("Initial delay cannot be negative.");
        }

        // Validate car prefabs
        if (carPrefabs != null)
        {
            foreach (var prefab in carPrefabs)
            {
                if (prefab != null && prefab.GetComponent<Car>() == null)
                {
                    Debug.LogError($"Car prefab {prefab.name} does not have a Car component.");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, endPoint.position);
    }
}