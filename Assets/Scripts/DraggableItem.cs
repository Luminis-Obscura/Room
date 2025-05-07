using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class DraggableItem : InteractiveObject
{
    [Header("Drag Settings")]
    [SerializeField] private float dragSpeed = 10f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip floorCollisionSound;

    private Camera _mainCamera;
    private Rigidbody2D _rb;
    private AudioSource _audioSource;
    private bool _isDragging;
    private Vector2 _dragOffset;

    private float _lastCollisionSoundTime; // Tracks the last time a collision sound was played
    private const float CollisionSoundCooldown = 0.5f; // Minimum time between collision sounds

    protected override void Awake()
    {
        base.Awake(); // Call base.Awake to maintain hover functionality
        _mainCamera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        if (!_isDragging)
        {
            base.OnInteract();
            StartDragging();
        }
    }

    public override void OnStopInteract()
    {
        base.OnStopInteract();
        StopDragging();
    }

    private void OnMouseDown()
    {
        if (CanInteract())
        {
            PlaySound(clickSound); // Play click sound
            StartDragging();
        }
    }

    private void OnMouseUp()
    {
        if (_isDragging)
        {
            StopDragging();
            OnStopInteract();
        }
    }

    private void OnMouseDrag()
    {
        if (_isDragging)
        {
            Vector2 targetPos = GetMouseWorldPosition() + _dragOffset;
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * dragSpeed);
        }
    }

    private void OnMouseExit()
    {
        // Safety check - if mouse leaves collider while dragging
        if (_isDragging && !Input.GetMouseButton(0))
        {
            StopDragging();
            OnStopInteract();
        }
    }

    private void StartDragging()
    {
        _isDragging = true;
        _rb.gravityScale = 0f;
        _rb.linearVelocity = Vector2.zero;

        // Calculate offset from mouse to object center
        Vector2 mousePos = GetMouseWorldPosition();
        _dragOffset = (Vector2)transform.position - mousePos;
    }

    private void StopDragging()
    {
        if (_isDragging)
        {
            _isDragging = false;
            _rb.gravityScale = 1f;
        }
    }

    private void Update()
    {
        // Ensure we stop dragging if mouse is released anywhere
        if (_isDragging && !Input.GetMouseButton(0))
        {
            StopDragging();
            OnStopInteract();
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -_mainCamera.transform.position.z;
        return _mainCamera.ScreenToWorldPoint(mousePos);
    }

    private void OnValidate()
    {
        if (dragSpeed <= 0)
        {
            dragSpeed = 10f;
            Debug.LogWarning($"[{gameObject.name}] Drag speed should be greater than 0!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time - _lastCollisionSoundTime < CollisionSoundCooldown)
        {
            return; // Skip playing sound if cooldown has not elapsed
        }

        _lastCollisionSoundTime = Time.time; // Update the last collision sound time

        if (collision.gameObject.CompareTag("FloorObject"))
        {
            PlaySound(floorCollisionSound); // Play floor collision sound
        }
        else
        {
            PlaySound(collisionSound); // Play general collision sound
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