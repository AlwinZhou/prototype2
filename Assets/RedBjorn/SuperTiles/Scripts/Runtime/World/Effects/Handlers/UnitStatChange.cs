using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles.Effects.Handlers
{
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Effect.Handlers.UnitStatChange)]
    public class UnitStatChange : EffectHandler
    {
        public enum Operation
        {
            Add,
            Substract,
            Multiply,
            Divide
        }

        public UnitStatTag UnitStat;
        public EffectStatTag EffectStat;
        [Header("Stat = Multiplicator x DataValue + Additive")]
        public bool ChangeMultiplicator;
        public Operation MultiplicatorOperation;
        public bool ChangeAdditive;
        public Operation AdditiveOperation;

        protected override IEnumerator DoHandle(EffectEntity effect, UnitEntity unit, BattleEntity battle)
        {
            var stat = unit[UnitStat];
            var power = effect[EffectStat];

            if (ChangeMultiplicator)
            {
                switch (MultiplicatorOperation)
                {
                    case Operation.Add: stat.Multiplicator += power; break;
                    case Operation.Substract: stat.Multiplicator -= power; break;
                    case Operation.Multiply: stat.Multiplicator *= power; break;
                    case Operation.Divide: stat.Multiplicator /= power; break;
                    default: break;
                }
            }

            if (ChangeAdditive)
            {
                switch (AdditiveOperation)
                {
                    case Operation.Add: stat.Additive += power; break;
                    case Operation.Substract: stat.Additive -= power; break;
                    case Operation.Multiply: stat.Multiplicator *= power; break;
                    case Operation.Divide: stat.Multiplicator /= power; break;
                    default: break;
                }
            }

            unit.StatUpdate(UnitStat, stat);
            yield break;
        }
    }
}