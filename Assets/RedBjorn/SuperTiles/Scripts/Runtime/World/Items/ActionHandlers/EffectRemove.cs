using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    /// <summary>
    /// ItemAction logic which remove effects from targets
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Handlers.EffectRemove)]
    public class EffectRemove : ActionHandler
    {
        public TransformTag HolderTag;
        public string AnimatorState;
        public List<EffectData> Effects = new List<EffectData>();

        public override IEnumerator DoHandle(ItemAction data, BattleEntity battle)
        {
            var item = data.Item;
            var owner = data.Unit;
            var position = data.Position;
            var targets = item.Data.Selector.SelectTargets(item, owner.WorldPosition, position, owner, battle);
            var holder = owner.View.GetTransformHolder(HolderTag);

            // Create item model
            GameObject model = null;
            if (item.Data.Visual.Model)
            {
                model = Spawner.Spawn(item.Data.Visual.Model, holder);
            }

            //Play animator state
            if (!string.IsNullOrEmpty(AnimatorState))
            {
                var animator = owner.View.GetComponentInChildren<UnitAnimator>();
                if (animator)
                {
                    animator.PlayState(AnimatorState);
                }
            }

            //Remove status effects
            foreach (var target in targets)
            {
                foreach (var effect in Effects)
                {
                    yield return target.RemoveEffect(effect, battle);
                }
            }

            //Destroy item model
            if (model)
            {
                Spawner.Despawn(model);
            }
        }
    }
}