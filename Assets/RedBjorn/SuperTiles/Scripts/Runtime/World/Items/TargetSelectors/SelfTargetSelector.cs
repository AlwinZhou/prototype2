using RedBjorn.SuperTiles.Battle.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.TargetSelectors
{
    /// <summary>
    /// TargetSelector which select targets are located inside StatAoeRange circle at StatRange distance
    /// </summary>
    public class SelfTargetSelector : TargetSelector
    {
        public override IEnumerable<UnitEntity> PossibleTargets(ItemEntity item, Vector3 attackPosition, BattleEntity battle)
        {
            return battle.Map.Tile(attackPosition).Units;
        }

        public override IEnumerable<UnitEntity> SelectTargets(ItemEntity item, Vector3 origin, Vector3 finish, UnitEntity owner, BattleEntity battle)
        {
            yield return owner;
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
