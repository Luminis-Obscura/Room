using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string currentSceneName;
    
    public void LoadScene(string sceneName)
    {
        currentSceneName = sceneName;
        GameEvents.TriggerSceneChange(sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
    public string GetCurrentSceneName()
    {
        return currentSceneName;
    }
}