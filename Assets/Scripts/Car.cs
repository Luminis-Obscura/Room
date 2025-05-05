using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Car : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float destroyDistance = 20f;
    
    [Header("Optional Components")]
    [SerializeField] private AudioSource carSound;
    
    private Vector3 _startPosition;
    private Rigidbody2D _rb;
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
        if (_target == null)
        {
            Debug.LogWarning($"[{gameObject.name}] No target set for car movement!");
        }
    }

    private void Start()
    {
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        
        if (_rb != null)
        {
            _rb.freezeRotation = true;
        }

        if (carSound != null)
        {
            carSound.Play();
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckDestroyDistance();
    }

    private void Move()
    {
        if (_rb == null || _target == null) return;

        Vector3 direction = (_target.position - transform.position).normalized;
        _rb.MovePosition(transform.position + direction * (speed * Time.fixedDeltaTime));
    }

    private void CheckDestroyDistance()
    {
        float distanceTraveled = Vector3.Distance(_startPosition, transform.position);
        if (distanceTraveled > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Car {gameObject.name} hit player!");
        }
    }

    private void OnValidate()
    {
        if (speed <= 0)
        {
            Debug.LogWarning($"[{gameObject.name}] Car speed should be greater than 0!");
            speed = 5f;
        }

        if (destroyDistance <= 0)
        {
            Debug.LogWarning($"[{gameObject.name}] Destroy distance should be greater than 0!");
            destroyDistance = 20f;
        }
    }
}