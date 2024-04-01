using System.Collections.Generic;
using System.Linq;

namespace RedBjorn.SuperTiles.UnitConditions
{
    public class Not : IUnitCondition
    {
        public IUnitCondition Condition;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            return Condition != null && !Condition.IsMet(unit, battle);
        }
    }

    public class And : IUnitCondition
    {
        public List<IUnitCondition> List;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            return List != null && List.All(l => l.IsMet(unit, battle));
        }
    }

    public class DataEqual : IUnitCondition
    {
        public UnitData Unit;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            return unit.Data == Unit;
        }
    }

    public class DataTagsContains : IUnitCondition
    {
        public UnitTag Tag;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            return unit.Data.Tags.Contains(Tag);
        }
    }

    public class HealthLack : IUnitCondition
    {
        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            return unit.Health.ValueCurrent < unit.Health.Max;
        }
    }
}

namespace RedBjorn.SuperTiles.UnitConditions.Tile
{
    public class UnitsAny : IUnitCondition
    {
        public IUnitCondition TileCondition;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            return battle.Map.Tile(unit.TilePosition).Units.Any(u => TileCondition.IsMet(u, battle));
        }
    }

    public class UnitsSpecificAlly : IUnitCondition
    {
        public IUnitCondition SpecificUnit;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            var player = battle.Players.FirstOrDefault(p => p.Squad.Contains(unit));
            if (player == null)
            {
                return false;
            }
            var units = battle.Map.Tile(unit.TilePosition).Units.Where(u => SpecificUnit == null || SpecificUnit.IsMet(u, battle))
                                                                .Where(u => u != unit);
            return units.Any(u => battle.Level.IsAllies(player, battle.Players.FirstOrDefault(p => p.Squad.Contains(u))));
        }
    }

    public class UnitsSpecificNotAlly : IUnitCondition
    {
        public IUnitCondition SpecificUnit;
        public bool CheckIfAnyone;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            var player = battle.Players.FirstOrDefault(p => p.Squad.Contains(unit));
            if (player == null)
            {
                return false;
            }
            var units = battle.Map.Tile(unit.TilePosition).Units.Where(u => SpecificUnit == null || SpecificUnit.IsMet(u, battle))
                                                                .Where(u => u != unit);
            return (!CheckIfAnyone || units.Any()) 
                        && units.All(u => !battle.Level.IsAllies(player, battle.Players.FirstOrDefault(p => p.Squad.Contains(u))));

        }
    }

    public class UnitsSpecificNotEnemy : IUnitCondition
    {
        public IUnitCondition SpecificUnit;
        public bool CheckIfAnyone;

        public bool IsMet(UnitEntity unit, BattleEntity battle)
        {
            var player = battle.Players.FirstOrDefault(p => p.Squad.Contains(unit));
            if (player == null)
            {
                return false;
            }
            var units = battle.Map.Tile(unit.TilePosition).Units.Where(u => SpecificUnit == null || SpecificUnit.IsMet(u, battle))
                                                                .Where(u => u != unit);
            return (!CheckIfAnyone || units.Any()) 
                        && units.All(u => !battle.Level.IsEnemies(player, battle.Players.FirstOrDefault(p => p.Squad.Contains(u))));
        }
    }
}
