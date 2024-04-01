using RedBjorn.SuperTiles.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Controller which wraps UnityEnity.Input class
    /// </summary>
    public class InputController
    {
        public class FrameInfo
        {
            public int Frame;
            public bool OverUI;
            public GameObject OverObject;
            public Vector3 CameraGroundPosition;
        }

        public static bool LockUp;

        static FrameInfo LastFrame = new FrameInfo();

        static void Validate(Plane plane)
        {
            if (LastFrame.Frame != Time.frameCount)
            {
                LastFrame.Frame = Time.frameCount;
                LastFrame.OverUI = EventSystem.current.IsPointerOverGameObject();
                if (LastFrame.OverUI)
                {
                    LastFrame.OverObject = null;
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
                    {
                        LastFrame.OverObject = hit.collider.gameObject;
                    }
                    else
                    {
                        LastFrame.OverObject = null;
                    }
                }
                var screemCenterRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                float enter = 0f;
                if (plane.Raycast(screemCenterRay, out enter))
                {
                    LastFrame.CameraGroundPosition = screemCenterRay.GetPoint(enter);
                }
                else
                {
                    LastFrame.CameraGroundPosition = Vector3.zero;
                }
            }
        }

        public static GameObject OverGameobject(Plane plane)
        {
            Validate(plane);
            return LastFrame.OverObject;
        }

        public static bool GetOnWorldDown(Plane plane, bool lockCheck = false)
        {
            Validate(plane);
            var key = S.Input.WorldClick;
            return !LastFrame.OverUI && (!lockCheck || !LockUp) && (Input.GetKeyDown(key.Main) || Input.GetKeyDown(key.Alternative));
        }

        public static bool GetOnWorldUp(Plane plane, bool lockCheck = false)
        {
            Validate(plane);
            var key = S.Input.WorldClick;
            return !LastFrame.OverUI && (!lockCheck || !LockUp) && (Input.GetKeyUp(key.Main) || Input.GetKeyUp(key.Alternative));
        }

        public static bool GetGameHotkeyUp(Settings.InputSettings.Key key, bool ignoreConfirmMessage = false)
        {
            return (ignoreConfirmMessage || !ConfirmMessageUI.IsActive) && (Input.GetKeyUp(key.Main) || Input.GetKeyUp(key.Alternative));
        }

        public static Vector3 CameraGroundPosition(Plane plane)
        {
            Validate(plane);
            return LastFrame.CameraGroundPosition;
        }

        public static Vector3 GroundPosition(Plane plane)
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0f;
            if (plane.Raycast(mouseRay, out enter))
            {
                return mouseRay.GetPoint(enter);
            }
            return Vector3.zero;
        }

        public static Vector3 GroundPositionOffset(Plane plane)
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0f;
            if (plane.Raycast(mouseRay, out enter))
            {
                return mouseRay.GetPoint(enter) - Camera.main.transform.position;
            }
            return Vector3.zero;
        }
    }
}