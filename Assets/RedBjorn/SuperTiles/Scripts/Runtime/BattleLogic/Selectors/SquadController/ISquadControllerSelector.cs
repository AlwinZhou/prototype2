using System.Collections.Generic;

namespace RedBjorn.SuperTiles
{
    public interface ISquadControllerSelector
    {
        IEnumerable<SquadControllerEntity> Select(BattleEntity battle);
    }
}
