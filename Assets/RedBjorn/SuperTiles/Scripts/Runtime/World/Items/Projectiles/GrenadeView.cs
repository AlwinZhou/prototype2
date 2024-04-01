using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items.Projectiles
{
    /// <summary>
    /// Projectile view with parabolic movement trajectory from instantiated point to target point
    /// </summary>
    public class GrenadeView : ProjectileView
    {
        public float InitSpeedXZ = 1f;
        public GameObject ModelPrefab;
        public GameObject FXPrefab;
        public float FXDuration = 2f;

        const float constG = 9.8f;

        GameObject Model;
        GameObject FX;

        Action OnReached;
        Action OnDestroyed;

        void Awake()
        {
            if (ModelPrefab)
            {
                Model = Spawner.Spawn(ModelPrefab, transform);
                Model.transform.localPosition = Vector3.zero;
                Model.transform.localRotation = Quaternion.identity;
                Model.SetActive(false);
            }

            if (FXPrefab)
            {
                FX = Spawner.Spawn(FXPrefab, transform);
                FX.transform.localPosition = Vector3.zero;
                FX.transform.localRotation = Quaternion.identity;
                FX.SetActive(false);
            }
        }

        public override void FireTarget(Vector3 target, float speed, MapEntity map, Action onReached, Action onDestroyed)
        {
            OnReached = onReached;
            OnDestroyed = onDestroyed;
            StartCoroutine(GrenadeLife(target, speed, map));
        }

        IEnumerator GrenadeLife(Vector3 target, float speed, MapEntity map)
        {
            if (Model)
            {
                Model.SetActive(true);
            }
            var speedInitial = speed;
            if (speedInitial < 0.0001f)
            {
                speedInitial = InitSpeedXZ;
            }

            var origin = transform.position;
            var distance = map.Settings.Projection(target - origin).magnitude;
            var timeMax = distance / speedInitial;
            var speedMain = map.Settings.AxisMainGet(target - origin) * speedInitial / distance;
            var speedSecondary = map.Settings.AxisSecondaryGet(target - origin) * speedInitial / distance;
            var speedOrthogonal = constG * timeMax / 2f;

            var time = 0f;
            while (time <= timeMax)
            {
                var oldPosition = transform.position;
                var main = speedMain * time;
                var secondary = speedSecondary * time;
                var ortho = speedOrthogonal * time - constG / 2f * time * time;
                transform.position = origin + map.Settings.VectorCreate(main, secondary, ortho);
                transform.localRotation = map.Settings.LookAt(transform.position - oldPosition);
                yield return null;
                time += Time.deltaTime;
            }
            if (Model)
            {
                Model.SetActive(false);
            }
            transform.position = target;
            OnReached.SafeInvoke();
            if (FX)
            {
                FX.SetActive(true);
            }
            yield return new WaitForSeconds(FXDuration);
            Spawner.Despawn(gameObject);
            OnDestroyed.SafeInvoke();
        }
    }
}