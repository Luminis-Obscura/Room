using Unity.VisualScripting;
using UnityEngine;

public class InteractiveItem : InteractiveObject
{
    public override void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
        // Also call the base class method if needed
        base.OnInteract();
    }
}