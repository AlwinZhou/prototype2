using System.Collections.Generic;
using System.Linq;

namespace RedBjorn.SuperTiles.BattleActions
{
    public class Multiple : IBattleAction
    {
        public List<IBattleAction> List;

        public void Handle(BattleEntity battle)
        {
            if (List != null)
            {
                foreach(var action in List)
                {
                    if (action != null)
                    {
                        action.Handle(battle);
                    }
                }
            }
        }
    }
    
    public class ResourcesControllerAdd : IBattleAction
    {
        public ISquadControllerSelector ControllerSelector;
        public ResourceTag Resource;
        public IFloatProvider ResourceAmount;

        public void Handle(BattleEntity battle)
        {
            var amount = ResourceAmount.Get(battle);
            foreach (var controller in ControllerSelector.Select(battle))
            {
                controller.Resources[Resource] += amount;
            }
        }
    }

    public class UnitHealOtherUnit : IBattleAction
    {
        public IUnitSelector Target;
        public IFloatProvider Amount;
        public IUnitSelector Healer;

        public void Handle(BattleEntity battle)
        {
            var amount = Amount.Get(battle);
            var healer = Healer != null ? Healer.Select(battle).FirstOrDefault() : null;
            foreach (var target in Target.Select(battle))
            {
                UnitEntityActions.Heal(target, amount, healer, null, battle);
            }
        }
    }
}
