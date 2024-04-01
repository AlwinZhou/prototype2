using RedBjorn.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.SuperTiles.Utils
{
    /// <summary>
    /// Utility which average normals of MeshInfo
    /// </summary>
    public class NormalsAverager : MonoBehaviour
    {
        public class MeshInfo
        {
            public List<Vector3> OriginNormals = new List<Vector3>();

            public Vector3 AverageNormal
            {
                get
                {
                    return OriginNormals.Aggregate((x, y) => x + y).normalized;
                }
            }
        }

        public MeshFilter Filter;

        void Awake()
        {
            if (!Filter)
            {
                Filter = GetComponent<MeshFilter>();
            }

            if (Filter)
            {
                DoAverageNormals();
            }
        }

        void DoAverageNormals()
        {
            var vertices = Filter.mesh.vertices;
            var norm = Filter.mesh.normals;
            var newNorm = new Vector4[norm.Length];
            var meshInfo = new Dictionary<Vector3, MeshInfo>();

            for (int i = 0; i < vertices.Length; i++)
            {
                meshInfo.TryGetOrCreate(vertices[i]).OriginNormals.Add(norm[i]);
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                var average = meshInfo.TryGetOrDefault(vertices[i]).AverageNormal;
                newNorm[i] = new Vector4(average.x, average.y, average.z, 0f);
            }
            Filter.mesh.tangents = newNorm;
        }
    }
}
