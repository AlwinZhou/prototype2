using RedBjorn.Utils;
using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles.Effects.Handlers
{
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Effect.Handlers.FxPlay)]
    public class FxPlay : EffectHandler
    {
        public GameObject Prefab;
        public float Duration;

        protected override IEnumerator DoHandle(EffectEntity effect, UnitEntity unit, BattleEntity battle)
        {
            if (Prefab)
            {
                var fx = Spawner.Spawn(Prefab);
                fx.transform.position = unit.WorldPosition;
                yield return new WaitForSeconds(Duration);
                Spawner.Despawn(fx);
            }
        }
    }
}
