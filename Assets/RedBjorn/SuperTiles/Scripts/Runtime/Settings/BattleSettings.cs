using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Settings
{
    /// <summary>
    /// Settings which contains information that helps during Battle
    /// </summary>
    public class BattleSettings : ScriptableObjectExtended
    {
        [Serializable]
        public class TagsSettings
        {
            [Serializable]
            public class ItemSettings
            {
                public ItemStatTag Cooldown;
                public ItemStatTag Range;
                public ItemStatTag AoeRange;
                public ItemStatTag Power;
                public ItemStatTag PowerMin;
                public ItemStatTag PowerMax;
                public ItemStatTag WarmUp;
                public ItemStatTag ProjectileSpeed;
                public ItemStatTag EffectDuration;

                public ItemTag Weapon;
                public ItemTag Device;
                public ItemTag Heal;
                public ItemTag Buff;
                public ItemTag Debuff;
            }

            [Serializable]
            public class UnitSettings
            {
                public UnitStatTag Health;
                public float HealthDefault;
                public UnitStatTag MoveRange;
                public float MoveRangeDefault;
                public UnitStatTag Speed;
                public float SpeedDefault;
                public UnitStatTag RotationSpeed;
                public float RotationSpeedDefault;

                public System.Collections.Generic.List<UnitData.StatData> GetDefault()
                {
                    var stats = new System.Collections.Generic.List<UnitData.StatData>();
                    stats.Add(new UnitData.StatData { Stat = S.Battle.Tags.Unit.Health, Value = HealthDefault });
                    stats.Add(new UnitData.StatData { Stat = S.Battle.Tags.Unit.MoveRange, Value = MoveRangeDefault });
                    stats.Add(new UnitData.StatData { Stat = S.Battle.Tags.Unit.Speed, Value = SpeedDefault });
                    stats.Add(new UnitData.StatData { Stat = S.Battle.Tags.Unit.RotationSpeed, Value = RotationSpeedDefault });
                    return stats;
                }
            }

            [Serializable]
            public class TransformSettings
            {
                public TransformTag ItemHolder;
                public TransformTag UiHolder;
                public TransformTag EffectHolder;
            }

            [Serializable]
            public class EffectSettings
            {
                public EffectStatTag Power;
            }

            [Serializable]
            public class SynonymSettings
            {
                [Serializable]
                public class Synonym
                {
                    public Tag Tag;
                    public string Text;
                }

                public List<Synonym> Data;

                public string Get(Tag tag)
                {
                    if (tag == null)
                    {
                        return string.Empty;
                    }
                    var t = Data.Find(d => d.Tag == tag);
                    return t == null ? tag.name : t.Text;
                }
            }

            public ItemSettings Item;
            public UnitSettings Unit;
            public TransformSettings Transform;
            public EffectSettings Effect;
            public SynonymSettings Synonym;
        }

        [Serializable]
        public class HealthSettings
        {
            public bool BarOnDeathHide;
            public Color BarOnDeathColor;
            public float BarSpeedFill;
        }

        [Serializable]
        public class UiSettings
        {
            public bool NicknameShow;
            public bool TurnStartShowMy;
            public bool TurnStartShowOther;
            public bool TurnShow;
            public bool StatusTextShow;
            public bool BattleFinishShow;
        }

        public bool AutoSave;
        public bool ShowHealthChange;
        public HealthSettings Health;
        public UiSettings UI;
        public Material Interactable;
        public Effects.EffectHandler EffectFxAddDefault;
        public Effects.EffectHandler EffectFxRemoveDefault;
        public TagsSettings Tags;
    }
}