using UnityEngine;

public class InteractiveManager : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D interactiveCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;
    
    private bool _interactionsEnabled = true;
    private static InteractiveManager _instance;
    
    public static InteractiveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<InteractiveManager>();
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        // Set default cursor
        SetDefaultCursor();
    }
    
    public bool AreInteractionsEnabled()
    {
        return _interactionsEnabled;
    }
    
    public void SetInteractionsEnabled(bool enabled)
    {
        _interactionsEnabled = enabled;
        if (!enabled)
        {
            // Reset cursor when disabling interactions
            SetDefaultCursor();
        }
    }
    
    public void SetInteractiveCursor()
    {
        if (_interactionsEnabled && interactiveCursor != null)
        {
            Cursor.SetCursor(interactiveCursor, cursorHotspot, CursorMode.Auto);
        }
    }
    
    public void SetDefaultCursor()
    {
        if (defaultCursor != null)
        {
            Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}