using UnityEngine;

namespace RedBjorn.SuperTiles
{
    public class CameraParallel : MonoBehaviour
    {
        void LateUpdate()
        {
            Align();
        }

        void Align()
        {
            transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Vector3.up);
        }
    }
}
