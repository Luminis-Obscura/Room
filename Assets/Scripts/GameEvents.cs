public static class GameEvents
{
    // Interaction events
    public static event System.Action<IInteractable> OnInteraction;
    public static void TriggerInteraction(IInteractable interactable)
    {
        OnInteraction?.Invoke(interactable);
    }
    
    // Inventory events
    public static event System.Action<InteractiveItem> OnItemAdded;
    public static void TriggerItemAdded(InteractiveItem item)
    {
        OnItemAdded?.Invoke(item);
    }
    
    public static event System.Action<string> OnItemUsed;
    public static void TriggerItemUsed(string itemId)
    {
        OnItemUsed?.Invoke(itemId);
    }
    
    // Scene events
    public static event System.Action<string> OnSceneChange;
    public static void TriggerSceneChange(string sceneName)
    {
        OnSceneChange?.Invoke(sceneName);
    }
    
    // Game state events
    public static event System.Action<GameState> OnGameStateChanged;
    public static void TriggerGameStateChanged(GameState newState)
    {
        OnGameStateChanged?.Invoke(newState);
    }
}