using System.Linq;

namespace RedBjorn.SuperTiles
{
    public class UnitEntityActions
    {
        public static void Heal(UnitEntity target, float power, UnitEntity healer, ItemEntity item, BattleEntity battle)
        {
            Change(target, power, healer, item, battle);
        }

        public static void Damage(UnitEntity target, float power, BattleEntity battle)
        {
            Change(target, -power, null, null, battle);
        }

        public static void Damage(UnitEntity target, float power, UnitEntity damager, ItemEntity item, BattleEntity battle)
        {
            Change(target, -power, damager, item, battle);
        }

        static void Change(UnitEntity target, float delta, UnitEntity changer, ItemEntity item, BattleEntity battle)
        {
            if (target == null || target.Health == null)
            {
                Log.W("Can't change health to unknown target");
                return;
            }
            var convertedDelta = delta;
            if (battle.Level.Health)
            {
                convertedDelta = battle.Level.Health.Handle(delta, target, changer, item);
            }
            target.Health.Change(convertedDelta);
            if (target.IsDead)
            {
                target.Data.GetDiedAction().Handle(target, changer, battle);
            }
        }
    }
}
