using RedBjorn.ProtoTiles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Utils
{
    /// <summary>
    /// Colored wrapper of LineDrawer
    /// </summary>
    public class AreaOutline : MonoBehaviour
    {
        public LineDrawer Line;
        public Color ActiveColor;
        public Color InactiveColor;

        public void ActiveState()
        {
            SetColor(ActiveColor);
        }

        public void InactiveState()
        {
            SetColor(InactiveColor);
        }

        void SetColor(Color color)
        {
            Line.Line.material.color = color;
        }

        public void Show(List<Vector3> points, MapEntity map, float offset)
        {
            Line.Line.transform.localPosition = map.Settings.VectorCreateOrthogonal(offset);
            Line.Line.transform.localRotation = map.Settings.RotationPlane();

            var pointsXY = new Vector3[points.Count];
            for (int i = 0; i < pointsXY.Length; i++)
            {
                pointsXY[i] = map.Settings.ProjectionXY(points[i]);
            }
            Line.Show(pointsXY);
        }

        public void Hide()
        {
            Line.Hide();
        }
    }
}
