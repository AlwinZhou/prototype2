using RedBjorn.SuperTiles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.ProtoTiles
{
    /// <summary>
    /// A bunch of helper methods to operate with units on map
    /// </summary>
    public partial class MapEntity
    {
        public UnitEntity NearestUnit(Vector3 position, IEnumerable<UnitEntity> units)
        {
            if (units != null)
            {
                return units.OrderBy(u => Distance(position, u.WorldPosition))
                            .ThenBy(u => u.Id)
                            .FirstOrDefault();
            }
            return null;
        }

        public void RegisterUnit(UnitEntity unit)
        {
            var tile = Tiles.TryGetOrDefault(unit.TilePosition);
            if (tile != null)
            {
                tile.RegisterUnit(unit);
            }
        }

        public void UnRegisterUnit(UnitEntity unit)
        {
            var tile = Tiles.TryGetOrDefault(unit.TilePosition);
            if (tile != null)
            {
                tile.UnregisterUnit(unit);
            }
        }

        public void Clear()
        {
            foreach (var t in Tiles)
            {
                t.Value.UnregisterUnits();
            }
        }
    }
}
