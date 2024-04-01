using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Root interactable component consolidating  IInteractable interfaces
    /// </summary>
    public class InteractableGameobject : MonoBehaviour
    {
        IInteractable[] Interactables;
        InteractableDetector Detector;
        GameEntity Game;

        void Awake()
        {
            Detector = ObjectExtensions.FindObjectOfType<InteractableDetector>();
        }

        void OnEnable()
        {
            RefreshInteractables();
            if (Detector)
            {
                Detector.Register(this);
            }
        }

        void OnDisable()
        {
            if (Detector)
            {
                Detector.Unregister(this);
            }
        }

        void OnDestroy()
        {
            if (Game != null)
            {
                Game.OnStarted -= OnBattleCreated;
            }
        }

        public void Init(GameEntity game)
        {
            if (Game != null)
            {
                Game.OnStarted -= OnBattleCreated;
            }
            Game = game;
            if (Game != null)
            {
                Game.OnStarted += OnBattleCreated;
            }
        }

        public void StartInteracting()
        {
            foreach (var interactable in Interactables)
            {
                interactable.StartInteracting();
            }
        }

        public void StopInteracting()
        {
            foreach (var interactable in Interactables)
            {
                interactable.StopInteracting();
            }
        }

        void RefreshInteractables()
        {
            Interactables = GetComponentsInChildren<IInteractable>(includeInactive: true);
        }

        void OnBattleCreated()
        {
            RefreshInteractables();
        }
    }
}
