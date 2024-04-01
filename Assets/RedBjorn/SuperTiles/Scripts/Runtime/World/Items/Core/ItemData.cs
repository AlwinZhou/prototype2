using RedBjorn.SuperTiles.Items;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Item informational storage 
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Asset)]
    public class ItemData : ScriptableObjectExtended
    {
        [Serializable]
        public class StatData
        {
            public ItemStatTag Stat;
            public float Value;
        }

        [Serializable]
        public class AvailableConditionData 
        {
            public bool IsOwnerAloneInTile;
            [Space]
            public bool IsOwnerTileContains;
            public UnitTag OwnerTileContains;
            [Space]
            public ItemStatTag ItemCostTag;
            public ResourceTag ResourceTag;
        }

        [Serializable]
        public class VisualConfig
        {
            public GameObject Model;
            [Header("UI")]
            public Sprite IconSmall;
            public bool UseColor;
            public Color Color;
            [Header("Trajectory")]
            public Material TrajectoryMaterial;
            [Header("Selector")]
            public bool UseSelectorCustom;
            public GameObject SelectorCustom;
            public ProtoTiles.MapSettings.TileVisual SelectorGenerated;
            [Header("Available")]
            public bool UseAvailableCustom;
            public GameObject AvailableCustom;
            public ProtoTiles.MapSettings.TileVisual AvailableGenerated;
            [Header("Obsolete")]
            public Material SelectorMaterial;
        }

        public string Caption;
        [TextArea(2, 5)]
        public string Description;
        public bool Stackable;
        public int MaxStackCount = 1;

        [Space]
        public List<StatData> Stats = new List<StatData>();
        public List<ItemTag> Tags = new List<ItemTag>();
        public TargetSelector Selector;
        public ActionHandler ActionHandler;
        public AvailableConditionData AvailableCondition;
        public VisualConfig Visual;

        public IItemCondition GetCondition()
        {
            var condition = new ItemConditions.And() { List = new List<IItemCondition>() };
            if (AvailableCondition != null)
            {
                if (AvailableCondition.IsOwnerAloneInTile)
                {
                    condition.List.Add(new ItemConditions.OwnerAloneInTile());
                }
                if (AvailableCondition.IsOwnerTileContains)
                {
                    condition.List.Add(new ItemConditions.ProxyUnit()
                    {
                        Unit = new UnitConditions.Tile.UnitsSpecificNotAlly()
                        { 
                            SpecificUnit = new UnitConditions.DataTagsContains { Tag = AvailableCondition.OwnerTileContains },
                            CheckIfAnyone = true
                        }
                    });
                }
                if (AvailableCondition.ItemCostTag && AvailableCondition.ResourceTag)
                {
                    var resource = new ItemConditions.ResourceAvailability() 
                    { 
                        Cost = AvailableCondition.ItemCostTag, 
                        Resource = AvailableCondition.ResourceTag
                    };
                    condition.List.Add(resource);
                }
            }
            return condition;
        }
    }
}
