using UnityEngine;

namespace RedBjorn.SuperTiles.UI
{
    public class EffectUnitWorldUI : MonoBehaviour
    {
        public GameObject Marker;
        public Vector3 LocalPosition;

        UnitEntity Unit;

        void Awake()
        {
            Marker.SetActive(false);
            transform.localPosition = LocalPosition;
        }

        void OnDestroy()
        {
            if (Unit != null)
            {
                Unit.OnEffectAdded += OnEffectAdded;
                Unit.OnEffectRemoved += OnEffectRemoved;
                Unit.Health.OnHeathChanged -= OnHealthChanged;
            }
        }

        public void Init(UnitEntity unit)
        {
            if (Unit != null)
            {
                Unit.OnEffectAdded -= OnEffectAdded;
                Unit.OnEffectRemoved -= OnEffectRemoved;
                Unit.Health.OnHeathChanged -= OnHealthChanged;
            }
            Unit = unit;
            if (Unit != null)
            {
                Unit.OnEffectAdded += OnEffectAdded;
                Unit.OnEffectRemoved += OnEffectRemoved;
                Unit.Health.OnHeathChanged += OnHealthChanged;
            }
        }

        void OnHealthChanged(HealthChangeContext obj)
        {
            MarkerUpdate();
        }

        void OnEffectAdded(EffectEntity effect)
        {
            MarkerUpdate();
        }

        void OnEffectRemoved(EffectEntity effect)
        {
            MarkerUpdate();
        }

        void MarkerUpdate()
        {
            Marker.gameObject.SetActive(!Unit.IsDead && Unit.Effects.Count > 0);
        }
    }
}

