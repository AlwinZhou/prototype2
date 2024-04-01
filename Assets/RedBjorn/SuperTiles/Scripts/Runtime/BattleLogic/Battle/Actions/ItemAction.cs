using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles.Battle.Actions
{
    /// <summary>
    /// IAction which handle item information
    /// </summary>
    [Serializable]
    public partial class ItemAction : BaseAction
    {
        [SerializeReference]
        SquadControllerEntity PlayerInternal;

        [SerializeReference]
        UnitEntity UnitInternal;

        [SerializeReference]
        public ItemEntity Item;

        public Vector3 Position;

        public override SquadControllerEntity Player => PlayerInternal;
        public override UnitEntity Unit => UnitInternal;

        public ItemAction() { }

        public ItemAction(SquadControllerEntity player, UnitEntity unit, ItemEntity item)
        {
            PlayerInternal = player;
            UnitInternal = unit;
            Item = item;
        }

        public ItemAction(SquadControllerEntity player, UnitEntity unit, ItemEntity item, Vector3 position)
        {
            PlayerInternal = player;
            UnitInternal = unit;
            Item = item;
            Position = position;
        }

        public override void Do(Action onCompleted, BattleEntity battle)
        {
            if (Unit == null)
            {
                Log.E($"Action {this} with null unit won't play");
                onCompleted.SafeInvoke();
                return;
            }
            if (Unit.IsDead)
            {
                Log.E($"Unit {Unit} won't use {Item}. He is dead");
                onCompleted.SafeInvoke();
                return;
            }
            if (Item == null || Item.Data == null || Item.Data.ActionHandler == null)
            {
                Log.E($"Unit {Unit} won't use {Item} on {Position}. Action is invalid");
                onCompleted.SafeInvoke();
                return;
            }
            Log.I($"Unit {Unit} use {Item} on {Position}");
            Item.Use(this, battle, onCompleted);
            battle.TurnItems.Add(Item);
        }

        public bool ValidatePosition(BattleEntity battle)
        {
            Vector3 position;
            var valid = Item.Data.Selector.ValidatePosition(Item, Unit.WorldPosition, Position, Unit, battle, out position);
            if (valid)
            {
                if ((Position - position).sqrMagnitude > 0.01f)
                {
                    Position = position;
                }
            }
            return valid;
        }

        public override string ToString()
        {
            return string.Format("Owner: {0}. Item: {1}. Position: {2}", Unit, Item, Position);
        }
    }
}