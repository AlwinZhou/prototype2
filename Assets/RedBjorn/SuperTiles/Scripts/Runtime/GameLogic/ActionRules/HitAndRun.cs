using RedBjorn.SuperTiles.Battle;
using RedBjorn.SuperTiles.Paths;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.ActionRule
{
    /// <summary>
    /// Rules which allow 1 MoveAction and 1 ItemAction in any order
    /// </summary>
    [CreateAssetMenu(menuName = ScriptablePath.Level.ActionRules.HitAndRun)]
    public class HitAndRun : ActionRules
    {
        public override bool CanItem(UnitEntity unit, BattleEntity battle)
        {
            return battle.TurnActions.Count(s => s.Unit == unit && s.GetType() == Item) < 1;
        }

        public override bool CanMove(UnitEntity unit, BattleEntity battle)
        {
            return battle.TurnActions.Count(s => s.Unit == unit && s.GetType() == Move) < 1;
        }

        public override bool Validate(BaseAction action, BattleEntity battle)
        {
            if (action == null)
            {
                return false;
            }
            return battle.TurnActions.Count(s => s.Unit == action.Unit) < 2;
        }
    }
}
