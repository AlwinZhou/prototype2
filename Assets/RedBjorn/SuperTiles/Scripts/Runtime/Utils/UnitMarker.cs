using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    public class UnitMarker : MonoBehaviour
    {
        UnitEntity Unit;

        public void Init(UnitEntity unit)
        {
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHeathChanged;
            }
            Unit = unit;
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged += OnHeathChanged;
            }
        }

        void OnDestroy()
        {
            if (Unit != null)
            {
                Unit.Health.OnHeathChanged -= OnHeathChanged;
            }
        }

        void OnHeathChanged(HealthChangeContext context)
        {
            if (Unit.IsDead)
            {
                Unit.Health.OnHeathChanged -= OnHeathChanged;
                gameObject.SetActive(false);
            }
        }
    }
}
