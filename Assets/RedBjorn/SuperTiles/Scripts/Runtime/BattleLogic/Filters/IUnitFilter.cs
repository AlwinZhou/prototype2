using System.Collections.Generic;

namespace RedBjorn.SuperTiles
{
    public interface IUnitFilter
    {
        IEnumerable<UnitEntity> Select(IEnumerable<UnitEntity> context, BattleEntity battle);
    }
}

