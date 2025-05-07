using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel; // UI面板引用
    private static bool hasShownOnce = false; // 静态变量，标记是否已经显示过
    private bool isCurrentMainScene = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isCurrentMainScene = scene.name == "MainScene";

        if (isCurrentMainScene)
        {
            // 如果从别的场景切回来，并且已经显示过，则不显示UI
            if (!hasShownOnce)
            {
                hasShownOnce = true;
                if (mainMenuPanel != null)
                    mainMenuPanel.SetActive(true);
            }
            else
            {
                if (mainMenuPanel != null)
                    mainMenuPanel.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // 按 R 重置标志（用于测试或某种玩法）
        if (Input.GetKeyDown(KeyCode.R))
        {
            hasShownOnce = false;

            // 若当前是主场景则重新启用UI
            if (isCurrentMainScene && mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

