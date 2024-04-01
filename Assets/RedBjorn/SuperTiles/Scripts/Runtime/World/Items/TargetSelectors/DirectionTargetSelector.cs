using RedBjorn.ProtoTiles;
using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.TargetSelectors
{
    /// <summary>
    /// TargetSelector which selects targets using ray-style logic
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Selectors.Direction)]
    public class DirectionTargetSelector : TargetSelector
    {
        [Header("Through")]
        public bool ThroughObstacles;
        public bool ThroughUnits = true;

        [Header("Other")]
        public bool ShowTrajectory;
        public ItemStatTag StatRange;

        public override IEnumerable<UnitEntity> PossibleTargets(ItemEntity item, Vector3 attackPosition, BattleEntity battle)
        {
            var inRange = battle.UnitsAlive.Where(u => battle.Map.Distance(u.WorldPosition, attackPosition) <= item[StatRange])
                                           .ToList();
            return inRange;
        }

        public override IEnumerable<UnitEntity> SelectTargets(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle)
        {
            return SelectTargets(item, TargetTiles(item, origin, target, owner, battle), owner, battle);
        }

        public override ITargetSelectorView StartActivation(ItemAction data, Action<ItemAction> onCompleted, BattleView battle)
        {
            var strategy = Spawner.Spawn(S.Prefabs.DirectionMode, Vector3.zero, Quaternion.identity);
            strategy.Init(data, this, onCompleted, battle);
            return strategy;
        }

        public override bool ValidatePosition(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle, out Vector3 validTarget)
        {
            validTarget = NormalizeTarget(item, origin, target, battle);
            return true;
        }

        public IEnumerable<UnitEntity> SelectTargets(ItemEntity item, List<TileEntity> tiles, UnitEntity owner, BattleEntity battle)
        {
            return tiles.Where(t => t != null && t.HasUnit && !t.Units.Contains(owner)).SelectMany(t => t.Units);
        }

        public List<TileEntity> TargetTiles(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle)
        {
            var validTarget = NormalizeTarget(item, origin, target, battle);
            var lineCasted = battle.Map.LineCast(origin, validTarget, item[StatRange], (tile) => IsTileValid(tile, owner, battle));
            for (int i = lineCasted.Count - 1; i >= 0; i--)
            {
                if (!IsTargetTile(lineCasted[i], owner, battle))
                {
                    lineCasted.RemoveAt(i);
                }
            }
            return lineCasted;
        }

        public Vector3 NormalizeTarget(ItemEntity item, Vector3 origin, Vector3 target, BattleEntity battle)
        {
            var distance = item[StatRange];
            var direction = target - origin;
            if (direction.sqrMagnitude < 0.01f)
            {
                direction = battle.Map.Settings.VectorCreate(1f, 0f, 0f);
            }
            return origin + distance * battle.Map.Normalize(direction);
        }

        bool IsTileValid(TileEntity tile, UnitEntity owner, BattleEntity battle)
        {
            if (tile != null)
            {
                if (ThroughObstacles)
                {
                    if (ThroughUnits)
                    {
                        return true;
                    }
                    return !tile.HasUnit || (tile.Units.Count() == 1 && tile.Units.Contains(owner));
                }
                else if (ThroughUnits)
                {
                    return battle.Level.ItemAvailable(tile) || tile.HasUnit;
                }
                return battle.Level.ItemAvailable(tile) || tile.Units.Contains(owner);
            }
            return false;
        }

        bool IsTargetTile(TileEntity tile, UnitEntity owner, BattleEntity battle)
        {
            var result = false;
            if (tile != null)
            {
                result = battle.Level.ItemAvailable(tile) || tile.HasUnit;
            }
            return result;
        }
    }
}