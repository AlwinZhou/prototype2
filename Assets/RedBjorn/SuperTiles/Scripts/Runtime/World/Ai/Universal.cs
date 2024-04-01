using RedBjorn.ProtoTiles;
using RedBjorn.SuperTiles.Battle;
using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.SuperTiles.Squad;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Ai
{
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Ai.Universal)]
    public class Universal : UnitAiData
    {
        [Range(0f, 1f)]
        public float Intelligence = 0.75f;
        [Range(0f, 1f)]
        public float HealRatio = 0.5f;

        public override bool TryNextAction(AiEntity player, UnitAiEntity ai, BattleEntity battle, out BaseAction action)
        {
            action = null;
            var unit = ai.Unit;
            var map = battle.Map;
            var levelActions = battle.Level.Actions;
            var moveAllowed = levelActions.CanMove(unit, battle);
            var itemAllowed = levelActions.CanItem(unit, battle);
            if (!moveAllowed && !itemAllowed)
            {
                return false;
            }

            var rnd = battle.Randomizer;
            var itemTags = S.Battle.Tags.Item;
            var itemsAvailable = unit.Items.Where(item => item.CanUse(unit));
            var devices = itemsAvailable.Where(item => item.Data.Tags.Contains(itemTags.Device));
            var heals = devices.Where(item => item.Data.Tags.Contains(itemTags.Heal)).ToList();
            var buffs = devices.Where(item => item.Data.Tags.Contains(itemTags.Buff)).ToList();
            var debuffs = devices.Where(item => item.Data.Tags.Contains(itemTags.Debuff)).ToList();
            var weapons = itemsAvailable.Where(i => i.Data.Tags.Contains(itemTags.Weapon))
                                        .Where(i => i.PossibleTargets(unit.WorldPosition, battle)
                                                     .Any(t => battle.Level.EnemyUnits(player, battle).Contains(t)))
                                        .OrderByDescending(i => i[itemTags.Power] + (i[itemTags.PowerMin] + i[itemTags.PowerMax]) / 2f)
                                        .ToList();
            ItemEntity weapon = null;
            if (weapons.Count >= 2)
            {
                weapon = rnd.Chance(Intelligence) ? weapons[0] : weapons[1];
            }
            else
            {
                weapon = weapons.FirstOrDefault();
            }

            ItemEntity heal = heals.Random(rnd);
            ItemEntity buff = buffs.Random(rnd);
            ItemEntity debuff = debuffs.Random(rnd);

            var damagePotential = 0f;
            if (weapon != null)
            {
                damagePotential = weapon[itemTags.Power] + (weapon[itemTags.PowerMin] + weapon[itemTags.PowerMax]) / 2f;
            }

            var allies = battle.Level.AllyUnits(player, battle).Where(u => !u.IsDead);
            var enemies = battle.Level.EnemyUnits(player, battle).Where(u => !u.IsDead);

            UnitEntity damager = null;
            foreach (var enemy in enemies.OrderBy(en => map.Distance(en.WorldPosition, unit.WorldPosition)))
            {
                if(enemy.Items.Where(item => item.CanUse(enemy))
                              .Any(item => item.Data.Selector.SelectTargets(item, enemy.WorldPosition, unit.WorldPosition, enemy, battle).Contains(unit)))
                {
                    damager = enemy;
                    break;
                }
            }

            UnitEntity attackTarget = null;
            if (weapon != null)
            {
                var targets = weapon.PossibleTargets(unit.WorldPosition, battle)
                                    .Where(p => !p.IsDead)
                                    .Where(p => enemies.Contains(p))
                                    .OrderBy(p => p.Health.ValueCurrent)
                                    .ThenBy(p => map.Distance(p.WorldPosition, unit.WorldPosition))
                                    .Where(p => weapon.Data.Selector.SelectTargets(weapon, unit.WorldPosition, p.WorldPosition, unit, battle).Contains(p))
                                    .ToList();
                if (targets.Count >= 2)
                {
                    attackTarget = rnd.Chance(Intelligence) ? targets[0] : targets[1];
                }
                else
                {
                    attackTarget = targets.FirstOrDefault();
                }
            }

            var moveTarget = attackTarget;
            if (moveTarget == null)
            {
                moveTarget = map.NearestUnit(unit.WorldPosition, enemies);
            }

            var lowHp = unit.Health.ValueCurrent <= HealRatio * unit.Health.Max;

            if (itemAllowed && attackTarget != null && attackTarget.Health.ValueCurrent - damagePotential <= 0f)
            {
                action = new ItemAction(player, unit, weapon, attackTarget.WorldPosition);
                Log.I($"{ai} decide to kill {attackTarget}");
                return true;
            }

            if (moveAllowed && lowHp && RunAway(player, ai, map, damager, out action))
            {
                Log.I($"{ai} decide to run away from {damager}");
                return true;
            }

            if (heal != null)
            {
                var damagedAlly = allies.Where(a => a != ai.Unit)
                                        .Where(a => a.Health.ValueCurrent <= HealRatio * a.Health.Max)
                                        .ToList()
                                        .Where(a => heal.Data.Selector.SelectTargets(heal, unit.WorldPosition, a.WorldPosition, unit, battle).Contains(a))
                                        .OrderBy(a => map.Distance(a.WorldPosition, unit.WorldPosition))
                                        .FirstOrDefault();
                if (itemAllowed && damagedAlly != null)
                {
                    action = new ItemAction(player, unit, heal, damagedAlly.WorldPosition);
                    Log.I($"{ai} decide to heal ally {damagedAlly} with {heal}");
                    return true;
                }

                if (itemAllowed && lowHp)
                {
                    action = new ItemAction(player, unit, heal, unit.WorldPosition);
                    Log.I($"{ai} decide to heal himself with {heal}");
                    return true;
                }
            }

            UnitEntity targetBuff = null;
            if (buff != null)
            {
                targetBuff = buff.PossibleTargets(unit.WorldPosition, battle)
                                 .Where(p => !p.IsDead)
                                 .Where(p => allies.Contains(p))
                                 .Where(p => buff.Data.Selector.SelectTargets(buff, unit.WorldPosition, p.WorldPosition, unit, battle).Contains(p))
                                 .ToList()
                                 .Random(rnd);
            }
            UnitEntity targetDebuff = null;
            if (debuff != null)
            {
                targetDebuff = debuff.PossibleTargets(unit.WorldPosition, battle)
                                     .Where(p => !p.IsDead)
                                     .Where(p => enemies.Contains(p))
                                     .Where(p => debuff.Data.Selector.SelectTargets(debuff, unit.WorldPosition, p.WorldPosition, unit, battle).Contains(p))
                                     .ToList()
                                     .Random(rnd);
            }
            var doBuff = false;
            var doDebuff = false;
            if (allies.Count() > 1 && rnd.Chance(Intelligence))
            {
                if (targetBuff != null && targetDebuff != null)
                {
                    doBuff = rnd.Chance(0.5f);
                    doDebuff = !doBuff;
                }
                else if (targetBuff != null)
                {
                    doBuff = true;
                }
                else if (targetDebuff != null)
                {
                    doDebuff = true;
                }
            }

            if (itemAllowed && doBuff)
            {
                action = new ItemAction(player, unit, buff, targetBuff.WorldPosition);
                Log.I($"{ai} decide to apply {buff} to {targetBuff}");
                return true;
            }

            if (itemAllowed && doDebuff)
            {
                action = new ItemAction(player, unit, debuff, targetDebuff.WorldPosition);
                Log.I($"{ai} decide to apply {debuff} to {targetDebuff}");
                return true;
            }

            if (moveAllowed && moveTarget != null && attackTarget == null)
            {
                var point = Vector3Int.zero;
                Func<TileEntity, bool> condition = (t) => (t.Vacant || t.Units.Contains(unit)) 
                                                            && map.PathTiles(unit.WorldPosition, map.WorldPosition(t), 10).Count > 1;
                Func<Vector3Int, float> orderBy = (v) => map.Distance(moveTarget.TilePosition + v, unit.TilePosition);
                if (map.NearestPosition(moveTarget.TilePosition, out point, condition, orderBy) && point != unit.TilePosition)
                {
                    var targetPoint = map.WorldPosition(point);
                    action = new MoveAction(player, unit, targetPoint);
                    Log.I($"{ai} decide to move to the {moveTarget}. Point: {targetPoint}");
                    return true;
                }
            }

            if (itemAllowed && weapon != null && attackTarget != null)
            {
                action = new ItemAction(player, unit, weapon, attackTarget.WorldPosition);
                Log.I($"{ai} decide to attack {attackTarget} with {weapon}");
                return true;
            }
            return false;
        }

        bool RunAway(AiEntity player, UnitAiEntity ai, MapEntity map, UnitEntity damager, out BaseAction action)
        {
            action = null;
            var unit = ai.Unit;
            if (damager != null)
            {
                var directions = new List<Vector3>();
                var directionDamager = unit.WorldPosition - damager.WorldPosition;
                directions.Add(directionDamager);
                foreach(var dir in map.NeighboursDirection)
                {
                    var dirWorld = map.WorldPosition(dir);
                    if (Vector3.Dot(directionDamager, dirWorld) > 0)
                    {
                        directions.Add(dirWorld);
                    }
                }
                Func<TileEntity, bool> valid = (tile) =>
                {
                    return tile != null && (tile.Vacant || tile.Units.Contains(unit));
                };
                foreach(var direction in directions)
                {
                    var tiles = map.LineCast(unit.WorldPosition, unit.WorldPosition + direction.normalized * unit.Mover.MoveRange, unit.Mover.MoveRange, valid);
                    var finishTile = tiles.LastOrDefault(t => t.Vacant);
                    if (finishTile != null)
                    {
                        action = new MoveAction(player, unit, map.WorldPosition(finishTile));
                        return true;
                    }
                }
            }
            return false;
        }
    }
}