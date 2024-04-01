using RedBjorn.ProtoTiles;
using System.Collections.Generic;
using UnityEngine;

namespace RedBjorn.SuperTiles.Utils
{
    /// <summary>
    /// Path drawer which use LineDrawer
    /// </summary>
    public class PathDrawer : MonoBehaviour
    {
        public LineDrawer Line;
        public Material MaterialActive;
        public Material MaterialInactive;

        GameObject Tail;
        MeshRenderer TailRenderer;
        MapEntity Map;

        const float TrajectoryOffset = 0.01f;

        public bool IsEnabled { get; set; }

        public void Init(MapEntity map)
        {
            Map = map;
            var offset = Map.Settings.VectorCreateOrthogonal(TrajectoryOffset);
            Line.Line.transform.localPosition = offset;
            Tail = Map.TileCreate(true, false, 0.2f, inner: MaterialInactive);
            Tail.transform.SetParent(transform);
            Tail.transform.localPosition = offset;
            TailRenderer = Tail.GetComponent<MeshRenderer>();
            Tail.SetActive(false);
        }

        public void ActiveState()
        {
            if (TailRenderer)
            {
                TailRenderer.material = MaterialActive;
            }
            Line.Line.material = MaterialActive;
        }

        public void InactiveState()
        {
            if (TailRenderer)
            {
                TailRenderer.material = MaterialInactive;
            }
            Line.Line.material = MaterialInactive;
        }

        public void Show(List<Vector3> points)
        {
            if (points == null || points.Count == 0)
            {
                Hide();
            }
            else
            {
                var tailPos = points[points.Count - 1];
                Tail.transform.localPosition = Map.Settings.Projection(tailPos, TrajectoryOffset);
                Tail.SetActive(true);
                if (points.Count > 1)
                {
                    Line.Line.transform.localRotation = Map.Settings.RotationPlane();
                    points[points.Count - 1] = (points[points.Count - 1] + points[points.Count - 2]) / 2f;
                    var pointsXY = new Vector3[points.Count]; 
                    for(int i = 0; i < pointsXY.Length; i++)
                    {
                        pointsXY[i] = Map.Settings.ProjectionXY(points[i]);
                    }

                    Line.Show(pointsXY);
                }
            }
        }

        public void Hide()
        {
            Line.Hide();
            if (Tail)
            {
                Tail.SetActive(false);
            }
        }
    }
}