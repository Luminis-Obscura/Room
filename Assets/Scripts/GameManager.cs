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

    [Header("GameObject Activation")]
    [SerializeField]
    private GameObject targetGameObject; // The GameObject to activate when "R" is pressed

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

    private void Update()
    {
        // Check if the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (targetGameObject != null)
            {
                targetGameObject.SetActive(true); // Activate the target GameObject
                Debug.Log($"{targetGameObject.name} has been activated.");
            }
            else
            {
                Debug.LogWarning("Target GameObject is not assigned in the GameManager.");
            }
        }
    }
}
