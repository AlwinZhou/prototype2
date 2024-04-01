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
    /// TargetSelector which select targets are located inside StatAoeRange circle at StatRange distance
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Selectors.Range)]
    public class RangeTargetSelector : TargetSelector
    {
        [Header("Select")]
        public bool SelectTileVacant;
        public bool SelectUnits;
        public List<UnitTag> ShouldContain = new List<UnitTag>();
        public List<UnitTag> ShouldAvoid = new List<UnitTag>();

        [Header("Through")]
        public bool ThroughObstacles;
        public bool ThroughUnits;

        [Header("Other")]
        public bool ShowTrajectory;
        public ItemStatTag StatRange;
        public ItemStatTag StatAoeRange;

        public override IEnumerable<UnitEntity> PossibleTargets(ItemEntity item, Vector3 attackPosition, BattleEntity battle)
        {
            return InRange(item, attackPosition, battle);
        }

        public override IEnumerable<UnitEntity> SelectTargets(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle)
        {
            var tile = SelectTile(item, origin, target, owner, battle);
            return SelectTargetsInternal(item, origin, tile, owner, battle);
        }

        public override ITargetSelectorView StartActivation(ItemAction data, Action<ItemAction> onCompleted, BattleView battle)
        {
            var strategy = Spawner.Spawn(S.Prefabs.RangeMode, Vector3.zero, Quaternion.identity);
            strategy.Init(this, data, onCompleted, battle);
            return strategy;
        }

        public override bool ValidatePosition(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle, out Vector3 validTarget)
        {
            validTarget = target;
            var validTile = SelectTile(item, origin, target, owner, battle);
            if (validTile != null)
            {
                var targetTile = battle.Map.Tile(target);
                if (targetTile != validTile)
                {
                    validTarget = battle.Map.WorldPosition(validTile.Position);
                }
                return true;
            }
            return false;
        }

        public IEnumerable<UnitEntity> SelectTargets(ItemEntity item, Vector3 origin, TileEntity target, UnitEntity owner, BattleEntity battle)
        {
            return SelectTargetsInternal(item, origin, target, owner, battle);
        }

        public TileEntity SelectTile(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle)
        {
            var map = battle.Map;
            var distance = map.Distance(origin, target);
            distance = Mathf.Min(distance, item[StatRange]);
            var validTiles = map.LineCast(origin, target, distance, (tile) => IsValidTile(tile, owner, battle));
            for (int i = validTiles.Count - 1; i >= 0; i--)
            {
                if (IsTargetTile(validTiles[i], owner, battle))
                {
                    return validTiles[i];
                }
            }
            return null;
        }

        public List<Vector3> AreaSelectPositions(ItemEntity item, TileEntity tile, BattleEntity battle)
        {
            return battle.Map.AreaExistedPositions(tile, item[StatAoeRange]);
        }

        public int AreaCapacity(ItemEntity item, BattleEntity battle)
        {
            return battle.Map.AreaPositions(Vector3Int.zero, item[StatAoeRange]).Count;
        }

        public List<Vector3> AreaAvailable(ItemEntity item, Vector3 origin, UnitEntity owner, BattleEntity battle)
        {
            var map = battle.Map;
            var result = new List<Vector3>();
            var offsets = new Vector3[map.VerticesInner.Length + 1];
            offsets[0] = Vector3.zero;
            for (int i = 0; i < map.VerticesInner.Length; i++)
            {
                offsets[i + 1] = 0.99f * map.VerticesInner[i];
            }
            var testPoints = new Vector3[offsets.Length];
            foreach (var position in map.AreaExistedPositions(origin, item[S.Battle.Tags.Item.Range]))
            {
                for (int i = 0; i < testPoints.Length; i++)
                {
                    testPoints[i] = position + offsets[i];
                }
                TileEntity tile = null;
                foreach (var point in testPoints)
                {
                    tile = SelectTile(item, origin, point, owner, battle);
                    if (map.IsSameTile(tile, point))
                    {
                        break;
                    }
                    tile = null;
                }
                if (tile != null)
                {
                    result.Add(position);
                }
            }
            return result;
        }

        IEnumerable<UnitEntity> SelectTargetsInternal(ItemEntity item, Vector3 origin, TileEntity target, UnitEntity owner, BattleEntity battle)
        {
            if (target != null)
            {
                var map = battle.Map;
                var worldPosition = map.WorldPosition(target);
                foreach (var possible in InRange(item, origin, battle).Where(t => map.Distance(t.WorldPosition, worldPosition) <= item[StatAoeRange]))
                {
                    var valid = ShouldAvoid.Count == 0 || !possible.Data.Tags.Any(t => ShouldAvoid.Contains(t));
                    valid &= ShouldContain.Count == 0 || possible.Data.Tags.Intersect(ShouldContain).Any();
                    if (valid)
                    {
                        yield return possible;
                    }
                }
            }
        }

        bool IsValidTile(TileEntity tile, UnitEntity owner, BattleEntity battle)
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
                result = SelectTileVacant && battle.Level.ItemAvailable(tile);
                result |= SelectUnits && tile.HasUnit;
            }
            return result;
        }

        IEnumerable<UnitEntity> InRange(ItemEntity item, Vector3 attackPosition, BattleEntity battle)
        {
            var inRange = battle.UnitsAlive.Where(u => battle.Map.Distance(u.WorldPosition, attackPosition) <= item[StatRange] + item[StatAoeRange])
                                           .ToList();
            return inRange;
        }
    }
}

