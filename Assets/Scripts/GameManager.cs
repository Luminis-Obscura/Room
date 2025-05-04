using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is null!");
            }
            return _instance;
        }
    }
    
    [Header("Managers")]
    public UIManager UIManager;
    public SceneController SceneController;

    [SerializeField]
    private GameObject eventSystemPrefab;
    
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError("Multiple GameManager instances found. Destroying this instance. This will likely cause issues.");
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        // Initialize subsystems if not set
        if (UIManager == null) UIManager = GetComponentInChildren<UIManager>();
        if (SceneController == null) SceneController = GetComponentInChildren<SceneController>();

        // Check if EventSystem already exists
        if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var eventSystem = Instantiate(eventSystemPrefab);
            eventSystem.name = "EventSystem";
        }
    }
}
