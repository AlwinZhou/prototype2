using RedBjorn.SuperTiles.Battle;
using RedBjorn.SuperTiles.Paths;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.ActionRule
{
    /// <summary>
    /// Rules which allow 1 MoveAction and  1 ItemAction. MoveAction should be before ItemAction
    /// </summary>
    [CreateAssetMenu(menuName = ScriptablePath.Level.ActionRules.HitAfterRun)]
    public class HitAfterRun : ActionRules
    {
        public override bool Validate(BaseAction action, BattleEntity battle)
        {
            if (action == null)
            {
                return false;
            }
            return battle.TurnActions.Count(s => s.Unit == action.Unit && s.GetType() == action.GetType()) < 1;
        }

        public override bool CanMove(UnitEntity unit, BattleEntity battle)
        {
            return !battle.TurnActions.Any(a => a.Unit == unit);
        }

        public override bool CanItem(UnitEntity unit, BattleEntity battle)
        {
            var actions = battle.TurnActions.Where(a => a.Unit == unit);
            if (actions.Any(a => a.GetType() == Item))
            {
                return false;
            }
            return actions.Count() <= 1;
        }
    }
}
