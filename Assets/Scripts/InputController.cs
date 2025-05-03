using UnityEngine;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        HandleMouseInput();
    }
    
    private void HandleMouseInput()
    {
        
    }
}