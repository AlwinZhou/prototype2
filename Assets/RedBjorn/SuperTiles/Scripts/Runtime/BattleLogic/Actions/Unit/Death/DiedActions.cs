using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.DiedActions
{
    public class Multiple : IDiedAction
    {
        public List<IDiedAction> List;

        public void Handle(UnitEntity unit, UnitEntity killer, BattleEntity battle)
        {
            if (List != null)
            {
                foreach (var action in List)
                {
                    if (action != null)
                    {
                        action.Handle(unit, killer, battle);
                    }
                }
            }
        }
    }

    public class Rebirth : IDiedAction
    {
        public void Handle(UnitEntity unit, UnitEntity killer, BattleEntity battle)
        {
            unit.Rebirth();
        }
    }

    public class KillerTakeControl : IDiedAction
    {
        public void Handle(UnitEntity unit, UnitEntity killer, BattleEntity battle)
        {
            battle.TakeUnitControl(unit, battle.Players.FirstOrDefault(p => p.Squad.Contains(killer)));
        }
    }

    public class VictimMapUnregister : IDiedAction
    {
        public void Handle(UnitEntity unit, UnitEntity killer, BattleEntity battle)
        {
            battle.Map.UnRegisterUnit(unit);
        }
    }
}
