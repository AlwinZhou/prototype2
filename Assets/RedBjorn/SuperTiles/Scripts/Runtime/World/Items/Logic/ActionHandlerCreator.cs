using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items
{
    /// <summary>
    /// Interface for ActionHandler creation from string type
    /// </summary>
    public interface ActionHandlerCreator
    {
        ActionHandler Create(ItemData item, string type);
    }

    namespace ActionHandlers
    {
        /// <summary>
        /// Default creator for ActionHandler with initial values
        /// </summary>
        public class DefaultCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                return ScriptableObject.CreateInstance(type) as ActionHandler;
            }
        }

        /// <summary>
        /// Creator class for Bullet ActionHandler
        /// </summary>
        public class BulletCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as Bullet;
                var stats = S.Battle.Tags;
                handler.StatPower = stats.Item.Power;
                handler.StatWarmUp = stats.Item.WarmUp;
                handler.ProjectileSpeed = stats.Item.ProjectileSpeed;
                handler.EffectAddDuration = stats.Item.EffectDuration;
                if (!item.Stats.Any(i => i.Stat == handler.StatPower))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatPower, Value = 100f });
                }
                if (!item.Stats.Any(i => i.Stat == handler.StatWarmUp))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatWarmUp, Value = 0.5f });
                }
                if (!item.Stats.Any(i => i.Stat == handler.ProjectileSpeed))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.ProjectileSpeed, Value = 100f });
                }
                handler.HolderTag = stats.Transform.ItemHolder;
                return handler;
            }
        }

        /// <summary>
        /// Creator class for Grenade ActionHandler
        /// </summary>
        public class GrenadeCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as Grenade;
                var stats = S.Battle.Tags;
                handler.StatPower = stats.Item.Power;
                handler.StatWarmUp = stats.Item.WarmUp;
                handler.ProjectileSpeed = stats.Item.ProjectileSpeed;
                handler.EffectAddDuration = stats.Item.EffectDuration;
                if (!item.Stats.Any(i => i.Stat == handler.StatPower))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatPower, Value = 100f });
                }
                if (!item.Stats.Any(i => i.Stat == handler.StatWarmUp))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatWarmUp, Value = 0.5f });
                }
                if (!item.Stats.Any(i => i.Stat == handler.ProjectileSpeed))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.ProjectileSpeed, Value = 100f });
                }
                handler.HolderTag = stats.Transform.ItemHolder;
                return handler;
            }
        }

        /// <summary>
        /// Creator class for Healt ActionHandler
        /// </summary>
        public class HealCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as Heal;
                var stats = S.Battle.Tags;
                handler.StatPower = stats.Item.Power;
                handler.StatWarmUp = stats.Item.WarmUp;
                handler.EffectAddDuration = stats.Item.EffectDuration;
                if (!item.Stats.Any(i => i.Stat == handler.StatPower))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatPower, Value = 100f });
                }
                if (!item.Stats.Any(i => i.Stat == handler.StatWarmUp))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatWarmUp, Value = 0.5f });
                }
                return handler;
            }
        }

        /// <summary>
        /// Creator class for Laser ActionHandler
        /// </summary>
        public class LaserCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as Laser;
                var stats = S.Battle.Tags;
                handler.StatPower = stats.Item.Power;
                handler.StatWarmUp = stats.Item.WarmUp;
                handler.ProjectileSpeed = stats.Item.ProjectileSpeed;
                handler.EffectAddDuration = stats.Item.EffectDuration;
                if (!item.Stats.Any(i => i.Stat == handler.StatPower))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatPower, Value = 100f });
                }
                if (!item.Stats.Any(i => i.Stat == handler.StatWarmUp))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatWarmUp, Value = 0.5f });
                }
                if (!item.Stats.Any(i => i.Stat == handler.ProjectileSpeed))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.ProjectileSpeed, Value = 100f });
                }
                handler.HolderTag = stats.Transform.ItemHolder;
                return handler;
            }
        }

        /// <summary>
        /// Creator class for Melee ActionHandler
        /// </summary>
        public class MeleeCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as Melee;
                var stats = S.Battle.Tags;
                handler.StatPower = stats.Item.Power;
                handler.EffectAddDuration = stats.Item.EffectDuration;
                if (!item.Stats.Any(i => i.Stat == handler.StatPower))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = handler.StatPower, Value = 100f });
                }
                return handler;
            }
        }

        /// <summary>
        /// Creator class for Teleport ActionHandler
        /// </summary>
        public class TeleporterCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as Teleporter;
                var stats = S.Battle.Tags;
                handler.EffectAddDuration = stats.Item.EffectDuration;
                if (!item.Stats.Any(i => i.Stat == stats.Item.Range))
                {
                    item.Stats.Add(new ItemData.StatData { Stat = stats.Item.Range, Value = 5f });
                }
                return handler;
            }
        }

        /// <summary>
        /// Creator class for EffectAdd ActionHandler
        /// </summary>
        public class EffectAddCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as EffectAdd;
                var stats = S.Battle.Tags;
                handler.Duration = stats.Item.EffectDuration;
                return handler;
            }
        }

        /// <summary>
        /// Creator class for EffectRemove ActionHandler
        /// </summary>
        public class EffectRemoveCreator : ActionHandlerCreator
        {
            public ActionHandler Create(ItemData item, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as EffectRemove;
                return handler;
            }
        }

    }
}
