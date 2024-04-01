using UnityEngine;

namespace RedBjorn.SuperTiles.Effects
{
    /// <summary>
    /// Interface for EffectHandler creation from string type
    /// </summary>
    public interface EffectHandlerCreator
    {
        EffectHandler Create(EffectData effect, string type);
    }

    namespace Handlers
    {
        /// <summary>
        /// Default creator for EffectHandler with initial values
        /// </summary>
        public class DefaultCreator : EffectHandlerCreator
        {
            public EffectHandler Create(EffectData effect, string type)
            {
                return ScriptableObject.CreateInstance(type) as EffectHandler;
            }
        }

        /// <summary>
        /// Creator class for Damage EffectHandler
        /// </summary>
        public class DamageCreator : EffectHandlerCreator
        {
            public EffectHandler Create(EffectData effect, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as Damage;
                var stats = S.Battle.Tags.Effect;
                handler.PowerTag = stats.Power;
                return handler;
            }
        }

        /// <summary>
        /// Creator class for FxPlay EffectHandler
        /// </summary>
        public class FxPlayCreator : EffectHandlerCreator
        {
            public EffectHandler Create(EffectData effect, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as FxPlay;
                handler.Duration = 1f;
                return handler;
            }
        }

        /// <summary>
        /// Creator class for UnitStatChange EffectHandler
        /// </summary>
        public class UnitStatChangeCreator : EffectHandlerCreator
        {
            public EffectHandler Create(EffectData effect, string type)
            {
                var handler = ScriptableObject.CreateInstance(type) as UnitStatChange;
                handler.ChangeMultiplicator = true;
                return handler;
            }
        }
    }
}
