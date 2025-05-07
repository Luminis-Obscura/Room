using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip transitionSound; // 音效
    [SerializeField] private AudioSource audioSource;   // 音频播放器

    private void Awake()
    {
        // 确保 AudioSource 存在
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(PlaySoundAndLoadScene(sceneName));
    }

    private IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        // 播放音效
        if (transitionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(transitionSound);
            yield return new WaitForSeconds(transitionSound.length); // 等待音效播放完成
        }

        // 触发场景切换事件并加载场景
        GameEvents.TriggerSceneChange(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}