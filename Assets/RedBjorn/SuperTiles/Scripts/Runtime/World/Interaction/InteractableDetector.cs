using RedBjorn.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Controller which implements detecting of InteractableGameobjects
    /// </summary>
    public class InteractableDetector : MonoBehaviour
    {
        Plane Plane;
        GameObject Current;
        Locker Paused = new Locker();
        Dictionary<GameObject, InteractableGameobject> Interactables = new Dictionary<GameObject, InteractableGameobject>();

        void Update()
        {
            if (!Paused)
            {
                //Only one gameobject can be detected as interactable
                var current = InputController.OverGameobject(Plane);
                if (current && current.TryGetComponent(out InteractableGameobjectLink link) && link.Rerefence)
                {
                    current = link.Rerefence; 
                }
                if (Current != current)
                {
                    StopDetecting(Current);
                    StartDetecting(current);
                    Current = current;
                }
            }
        }

        public void Init(Plane plane)
        {
            Plane = plane;
        }

        public void Register(InteractableGameobject controller)
        {
            Interactables[controller.gameObject] = controller;
        }

        public void Unregister(InteractableGameobject controller)
        {
            Interactables.Remove(controller.gameObject);
        }

        public void Play()
        {
            Paused.Unlock();
        }

        public void Pause()
        {
            Paused.Lock();
            StopDetecting(Current);
            Current = null;
        }

        void StartDetecting(GameObject interactable)
        {
            if (interactable != null)
            {
                var controller = Interactables.TryGetOrDefault(interactable);
                if (controller != null)
                {
                    controller.StartInteracting();
                }
            }
        }

        void StopDetecting(GameObject interactable)
        {
            if (interactable != null)
            {
                var controller = Interactables.TryGetOrDefault(interactable);
                if (controller != null)
                {
                    controller.StopInteracting();
                }
            }
        }
    }
}
