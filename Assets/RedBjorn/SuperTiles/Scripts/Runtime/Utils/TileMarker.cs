using RedBjorn.Utils;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    public class TileMarker : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer Icon;
        [SerializeField]
        float AlphaSpeed = 2f;
        [SerializeField]
        float DestroyTime = 2f;
        float Direction = 1f;

        float RestTime;

        public static TileMarker ShowInvalid(Vector3 position)
        {
            var hex = Spawner.Spawn(S.Prefabs.TileInvalid, position, Quaternion.identity);
            return hex;
        }

        void Start()
        {
            RestTime = DestroyTime;
            if (!Icon)
            {
                Icon = GetComponentInChildren<SpriteRenderer>(includeInactive: true);
            }
            if (!Icon)
            {
                enabled = false;
                Spawner.Despawn(gameObject);
            }
        }

        void Update()
        {
            var dt = Time.deltaTime;
            if (RestTime >= 0)
            {
                var alpha = Icon.color.a + Direction * dt * AlphaSpeed;
                if (alpha <= 0f)
                {
                    Direction = 1f;
                    alpha = 0f;
                }
                else if (alpha >= 1f)
                {
                    Direction = -1f;
                    alpha = 1f;
                }
                Icon.color = new Color(Icon.color.r, Icon.color.g, Icon.color.b, alpha);
                RestTime -= dt;
            }
            else
            {
                enabled = false;
                Spawner.Despawn(gameObject);
            }
        }
    }
}