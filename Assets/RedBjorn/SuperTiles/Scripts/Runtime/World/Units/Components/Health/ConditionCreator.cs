using UnityEngine;

namespace RedBjorn.SuperTiles.Health
{
    /// <summary>
    /// Interface for Condition creation from string type
    /// </summary>
    public interface ConditionCreator
    {
        Condition Create(ConvertRule rule, string type);
    }

    namespace Conditions
    {
        /// <summary>
        /// Default creator for Condition with initial values
        /// </summary>
        public class DefaultCreator : ConditionCreator
        {
            public Condition Create(ConvertRule rule, string type)
            {
                return ScriptableObject.CreateInstance(type) as Condition;
            }
        }

        /// <summary>
        /// Creator class for HaveEffect Condition
        /// </summary>
        public class HaveEffectCreator : ConditionCreator
        {
            public Condition Create(ConvertRule rule, string type)
            {
                var condition = ScriptableObject.CreateInstance(type) as HaveEffect;
                condition.ConvertDamage = true;
                return condition;
            }
        }
    }
}


