using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles.Battle.Actions
{
    /// <summary>
    /// IAction which handle unit movement information
    /// </summary>
    [Serializable]
    public partial class MoveAction : BaseAction
    {
        [SerializeReference]
        SquadControllerEntity PlayerInternal;

        [SerializeReference]
        UnitEntity UnitInternal;

        [SerializeField]
        Vector3 Point;

        public override SquadControllerEntity Player => PlayerInternal;
        public override UnitEntity Unit => UnitInternal;

        public MoveAction(SquadControllerEntity player, UnitEntity unit, Vector3 point)
        {
            PlayerInternal = player;
            UnitInternal = unit;
            Point = point;
        }

        public override void Do(Action onCompleted, BattleEntity battle)
        {
            if (Unit == null)
            {
                Log.E($"Can't move to {Point} null unit");
                onCompleted.SafeInvoke();
                return;
            }
            if (Unit.IsDead)
            {
                Log.E($"Can't move to {Point} dead unit");
                onCompleted.SafeInvoke();
                return;
            }
            var tile = battle.Map.Tile(Point);
            if (tile == null || !tile.Vacant)
            {
                Log.E($"Can't move {Unit} to {Point}, because of invalid tile");
                onCompleted.SafeInvoke();
                return;
            }
            battle.Map.UnRegisterUnit(Unit);
            Action onFinishMove = () =>
            {
                battle.Map.RegisterUnit(Unit);
                onCompleted.SafeInvoke();
            };
            Log.I($"Unit {Unit} moving to {Point}");
            Unit.Mover.Move(Point, onFinishMove);
        }

        public override string ToString()
        {
            return string.Format("Unit {0} move to {1}", UnitInternal, Point);
        }
    }
}