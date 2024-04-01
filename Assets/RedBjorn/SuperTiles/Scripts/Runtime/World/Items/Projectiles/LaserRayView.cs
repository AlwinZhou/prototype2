using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.Projectiles
{
    /// <summary>
    /// Projectile view with ray movement behaviour
    /// </summary>
    public class LaserRayView : ProjectileView
    {
        public float Speed = 10f;

        Action OnReached;
        Action OnDestroyed;

        public override void FireTarget(Vector3 target, float speed, MapEntity map, Action onReached, Action onDestroyed)
        {
            OnReached = onReached;
            OnDestroyed = onDestroyed;
            StartCoroutine(RayLife(target, speed));
        }

        IEnumerator RayLife(Vector3 target, float speed)
        {
            var speedInitial = speed;
            if (speedInitial < 0.0001f)
            {
                speedInitial = Speed;
            }
            var direction = target - transform.position;
            var directionOrigin = direction.normalized;
            var duration = direction.magnitude / speedInitial;
            while (duration > 0)
            {
                yield return null;
                duration -= Time.deltaTime;
                transform.position += directionOrigin * Time.deltaTime * speedInitial;
            }
            OnReached.SafeInvoke();
            Spawner.Despawn(gameObject);
            OnDestroyed.SafeInvoke();
        }
    }
}