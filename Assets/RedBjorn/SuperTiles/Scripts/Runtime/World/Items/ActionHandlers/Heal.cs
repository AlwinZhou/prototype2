using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    /// <summary>
    /// Logic of ItemAction which spawn fx in target positions and heal target units
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Handlers.Heal)]
    public class Heal : ActionHandler
    {
        public ItemStatTag StatPower;
        public ItemStatTag StatPowerMax;
        public bool PowerRandom;
        public ItemStatTag StatWarmUp;
        public TransformTag HolderTag;
        public string AnimatorState;
        public AudioClip Sound;
        public ItemFxData Fx;
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

            //Play animator state
            if (!string.IsNullOrEmpty(AnimatorState))
            {
                var animator = owner.View.GetComponentInChildren<UnitAnimator>();
                if (animator)
                {
                    animator.PlayState(AnimatorState);
                }
            }
            yield return new WaitForSeconds(item[StatWarmUp]);

            AudioController.PlaySound(Sound);

            //Create heal fx
            var fxs = new List<GameObject>();
            foreach (var target in targets)
            {
                UnitEntityActions.Heal(target, power, owner, item, battle);
                var fx = Spawner.Spawn(Fx.Prefab, target.WorldPosition, Quaternion.identity);
                fxs.Add(fx);
            }
            yield return new WaitForSeconds(Fx.Duration);

            //Destroy heal fx
            for (int i = fxs.Count - 1; i >= 0; i--)
            {
                Spawner.Despawn(fxs[i]);
            }

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

            //Destroy item model
            if (model)
            {
                Spawner.Despawn(model);
            }
        }
    }
}
