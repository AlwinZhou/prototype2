using RedBjorn.ProtoTiles;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles.Items
{
    /// <summary>
    /// View of projectile gameobject 
    /// </summary>
    public abstract class ProjectileView : MonoBehaviour
    {
        public abstract void FireTarget(Vector3 target, float speed, MapEntity map, Action OnReached, Action onDestroyed);
    }
}
