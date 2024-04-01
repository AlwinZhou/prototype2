using RedBjorn.ProtoTiles;
using RedBjorn.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// View which represents UnitMoveEntity at unity scene
    /// </summary>
    public class UnitMoveView : MonoBehaviour
    {
        float Eps = 0.01f;
        Vector3 LookPoint;
        [NonSerialized]
        UnitEntity Unit;
        StatEntity MoveSpeed;
        StatEntity RotateSpeed;
        Coroutine MovingCoroutine;
        Coroutine RotateCoroutine;
        Action OnCompleted;

        MapEntity Map { get { return Unit.Game.Battle.Map; } }

        public void Init(UnitEntity unit, UnitStatTag moveSpeed, UnitStatTag rotateTag)
        {
            Unit = unit;
            MoveSpeed = Unit[moveSpeed];
            RotateSpeed = Unit[rotateTag];
        }

        public void Move(Vector3 point, float range, Action onCompleted)
        {
            var path = Map.PathTiles(transform.position, point, range);
            Action onMoveCompleted = () =>
            {
                UpdateUnit();
                onCompleted.SafeInvoke();
            };
            Move(path, onMoveCompleted);
        }

        void Move(List<TileEntity> path, Action onCompleted)
        {
            if (path != null)
            {
                if (MovingCoroutine != null)
                {
                    StopCoroutine(MovingCoroutine);
                }
                MovingCoroutine = StartCoroutine(Moving(path, onCompleted));
            }
        }

        IEnumerator Moving(List<TileEntity> path, Action onCompleted)
        {
            var nextIndex = 0;
            transform.position = Map.Settings.Projection(transform.position, Vector3.zero);
            while (nextIndex < path.Count)
            {
                var targetPoint = Map.WorldPosition(path[nextIndex]);
                var stepDir = (targetPoint - transform.position) * MoveSpeed;
                if (Map.RotationType == RotationType.LookAt)
                {
                    transform.rotation = Map.Settings.LookAt(stepDir);
                }
                else if(Map.RotationType == RotationType.Flip)
                {
                    transform.rotation = Map.Settings.Flip(stepDir);
                }
                var reached = stepDir.sqrMagnitude < 0.01f;
                while (!reached)
                {
                    transform.position += stepDir * Time.deltaTime;
                    reached = Vector3.Dot(stepDir, (targetPoint - transform.position)) < 0f;
                    yield return null;
                }
                transform.position = targetPoint;
                nextIndex++;
            }
            onCompleted.SafeInvoke();
        }

        public void Rotate(Vector3 lookPoint, Action onCompleted)
        {
            if (RotateCoroutine != null)
            {
                StopCoroutine(RotateCoroutine);
                RotateCoroutine = null;
            }
            if (gameObject.activeInHierarchy)
            {
                OnCompleted = onCompleted;
                LookPoint = lookPoint;
                RotateCoroutine = StartCoroutine(Rotating());
            }
        }

        IEnumerator Rotating()
        {
            var targetDir = LookPoint - transform.position;
            if (targetDir.sqrMagnitude > Eps)
            {
                if (Map.RotationType == RotationType.LookAt)
                {
                    var targetAngle = Map.Settings.SignedAngle(transform, targetDir);
                    var originRotEuler = transform.rotation.eulerAngles;
                    float rotation = RotateSpeed;
                    if (rotation < 0.01f)
                    {
                        rotation = 360f;
                        Log.E($"Invalid RotateSpeed {Unit}. Will use default 360");
                    }
                    var speed = Mathf.Sign(targetAngle) * rotation;
                    var time = targetAngle / speed;

                    while (time > 0f)
                    {
                        yield return null;
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Map.Settings.VectorCreateOrthogonal(speed * Time.deltaTime));
                        time -= Time.deltaTime;
                    }
                    transform.rotation = Quaternion.Euler(originRotEuler + Map.Settings.VectorCreateOrthogonal(targetAngle));
                }
                else if(Map.RotationType == RotationType.Flip)
                {
                    transform.rotation = Map.Settings.Flip(targetDir);
                }

            }
            RotateCoroutine = null;
            OnCompleted.SafeInvoke();
        }

        void UpdateUnit()
        {
            var tile = Map.Tile(transform.position);
            if (tile != null)
            {
                Unit.TilePosition = tile.Position;
            }
            Unit.Rotation = transform.rotation;
        }
    }
}
