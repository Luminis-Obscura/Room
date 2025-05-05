using UnityEngine;
using UnityEngine.Events;

public class KeyboardEvent : MonoBehaviour
{
    [System.Serializable]
    public class KeyEventPair
    {
        public KeyCode key;
        public UnityEvent onKeyPressed;
        public UnityEvent onKeyHeld;
        public UnityEvent onKeyReleased;
    }

    [SerializeField]
    private KeyEventPair[] keyEvents;

    void Update()
    {
        foreach (var keyEvent in keyEvents)
        {
            // Key just pressed
            if (Input.GetKeyDown(keyEvent.key))
            {
                keyEvent.onKeyPressed?.Invoke();
            }
            
            // Key being held
            if (Input.GetKey(keyEvent.key))
            {
                keyEvent.onKeyHeld?.Invoke();
            }
            
            // Key just released
            if (Input.GetKeyUp(keyEvent.key))
            {
                keyEvent.onKeyReleased?.Invoke();
            }
        }
    }
}
