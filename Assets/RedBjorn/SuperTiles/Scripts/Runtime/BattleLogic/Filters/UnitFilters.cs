using System.Collections.Generic;
using System.Linq;

namespace RedBjorn.SuperTiles.UnitFilters
{
    public class And : IUnitFilter
    {
        public List<IUnitFilter> List;

        public IEnumerable<UnitEntity> Select(IEnumerable<UnitEntity> context, BattleEntity battle)
        {
            var units = context.ToList();
            if (List != null)
            {
                foreach (var filter in List)
                {
                    units = filter.Select(units, battle).ToList();
                }
            }
            return units;
        }
    }

    public class Where : IUnitFilter
    {
        public IUnitCondition Condition;

        public IEnumerable<UnitEntity> Select(IEnumerable<UnitEntity> context, BattleEntity battle)
        {
            return Condition != null ? context.Where(c => Condition.IsMet(c, battle)) : context;
        }
    }
}
