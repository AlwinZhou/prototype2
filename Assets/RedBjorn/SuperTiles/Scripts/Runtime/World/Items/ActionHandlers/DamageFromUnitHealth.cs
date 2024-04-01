using RedBjorn.SuperTiles.Battle.Actions;
using RedBjorn.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.ActionHandlers
{
    public class DamageFromUnitHealth : ActionHandler
    {
        public ItemStatTag VictimMultiplayer;
        public ItemStatTag OwnerMultiplier;
        public bool Round;
        public TransformTag HolderTag;
        public string AnimatorState;
        public AudioClip FireSound;

        public override IEnumerator DoHandle(ItemAction data, BattleEntity battle)
        {
            var item = data.Item;
            var owner = data.Unit;
            var position = data.Position;
            var holder = owner.View.GetTransformHolder(HolderTag);
            var targets = item.Data.Selector.SelectTargets(item, owner.WorldPosition, position, owner, battle);
            var ownerPower = owner.Health.ValueCurrent * item[VictimMultiplayer].Result;
            if (Round)
            {
                ownerPower = Mathf.Round(ownerPower);
            }
            ownerPower = Mathf.Max(1f, ownerPower);

            GameObject model = null;
            if (item.Data.Visual.Model)
            {
                model = Spawner.Spawn(item.Data.Visual.Model, holder);
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

            AudioController.PlaySound(FireSound);

            var targetHealth = 0f;
            foreach (var target in targets)
            {
                UnitEntityActions.Damage(target, ownerPower, owner, item, battle);
                targetHealth += target.Health.ValueCurrent;
            }
            var targetPower = targetHealth * item[OwnerMultiplier].Result;
            if (Round)
            {
                targetPower = Mathf.Round(targetPower);
            }
            targetPower = Mathf.Max(1f, targetPower);
            UnitEntityActions.Damage(owner, targetPower, owner, item, battle);
            
            //Destroy item model
            if (model)
            {
                Spawner.Despawn(model);
            }
        }
    }
}
