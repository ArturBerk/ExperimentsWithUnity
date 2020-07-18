using System.Linq;
using Drift.Shapes;
using Things;
using UnityEngine;

namespace Drift
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    class PolylineShape : QuadShape
    {
        private IProperty<ComputeBuffer> positionsProperty;
        private IProperty<float> positionsLengthProperty;
        private IProperty<float> widthProperty;

        public float Width;
        public Vector2[] Points;
        private int bufferLength;

        protected override void PrepareProperties(MaterialProperties materialProperties)
        {
            base.PrepareProperties(materialProperties);
            positionsProperty = materialProperties.GetComputeBufferProperty("_Positions");
            positionsLengthProperty = materialProperties.GetFloatProperty("_PositionsLength");
            widthProperty = materialProperties.GetFloatProperty("_Width");
        }

        protected override unsafe (Vector2 Min, Vector2 Max) InvalidateWithBounds()
        {
            if (Points == null) return default;

            if (positionsProperty.Value == null || bufferLength < Points.Length)
            {
                bufferLength = Points.Length > 4 ? Points.Length : 4;
                positionsProperty.Value?.Dispose();
                positionsProperty.Value = new ComputeBuffer(bufferLength, sizeof(Vector3), ComputeBufferType.Structured);
            }
            widthProperty.Value = Width;
            var points = new Vector3[Points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Points[i];
            }
            positionsProperty.Value.SetData(points);
            positionsLengthProperty.Value = points.Length;
            positionsProperty.Invalidate();

            if (Points.Length > 0)
            {
                var min = Points[0];
                var max = min;
                for (int i = 1; i < Points.Length; i++)
                {
                    min = Min(min, Points[i]);
                    max = Max(max, Points[i]);
                }
                return (min, max);
            }

            return default;
        }

        private static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y), Mathf.Min(v1.z, v2.z));
        }

        private static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z));
        }

        private void OnDisable()
        {
            if (positionsProperty?.Value != null)
            {
                positionsProperty.Value.Dispose();
                positionsProperty.Value = null;
            }
        }
    }
}
