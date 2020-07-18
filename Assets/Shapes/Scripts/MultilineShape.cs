using System;
using Drift.Shapes;
using Things;
using UnityEngine;

namespace Drift
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    class MultilineShape : QuadShape
    {
        private IProperty<ComputeBuffer> positionsProperty;
        private IProperty<float> positionsLengthProperty;
        private IProperty<float> widthProperty;

        public float Width;
        public Line[] Lines;
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
            if (Lines == null) return default;

            if (positionsProperty.Value == null || bufferLength < Lines.Length)
            {
                bufferLength = Lines.Length > 4 ? Lines.Length : 4;
                positionsProperty.Value?.Dispose();
                positionsProperty.Value = new ComputeBuffer(bufferLength, sizeof(Line), ComputeBufferType.Structured);
            }
            widthProperty.Value = Width;
            positionsProperty.Value.SetData(Lines);
            positionsLengthProperty.Value = Lines.Length * 2;
            positionsProperty.Invalidate();

            if (Lines.Length > 0)
            {
                var min = Lines[0].P0;
                var max = min;
                for (int i = 0; i < Lines.Length; i++)
                {
                    ref var line = ref Lines[i];
                    min = Min(min, line.P0);
                    max = Min(max, line.P0);
                    min = Min(min, line.P1);
                    max = Min(max, line.P1);
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

        [Serializable]
        public struct Line
        {
            public Vector3 P0;
            public Vector3 P1;
        }
    }
}
