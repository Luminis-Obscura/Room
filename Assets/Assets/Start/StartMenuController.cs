using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel; // 显式引用要关闭的UI

    public void StartGame()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false); // 延迟后再关闭
        // 或者加载场景 SceneManager.LoadScene("YourSceneName");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
