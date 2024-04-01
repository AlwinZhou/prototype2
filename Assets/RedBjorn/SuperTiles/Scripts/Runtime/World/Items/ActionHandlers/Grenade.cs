using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    /// <summary>
    /// Logic of ItemAction which spawn projectile in target position
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Handlers.Grenade)]
    public class Grenade : ActionHandler
    {
        public ItemStatTag StatPower;
        public ItemStatTag StatPowerMax;
        public bool PowerRandom;
        public ItemStatTag StatWarmUp;
        public ItemStatTag ProjectileSpeed;
        public TransformTag HolderTag;
        public ProjectileView Projectile;
        public TransformTag ProjectileTag;
        public string AnimatorState;
        public AudioClip FireSound;
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
            Transform projectileLaunch = null;
            if (item.Data.Visual.Model)
            {
                model = Spawner.Spawn(item.Data.Visual.Model, holder);
                projectileLaunch = model.GetComponentsInChildren<TransformTagHolder>()
                                        .Where(h => h.Tag == ProjectileTag)
                                        .Select(h => h.transform)
                                        .FirstOrDefault();
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
            yield return new WaitForSeconds(item[StatWarmUp]);

            AudioController.PlaySound(FireSound);

            //Spawn projectile
            if (Projectile)
            {
                var projectileCount = 1;
                Action onProjectileReached = () =>
                {
                    foreach (var t in targets)
                    {
                        UnitEntityActions.Damage(t, power, owner, item, battle);
                    }
                };
                Action onProjectileDestroyed = () => projectileCount--;

                var holderPosition = owner.WorldPosition;
                var holderRotation = owner.Rotation;
                if (projectileLaunch)
                {
                    holderPosition = projectileLaunch.position;
                    holderRotation = projectileLaunch.rotation;
                }
                else if (holder)
                {
                    holderPosition = holder.position;
                    holderRotation = holder.rotation;
                }

                var projectileSpeed = item[ProjectileSpeed];
                var speed = Mathf.Approximately(projectileSpeed, 0f) ? 10f : projectileSpeed;
                var projectile = Spawner.Spawn(Projectile, holderPosition, holderRotation);
                projectile.FireTarget(position, speed, battle.Map, onProjectileReached, onProjectileDestroyed);
                while (projectileCount > 0)
                {
                    yield return null;
                }
            }
            else
            {
                Log.W($"{typeof(Grenade).Name} of {item} has no projectile prefab. Projectile handling will be skipped");
                foreach (var target in targets)
                {
                    UnitEntityActions.Damage(target, power, owner, item, battle);
                }
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