using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles.Effects.Handlers
{
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Effect.Handlers.Damage)]
    public class Damage : EffectHandler
    {
        public EffectStatTag PowerTag;

        protected override IEnumerator DoHandle(EffectEntity effect, UnitEntity unit, BattleEntity battle)
        {
            UnitEntityActions.Damage(unit, effect[PowerTag], battle);
            yield break;
        }
    }
}

