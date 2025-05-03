using UnityEngine;

public class InteractiveItem : InteractiveObject
{
    [Header("Item Settings")]
    [SerializeField] private Sprite inventorySprite;
    
    public Sprite InventorySprite => inventorySprite;

    public override void OnInteract()
    {

    }
}