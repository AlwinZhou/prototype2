using RedBjorn.SuperTiles.UI;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Tooltip describes unit
    /// </summary>
    public class InteractableUnitTooltip : MonoBehaviour, IInteractable, IUnitInitialize
    {
        SessionTooltipUI Tooltip;
        UnitEntity Unit;

        void OnEnable()
        {
            Tooltip = ObjectExtensions.FindObjectOfType<SessionTooltipUI>();
        }

        public void Init(UnitEntity unit)
        {
            Unit = unit;
        }

        public void StartInteracting()
        {
            if (Tooltip)
            {
                Tooltip.Register(Unit);
            }
        }

        public void StopInteracting()
        {
            if (Tooltip)
            {
                Tooltip.Unregister(Unit);
            }
        }
    }
}
