using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.ItemConditions
{
    public class And : IItemCondition
    {
        public List<IItemCondition> List;

        public bool IsMet(ItemEntity item, UnitEntity owner)
        {
            return List != null ? List.All(l => l.IsMet(item, owner)) : false;
        }
    }

    public class ProxyUnit : IItemCondition
    {
        public IUnitCondition Unit;

        public bool IsMet(ItemEntity item, UnitEntity owner)
        {
            return Unit.IsMet(owner, owner.Game.Battle);
        }
    }

    public class OwnerAloneInTile : IItemCondition
    {
        public bool IsMet(ItemEntity item, UnitEntity owner)
        {
            var tile = owner.Game.Battle.Map.Tile(owner.TilePosition);
            return tile.Units.Count() == 1;
        }
    }

    public class ResourceAvailability : IItemCondition
    {
        public ResourceTag Resource;
        public ItemStatTag Cost;

        public bool IsMet(ItemEntity item, UnitEntity owner)
        {
            var player = owner.Game.Battle.Players.FirstOrDefault(p => p.Squad.Contains(owner));
            if (player != null)
            {
                return player[Resource] >= item[Cost].Result;
            }
            return false;
        }
    }
}
