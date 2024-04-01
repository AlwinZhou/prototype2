namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Interface for triggering game interactable objects
    /// </summary>
    public interface IInteractable
    {
        void StartInteracting();
        void StopInteracting();
    }
}
