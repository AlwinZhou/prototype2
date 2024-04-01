using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items
{
    /// <summary>
    /// Interface for TargetSelector creation from string type
    /// </summary>
    public interface TargetSelectorCreator
    {
        TargetSelector Create(ItemData item, string type);
    }

    namespace TargetSelectors
    {
        /// <summary>
        /// Default creator for TargetSelector with initial values
        /// </summary>
        public class DefaultCreator : TargetSelectorCreator
        {
            public TargetSelector Create(ItemData item, string type)
            {
                return ScriptableObject.CreateInstance(type) as TargetSelector;
            }
        }

        /// <summary>
        /// Creator class for RangeTargetSelector TargetSelector
        /// </summary>
        public class RangeTargetSelectorCreator : TargetSelectorCreator
        {
            public TargetSelector Create(ItemData item, string type)
            {
                var selector = ScriptableObject.CreateInstance(type) as RangeTargetSelector;
                selector.SelectTileVacant = true;
                selector.SelectUnits = true;
                var stats = S.Battle.Tags;
                selector.StatRange = stats.Item.Range;
                selector.StatAoeRange = stats.Item.AoeRange;
                if (!item.Stats.Any(i => i.Stat == selector.StatRange))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = selector.StatRange, Value = 5f });
                }

                if (!item.Stats.Any(i => i.Stat == selector.StatAoeRange))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = selector.StatAoeRange, Value = 0f });
                }
                return selector;
            }
        }

        /// <summary>
        /// Creator class for DirectionTargetSelector TargetSelector
        /// </summary>
        public class DirectionTargetSelectorCreator : TargetSelectorCreator
        {
            public TargetSelector Create(ItemData item, string type)
            {
                var selector = ScriptableObject.CreateInstance(type) as DirectionTargetSelector;
                selector.ThroughUnits = true;
                var stats = S.Battle.Tags;
                selector.StatRange = stats.Item.Range;
                if (!item.Stats.Any(i => i.Stat == selector.StatRange))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = selector.StatRange, Value = 5f });
                }
                return selector;
            }
        }
    }
}