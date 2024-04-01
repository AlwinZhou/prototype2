using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items
{
    /// <summary>
    /// Base class for item logic of target selection 
    /// </summary>
    public abstract class TargetSelector : ScriptableObjectExtended
    {
        /// <summary>
        /// Select possible unit targets which can be selected by current item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="attackPosition"></param>
        /// <param name="battle"></param>
        /// <returns></returns>
        public abstract IEnumerable<UnitEntity> PossibleTargets(ItemEntity item, Vector3 attackPosition, BattleEntity battle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="origin">Origin position for selected item</param>
        /// <param name="finish">Target position for selected item</param>
        /// <param name="owner">Item owner</param>
        /// <param name="battle"></param>
        /// <returns>Unit targets</returns>
        public abstract IEnumerable<UnitEntity> SelectTargets(ItemEntity item, Vector3 origin, Vector3 finish, UnitEntity owner, BattleEntity battle);

        /// <summary>
        /// Create view as a container for ItemAction handling
        /// </summary>
        /// <param name="initContext"></param>
        /// <param name="onCompleted"></param>
        /// <param name="battle"></param>
        /// <returns></returns>
        public abstract ITargetSelectorView StartActivation(ItemAction initContext, Action<ItemAction> onCompleted, BattleView battle);

        /// <summary>
        /// Check if target position is valid for current values: item, origin, owner and battle
        /// </summary>
        /// <param name="item"></param>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="owner"></param>
        /// <param name="battle"></param>
        /// <param name="validTarget">valid target position</param>
        /// <returns>true, if valid position can be calculated; false, otherwise</returns>
        public abstract bool ValidatePosition(ItemEntity item, Vector3 origin, Vector3 target, UnitEntity owner, BattleEntity battle, out Vector3 validTarget);

#if UNITY_EDITOR
        public static System.Type GetCreatorType(string typeName)
        {
            var type = typeof(TargetSelector);
            var creatorName = string.Concat(type.Namespace, ".", nameof(TargetSelectors), ".", typeName, "Creator");
            var creator = type.Assembly.GetType(creatorName);
            if (creator == null)
            {
                Log.W($"No creator class [{creatorName}] for TargetSelector ({typeName}). Default will be selected");
                creator = typeof(TargetSelectors.DefaultCreator);
            }
            return creator;
        }
#endif
    }
}
