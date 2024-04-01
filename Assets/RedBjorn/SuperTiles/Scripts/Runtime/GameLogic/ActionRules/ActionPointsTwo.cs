using RedBjorn.SuperTiles.Battle;
using RedBjorn.SuperTiles.Paths;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.ActionRule
{
    /// <summary>
    /// Rules which allow to spend 2 action points.
    /// MoveAction cost 1 point. ItemAction cost 1 point.
    /// MoveAction is restricted after ItemAction is completed
    /// </summary>
    [CreateAssetMenu(menuName = ScriptablePath.Level.ActionRules.ActionPointsTwo)]
    public class ActionPointsTwo : ActionRules
    {
        public const int MaxActionPoints = 2;

        public override bool Validate(BaseAction action, BattleEntity battle)
        {
            if (action == null)
            {
                return false;
            }

            var actions = battle.TurnActions.Where(a => a.Unit == action.Unit);
            if (actions.Any(a => a.GetType() == Item))
            {
                return false;
            }

            if (actions.Count(a => a.GetType() == Move) >= MaxActionPoints)
            {
                return false;
            }
            return true;
        }

        public override bool CanMove(UnitEntity unit, BattleEntity battle)
        {
            var actions = battle.TurnActions.Where(a => a.Unit == unit);
            if (actions.Any(a => a.GetType() == Item))
            {
                return false;
            }
            return battle.TurnActions.Count(a => a.Unit == unit && a.GetType() == Move) < MaxActionPoints;
        }

        public override bool CanItem(UnitEntity unit, BattleEntity battle)
        {
            var actions = battle.TurnActions.Where(a => a.Unit == unit);
            if (actions.Any(a => a.GetType() == Item))
            {
                return false;
            }
            return battle.TurnActions.Count(a => a.Unit == unit && a.GetType() == Move) < MaxActionPoints;
        }
    }
}
