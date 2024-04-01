using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Interface for ActionRules creation from string type
    /// </summary>
    public interface ActionRulesCreator
    {
        ActionRules Create(string type);
    }

    namespace ActionRule
    {
        /// <summary>
        /// Default creator for ActionRules with initial values
        /// </summary>
        public class DefaultCreator : ActionRulesCreator
        {
            public ActionRules Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as ActionRules;
            }
        }

        /// <summary>
        /// Creator class for HitAfterRun ActionRules
        /// </summary>
        public class HitAfterRunCreator : ActionRulesCreator
        {
            public ActionRules Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as HitAfterRun;
            }
        }

        /// <summary>
        /// Creator class for HitAndRun ActionRules
        /// </summary>
        public class HitAndRunCreator : ActionRulesCreator
        {
            public ActionRules Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as HitAndRun;
            }
        }

        /// <summary>
        /// Creator class for ActionPointsTwo ActionRules
        /// </summary>
        public class ActionPointsTwoCreator : ActionRulesCreator
        {
            public ActionRules Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as ActionPointsTwo;
            }
        }

        /// <summary>
        /// Creator class for ActionPointsCustom ActionRules
        /// </summary>
        public class ActionPointsCustomCreator : ActionRulesCreator
        {
            public ActionRules Create(string type)
            {
                return ScriptableObject.CreateInstance(type) as ActionPointsCustom;
            }
        }
    }
}
