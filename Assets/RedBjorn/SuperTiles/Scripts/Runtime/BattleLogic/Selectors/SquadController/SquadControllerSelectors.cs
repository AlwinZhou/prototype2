using System.Collections.Generic;

namespace RedBjorn.SuperTiles.SquadControllerSelectors
{
    public class All : ISquadControllerSelector
    {
        public IEnumerable<SquadControllerEntity> Select(BattleEntity battle)
        {
            return battle.Players;
        }
    }

    public class Current : ISquadControllerSelector
    {
        public IEnumerable<SquadControllerEntity> Select(BattleEntity battle)
        {
            yield return battle.Player;
        }
    }
}
