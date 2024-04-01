using UnityEngine;

namespace RedBjorn.SuperTiles.Utils
{
    public class CameraLook : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
