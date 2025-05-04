using System.Threading.Tasks;
using UnityEngine;

public class DontDestroyEventSystem : MonoBehaviour
{
    void Awake()
    {
        // Find all EventSystem objects in the scene FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
        UnityEngine.EventSystems.EventSystem[] eventSystems = FindObjectsByType<UnityEngine.EventSystems.EventSystem>(UnityEngine.FindObjectsSortMode.None);

        // If there are more than one EventSystem objects, destroy this one
        if (eventSystems.Length > 1)
        {
            Debug.LogError("Multiple EventSystems found in the scene.");
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
}
