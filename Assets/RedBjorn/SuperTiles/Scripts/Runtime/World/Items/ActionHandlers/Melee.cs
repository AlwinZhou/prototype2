using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    /// <summary>
    /// Logic of ItemAction which damage all target units
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Handlers.Melee)]
    public class Melee : ActionHandler
    {
        public ItemStatTag StatPower;
        public ItemStatTag StatPowerMax;
        public bool PowerRandom;
        public TransformTag HolderTag;
        public string AnimatorState;
        public AudioClip AttackSound;
        public float DurationStart = 0.2f;
        public float DurationFinish = 0.5f;
        public ItemStatTag EffectAddDuration;
        public List<EffectData> EffectsAdd = new List<EffectData>();
        public List<EffectData> EffectsRemove = new List<EffectData>();

        public override IEnumerator DoHandle(ItemAction data, BattleEntity battle)
        {
            var item = data.Item;
            var owner = data.Unit;
            var position = data.Position;
            var targets = item.Data.Selector.SelectTargets(item, owner.WorldPosition, position, owner, battle);
            var holder = owner.View.GetTransformHolder(HolderTag);
            var power = item[StatPower].Result;
            var powerMax = item[StatPowerMax].Result;
            if (PowerRandom && powerMax > power)
            {
                power = battle.Randomizer.Next(Mathf.RoundToInt(power), Mathf.RoundToInt(powerMax));
            }
            // Create item model
            GameObject model = null;
            if (item.Data.Visual.Model)
            {
                model = Spawner.Spawn(item.Data.Visual.Model, holder);
            }

            //Rotate unit to target
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
            yield return new WaitForSeconds(DurationStart); //Start delay

            AudioController.PlaySound(AttackSound);

            //Deal damage
            foreach (var t in targets)
            {
                UnitEntityActions.Damage(t, power, owner, item, battle);
            }
            yield return new WaitForSeconds(DurationFinish); //Finish delay

            //Add status effects
            if (EffectAddDuration)
            {
                var effectDuration = item[EffectAddDuration];
                if (effectDuration > 0)
                {
                    foreach (var target in targets)
                    {
                        foreach (var effect in EffectsAdd)
                        {
                            yield return target.AddEffect(effect, effectDuration, battle);
                        }
                    }
                }
            }

            //Remove status effects
            foreach (var target in targets)
            {
                foreach (var effect in EffectsRemove)
                {
                    yield return target.RemoveEffect(effect, battle);
                }
            }

            //Despawn item model
            if (model)
            {
                Spawner.Despawn(model);
            }
        }
    }
}
