using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    /// <summary>
    /// Logic of ItemAction which teleport unit to target position
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Handlers.Teleport)]
    public class Teleporter : ActionHandler
    {
        public float HideDiration;
        public ItemFxData FxStart;
        public float ShowDuration;
        public ItemFxData FxFinish;
        public string AnimatorState;
        public TransformTag HolderTag;
        public ItemStatTag EffectAddDuration;
        public List<EffectData> EffectsAdd = new List<EffectData>();
        public List<EffectData> EffectsRemove = new List<EffectData>();

        public override IEnumerator DoHandle(ItemAction data, BattleEntity battle)
        {
            var item = data.Item;
            var owner = data.Unit;
            var position = data.Position;
            var holder = owner.View.GetTransformHolder(HolderTag);

            // Create item model
            GameObject model = null;
            if (item.Data.Visual.Model)
            {
                model = Spawner.Spawn(item.Data.Visual.Model, holder);
            }

            //Rotate unit to target
            position = battle.Map.TileCenter(position);
            yield return owner.LookingAt(position);

            //Play animator state
            if (!string.IsNullOrEmpty(AnimatorState))
            {
                var animator = owner.View.GetComponentInChildren<UnitAnimator>();
                if (animator)
                {
                    animator.PlayState(AnimatorState);
                }
            }

            //Create Fx at Unit position
            var restFx = Mathf.Max(0f, FxStart.Duration - HideDiration);
            var fx = Spawner.Spawn(FxStart.Prefab, owner.WorldPosition, Quaternion.identity);
            yield return new WaitForSeconds(HideDiration);
            owner.ViewHide(); //Hide unit model
            battle.Map.UnRegisterUnit(owner);
            yield return new WaitForSeconds(restFx);
            Spawner.Despawn(fx);

            //Create Fx at target position
            restFx = Mathf.Max(0f, FxFinish.Duration - ShowDuration);
            fx = Spawner.Spawn(FxFinish.Prefab, position, Quaternion.identity);
            yield return new WaitForSeconds(ShowDuration);
            owner.WorldPosition  = position;
            owner.ViewShow(); //Show unit model
            battle.Map.RegisterUnit(owner);
            yield return new WaitForSeconds(restFx);
            Spawner.Despawn(fx);

            //Add status effects
            if (EffectAddDuration)
            {
                var effectDuration = item[EffectAddDuration];
                if (effectDuration > 0)
                {
                    foreach (var effect in EffectsAdd)
                    {
                        yield return owner.AddEffect(effect, effectDuration, battle);
                    }
                }
            }

            //Remove status effects
            foreach (var effect in EffectsRemove)
            {
                yield return owner.RemoveEffect(effect, battle);
            }

            //Destroy item model
            if (model)
            {
                Spawner.Despawn(model);
            }
        }
    }
}