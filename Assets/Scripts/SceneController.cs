using UnityEngine;

public class SceneController : MonoBehaviour
{   
    public void LoadScene(string sceneName)
    {
        GameEvents.TriggerSceneChange(sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}