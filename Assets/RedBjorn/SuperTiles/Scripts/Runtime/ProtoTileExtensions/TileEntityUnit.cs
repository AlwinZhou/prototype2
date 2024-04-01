using RedBjorn.SuperTiles;
using System.Collections.Generic;
using System.Linq;

namespace RedBjorn.ProtoTiles
{
    /// <summary>
    /// Extension of TileEntity for unit handling
    /// </summary>
    public partial class TileEntity
    {
        List<UnitEntity> UnitsInternal = new List<UnitEntity>();
        public IEnumerable<UnitEntity> Units { get { return UnitsInternal; } }
        public bool HasUnit { get { return UnitsInternal != null && UnitsInternal.Count > 0; } }

        public void RegisterUnit(UnitEntity unit)
        {
            if (UnitsInternal == null)
            {
                UnitsInternal = new List<UnitEntity>();
            }
            if (unit != null)
            {
                UnitsInternal.Add(unit);
                if (!unit.Incorporeal)
                {
                    ObstacleCount++;
                }
            }
        }

        public void UnregisterUnit(UnitEntity unit)
        {
            if (UnitsInternal == null)
            {
                UnitsInternal = new List<UnitEntity>();
            }
            if (unit != null)
            {
                if (UnitsInternal.Remove(unit) && !unit.Incorporeal)
                {
                    ObstacleCount--;
                }
            }
        }

        public void UnregisterUnits()
        {
            if (UnitsInternal != null)
            {
                foreach (var unit in UnitsInternal.Where(u => !u.Incorporeal))
                {
                    ObstacleCount--;
                }
                UnitsInternal.Clear();
            }
        }
    }
}

