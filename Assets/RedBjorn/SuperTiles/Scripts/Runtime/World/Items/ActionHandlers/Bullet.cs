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
    /// Logic of ItemAction which spawn projectile in all target unit directions
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Handlers.Bullet)]
    public class Bullet : ActionHandler
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
        public ItemFxData FxHit;
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
            if (Projectile)
            {
                var projectileSpeed = item[ProjectileSpeed];
                var speed = Mathf.Approximately(projectileSpeed, 0f) ? 10f : projectileSpeed;
                var fxs = new Dictionary<GameObject, float>();
                var projectileCount = 0;

                Action<Vector3> onProjectileReached = (targetPos) =>
                {
                    if (FxHit.Prefab != null)
                    {
                        var fx = Spawner.Spawn(FxHit.Prefab, targetPos, Quaternion.identity);
                        fxs.Add(fx, Time.realtimeSinceStartup + FxHit.Duration);
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
                
                if (targets.Any())
                {
                    foreach (var target in targets)
                    {
                        projectileCount++;
                        var projectile = Spawner.Spawn(Projectile, holderPosition, holderRotation);
                        var targetPos = battle.Map.Settings.Projection(target.WorldPosition, holderPosition);
                        projectile.FireTarget(targetPos,
                            speed,
                            battle.Map,
                            () =>
                            {
                                var victim = target;
                                UnitEntityActions.Damage(victim, power, owner, item, battle);
                                onProjectileReached(targetPos);
                            },
                            onProjectileDestroyed);
                    }
                }
                else
                {
                    projectileCount++;
                    var projectile = Spawner.Spawn(Projectile, holderPosition, holderRotation);
                    projectile.FireTarget(position, speed, battle.Map, () => onProjectileReached(position), onProjectileDestroyed);
                }

                var removeFxs = new List<GameObject>();
                while (projectileCount > 0 || fxs.Count > 0)
                {
                    yield return null;
                    foreach (var kv in fxs)
                    {
                        if (kv.Value < Time.realtimeSinceStartup)
                        {
                            removeFxs.Add(kv.Key);
                            Spawner.Despawn(kv.Key);
                        }
                    }
                    foreach (var f in removeFxs)
                    {
                        fxs.Remove(f);
                    }
                    removeFxs.Clear();
                }
            }
            else
            {
                Log.W($"{typeof(Bullet).Name} of {item} has no projectile prefab. Projectile handling will be skipped");
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
