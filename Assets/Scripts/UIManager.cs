using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject interactionTextPanel;
    [SerializeField] private TMPro.TextMeshProUGUI interactionText;
    [SerializeField] private GameObject inventoryPanel;
    
    public void ShowInteractionText(string text)
    {
        interactionText.text = text;
        interactionTextPanel.SetActive(true);
    }
    
    public void HideInteractionText()
    {
        interactionTextPanel.SetActive(false);
    }
    
    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
