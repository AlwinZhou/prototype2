using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles.Utils
{
    public class RotatorAround : MonoBehaviour
    {
        public enum Type { Linear, Square, Cubic, RootSqrt }

        public Type RotateType;
        public float TargetAngle = 0f;
        public float Speed = 10f;
        Coroutine RotateCoroutine;

        Plane Plane;

        void Update()
        {
            if (InputController.GetGameHotkeyUp(S.Input.CameraClockwise))
            {
                RotateClockwise(InputController.CameraGroundPosition(Plane));
            }
            else if (InputController.GetGameHotkeyUp(S.Input.CameraCounterClockwise))
            {
                RotateCounterClockwise(InputController.CameraGroundPosition(Plane));
            }
        }

        void OnDisable()
        {
            RotateCoroutine = null;
        }

        public void Init(Plane plane)
        {
            Plane = plane;
        }

        void RotateClockwise(Vector3 point)
        {
            Rotate(point, -TargetAngle);
        }

        void RotateCounterClockwise(Vector3 point)
        {
            Rotate(point, TargetAngle);
        }

        void Rotate(Vector3 point, float angle)
        {
            if (RotateCoroutine == null)
            {
                RotateCoroutine = StartCoroutine(RotateProcess(point, transform.position - point, transform.rotation, angle, Speed));
            }
        }

        IEnumerator RotateProcess(Vector3 aroundPoint, Vector3 originPos, Quaternion originRot, float targetAngle, float targetSpeed)
        {
            var step = 0f;
            Quaternion quat;
            var ratio = 0f;
            while (ratio < 1f)
            {
                quat = Quaternion.Euler(0f, ratio * targetAngle, 0f);
                transform.position = quat * originPos + aroundPoint;
                transform.rotation = quat * originRot;
                yield return null;
                step += Time.deltaTime;
                switch (RotateType)
                {
                    case Type.Linear: ratio = step; break;
                    case Type.Square: ratio = step * step; break;
                    case Type.Cubic: ratio = step * step * step; break;
                    case Type.RootSqrt: ratio = Mathf.Sqrt(step); break;
                }
            }
            quat = Quaternion.Euler(0f, targetAngle, 0f);
            transform.position = quat * originPos + aroundPoint;
            transform.rotation = quat * originRot;
            RotateCoroutine = null;
        }
    }
}