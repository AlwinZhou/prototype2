using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    /// <summary>
    /// Logic of ItemAction which spawn single projectile in target direction and damaging victims according to distance
    /// </summary>
    [CreateAssetMenu(menuName = Paths.ScriptablePath.Item.Handlers.Laser)]
    public class Laser : ActionHandler
    {
        public class Info
        {
            public float Time;
            public UnitEntity Target;
        }

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
        public GameObject Fx;
        public ItemStatTag EffectAddDuration;
        public List<EffectData> EffectsAdd = new List<EffectData>();
        public List<EffectData> EffectsRemove = new List<EffectData>();

        public override IEnumerator DoHandle(ItemAction data, BattleEntity battle)
        {
            var item = data.Item;
            var owner = data.Unit;
            var position = data.Position;
            var targets = item.Data.Selector.SelectTargets(item, owner.WorldPosition, position, owner, battle).ToList();
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

            //Create Fx
            GameObject fx = null;
            if (Fx)
            {
                fx = Spawner.Spawn(Fx, holderPosition, holderRotation);
            }

            //Spawn projectile
            if (Projectile)
            {
                var projectileSpeed = item[ProjectileSpeed];
                var speed = Mathf.Approximately(projectileSpeed, 0f) ? 10f : projectileSpeed;
                var queue = new Queue<Info>();
                foreach (var target in targets.OrderBy(t => Vector3.SqrMagnitude(t.WorldPosition - holderPosition)))
                {
                    queue.Enqueue(new Info { Target = target, Time = Vector3.Magnitude(target.WorldPosition - holderPosition) / speed });
                }
                var firing = true;
                var projectile = Spawner.Spawn(Projectile, holderPosition, holderRotation);
                projectile.FireTarget(position, projectileSpeed, battle.Map, null, () =>
                {
                    firing = false;
                    while (queue.Count > 0)
                    {
                        var info = queue.Dequeue();
                        UnitEntityActions.Damage(info.Target, power, owner, item, battle);
                    }
                });

                var time = 0f;
                if (queue.Count > 0)
                {
                    var info = queue.Peek();
                    while (firing)
                    {
                        while (info != null && info.Time <= time)
                        {
                            UnitEntityActions.Damage(info.Target, power, owner, item, battle);
                            queue.Dequeue();
                            if (queue.Count > 0)
                            {
                                info = queue.Peek();
                            }
                            else
                            {
                                info = null;
                            }
                        }
                        yield return null;
                        time += Time.deltaTime;
                    }
                }
                else
                {
                    while (firing)
                    {
                        yield return null;
                        time += Time.deltaTime;
                    }
                }
            }
            else
            {
                Log.W($"{typeof(Laser).Name} of {item} has no projectile prefab. Projectile handling will be skipped");
                foreach (var target in targets)
                {
                    UnitEntityActions.Damage(target, power, owner, item, battle);
                }
            }

            //Destroy Fx
            if (fx)
            {
                Spawner.Despawn(fx);
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