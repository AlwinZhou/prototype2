using System.Collections.Generic;

namespace RedBjorn.SuperTiles
{
    public interface IUnitSelector
    {
        IEnumerable<UnitEntity> Select(BattleEntity battle);
    }
}

