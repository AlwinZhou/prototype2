using System.Collections.Generic;
using System.Linq;

namespace RedBjorn.SuperTiles.UnitSelectors
{
    public class Filter : IUnitSelector
    {
        public IUnitSelector Selector;
        public IUnitFilter UnitFilter;

        public IEnumerable<UnitEntity> Select(BattleEntity battle)
        {
            if (Selector != null)
            {
                var unitsSelected = Selector.Select(battle);
                if (UnitFilter != null)
                {
                    foreach (var unit in UnitFilter.Select(unitsSelected, battle))
                    {
                        yield return unit;
                    }
                }
                else
                {
                    foreach (var unit in unitsSelected)
                    {
                        yield return unit;
                    }
                }
            }
            yield break;
        }
    }

    public class PlayerCurrentSquad : IUnitSelector
    {
        public IEnumerable<UnitEntity> Select(BattleEntity battle)
        {
            return battle.Player.Squad;
        }
    }
}