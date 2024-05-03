using RedBjorn.ProtoTiles;
using System.Collections;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Controller of camera movement bahaviour
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        float Eps;
        [SerializeField]
        float MoveSpeed;

        Plane Plane;
        Camera Camera;
        MapEntity Map;
        Vector3? HoldPosition;
        Vector3? ClickPosition;
        Coroutine MovingToCoroutine;

        void LateUpdate()
        {
            if (InputController.GetOnWorldDown(Plane))
            {
                StopMoving();
                HoldPosition = InputController.GroundPositionOffset(Plane);
                ClickPosition = transform.position;
            }
            else if (InputController.GetOnWorldUp(Plane))
            {
                HoldPosition = null;
                ClickPosition = null;
            }
            UpdatePosition();
        }

        void OnDisable()
        {
            InputController.LockUp = false;
        }

        public void Init(MapEntity map)
        {
            Map = map;
            if (Map != null)
            {
                Plane = Map.Settings.Plane();
            }
            Camera = gameObject.GetComponent<Camera>();
        }

        public void MoveTo(Vector3 groundPosition)
        {
            StopMoving();
            if (gameObject.activeInHierarchy)
            {
                MovingToCoroutine = StartCoroutine(MovingTo(groundPosition));
            }
        }

        void UpdatePosition()
        {
            if (HoldPosition.HasValue)
            {
                var delta = HoldPosition.Value - (InputController.GroundPositionOffset(Plane));
                transform.position += delta;
                transform.position = ClickPosition.Value + delta;
                if (!InputController.LockUp)
                {
                    InputController.LockUp = delta.sqrMagnitude > Eps;
                }
            }
            else
            {
                InputController.LockUp = false;
            }
        }

        void StopMoving()
        {
            if (MovingToCoroutine != null)
            {
                StopCoroutine(MovingToCoroutine);
                MovingToCoroutine = null;
            }
        }

        IEnumerator MovingTo(Vector3 groundPosition)
        {
            if (Camera == null || Map == null)
            {
                yield break;
            }
            var ray = Camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            var map = Map.Settings;
            var delta = Vector3.zero;
            var distance = 0f;
            if (map.Plane().Raycast(ray, out distance))
            {
                delta = map.Projection(groundPosition) - map.Projection(ray.GetPoint(distance));
            }
            if (delta.sqrMagnitude > 0.1f)
            {
                var step = delta.normalized * MoveSpeed;
                var stepMain = map.AxisMainGet(step);
                var stepSecondary = map.AxisSecondaryGet(step);
                var targetPos = transform.position + delta;
                var direction = targetPos - transform.position;
                while (stepMain * map.AxisMainGet(direction) + stepSecondary * map.AxisSecondaryGet(direction) > 0f)
                {
                    transform.position = transform.position + step * Time.deltaTime;
                    direction = targetPos - transform.position;
                    yield return null;
                }
                transform.position = targetPos;
            }
            MovingToCoroutine = null;


        }
        public float speed = 7f;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += Vector3.forward * Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += Vector3.forward * -1 * Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.right * -1 * Time.deltaTime * speed;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right  * Time.deltaTime * speed;
            }

        }

    }

}
