using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Component which change rendrer material when interactable logic is triggered
    /// </summary>
    public class InteractableMaterial : MonoBehaviour, IInteractable
    {
        public Renderer Renderer;

        Material Original;
        Material Interactable => S.Battle.Interactable;

        void Awake()
        {
            if (!Renderer)
            {
                Renderer = GetComponent<MeshRenderer>();
            }

            if (Renderer)
            {
                Original = Renderer.material;
            }
        }

        public void StartInteracting()
        {
            On();
        }

        public void StopInteracting()
        {
            Off();
        }

        void On()
        {
            if (Renderer)
            {
                Renderer.material = Interactable;
            }
        }

        void Off()
        {
            if (Renderer)
            {
                Renderer.material = Original;
            }
        }
    }
}