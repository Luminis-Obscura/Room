using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Dialogue,
    Cutscene,
    GameOver
}


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
    
    [Header("Game State")]
    [SerializeField] private GameState currentState;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Initialize subsystems if not set
        if (UIManager == null) UIManager = GetComponentInChildren<UIManager>();
        if (SceneController == null) SceneController = GetComponentInChildren<SceneController>();
    }
    
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        GameEvents.TriggerGameStateChanged(newState);
    }
    
    public bool IsState(GameState state)
    {
        return currentState == state;
    }
}
