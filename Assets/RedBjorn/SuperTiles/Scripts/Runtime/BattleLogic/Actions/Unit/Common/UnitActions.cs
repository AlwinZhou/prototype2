using RedBjorn.Utils;
using UnityEngine;

namespace RedBjorn.SuperTiles.UnitActions
{
    public class HealthViewCreateDefault : IUnitAction
    {
        public void Handle(UnitEntity unit, BattleEntity battle)
        {
            unit.View.gameObject.AddComponent<UnitHealthView>();
            S.HealthInit(unit.View.gameObject, unit);
        }
    }

    public class HealthViewCreatePrefab : IUnitAction
    {
        public GameObject HealthPrefab;

        public void Handle(UnitEntity unit, BattleEntity battle)
        {
            if (HealthPrefab)
            {
                var healthView = Spawner.Spawn(HealthPrefab, unit.View.transform);
                healthView.transform.localPosition = Vector3.zero;
                healthView.transform.localRotation = Quaternion.identity;
                S.HealthInit(healthView, unit);
            }
        }
    }
}
