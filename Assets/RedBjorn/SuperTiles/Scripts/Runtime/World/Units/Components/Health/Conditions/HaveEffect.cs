using RedBjorn.SuperTiles.Paths;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Health
{
    /// <summary>
    /// Condition which check victim effects and item tags
    /// </summary>
    [CreateAssetMenu(menuName = ScriptablePath.Health.Conditions.HaveEffect)]
    public class HaveEffect : CommonCondition
    {
        public enum OperatorLogic
        {
            ANY,
            ALL
        }

        public OperatorLogic EffectLogic;
        public List<EffectData> Effects = new List<EffectData>();
        [Space]
        public OperatorLogic ItemLogic;
        public List<ItemTag> ItemTags = new List<ItemTag>();

        protected override bool CheckIsMet(float delta, UnitEntity victim, UnitEntity damager, ItemEntity item)
        {
            var isMet = false;
            if (item != null && item.Data.Tags != null)
            {
                var victimEffects = victim.Effects.Select(e => e.Data).ToList();
                switch (EffectLogic)
                {
                    case OperatorLogic.ANY: isMet = Effects.Any(e => victimEffects.Contains(e)); break;
                    case OperatorLogic.ALL: isMet = Effects.All(e => victimEffects.Contains(e)); break;
                }

                switch (ItemLogic)
                {
                    case OperatorLogic.ANY: isMet &= ItemTags.Any(e => item.Data.Tags.Contains(e)); break;
                    case OperatorLogic.ALL: isMet &= ItemTags.All(e => item.Data.Tags.Contains(e)); break;
                }
            }
            return isMet;
        }
    }
}
