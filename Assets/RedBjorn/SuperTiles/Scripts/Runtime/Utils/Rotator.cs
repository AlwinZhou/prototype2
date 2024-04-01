using UnityEngine;

namespace RedBjorn.SuperTiles.Utils
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 RotationStep;

        void Update()
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + RotationStep * Time.deltaTime);
        }
    }
}