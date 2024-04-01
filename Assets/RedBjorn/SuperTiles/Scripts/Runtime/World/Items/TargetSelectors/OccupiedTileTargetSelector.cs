using RedBjorn.SuperTiles.Battle.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.TargetSelectors
{
    /// <summary>
    /// TargetSelector which select targets are located inside StatAoeRange circle at StatRange distance
    /// </summary>
    public class OccupiedTileTargetSelector : TargetSelector
    {
        public List<UnitTag> ShouldContains = new List<UnitTag>();
        public bool SelectAlly;

        public override IEnumerable<UnitEntity> PossibleTargets(ItemEntity item, Vector3 attackPosition, BattleEntity battle)
        {
            return battle.Map.Tile(attackPosition).Units;
        }

        public override IEnumerable<UnitEntity> SelectTargets(ItemEntity item, Vector3 origin, Vector3 finish, UnitEntity owner, BattleEntity battle)
        {
            var units = battle.Map.Tile(owner.TilePosition).Units;
            if (ShouldContains.Count > 0)
            {
                units = units.Where(u => u.Data.Tags.Intersect(ShouldContains).Any());
            }
            if (!SelectAlly)
            {
                var ownerPlayer = battle.Players.FirstOrDefault(p => p.Squad.Contains(owner));
                if (ownerPlayer != null)
                {
                    units = units.Where(u =>
                    {
                        var player = battle.Players.FirstOrDefault(p => p.Squad.Contains(u));
                        return !battle.Level.IsAllies(ownerPlayer, player);
                   });
                }
            }
            return units;
        }

        public override ITargetSelectorView StartActivation(ItemAction initContext, Action<ItemAction> onCompleted, BattleView battle)
        {
            initContext.Position = initContext.Unit.WorldPosition;
            onCompleted?.Invoke(initContext);
            return null;
        }

        public override bool ValidatePosition(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle, out Vector3 validTarget)
        {
            validTarget = owner.WorldPosition;
            return true;
        }
    }
}
