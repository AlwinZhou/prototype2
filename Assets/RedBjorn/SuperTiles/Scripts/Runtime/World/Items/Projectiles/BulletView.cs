using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.Projectiles
{
    /// <summary>
    /// Projectile view with staightforward movement behaviour from instantiated point to target point
    /// </summary>
    public class BulletView : ProjectileView
    {
        public float Speed = 1f;

        Action OnReached;
        Action OnDestroyed;

        public override void FireTarget(Vector3 target, float speed, MapEntity map, Action onReached, Action onDestroyed)
        {
            OnReached = onReached;
            OnDestroyed = onDestroyed;
            StartCoroutine(ReachTarget(target, speed));
        }

        IEnumerator ReachTarget(Vector3 target, float speed)
        {
            var speedInitial = speed;
            if (speedInitial < 0.0001f)
            {
                speedInitial = Speed;
            }
            var directionOrigin = (target - transform.position).normalized;
            var directionCurrent = directionOrigin;
            var step = directionOrigin * speedInitial;
            while (Vector3.Dot(directionOrigin, directionCurrent) > 0)
            {
                yield return null;
                transform.position += step * Time.deltaTime;
                directionCurrent = target - transform.position;
            }
            OnReached.SafeInvoke();
            Spawner.Despawn(gameObject);
            OnDestroyed.SafeInvoke();
        }
    }
}
