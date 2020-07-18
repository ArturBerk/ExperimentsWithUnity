using Drift.Shapes;
using Things;
using UnityEngine;

namespace Drift
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    class LineShape : QuadShape
    {
        private IProperty<float> widthProperty;

        public float Width;
        public Vector2 From;
        public Vector2 To;

        protected override void PrepareProperties(MaterialProperties materialProperties)
        {
            base.PrepareProperties(materialProperties);
            widthProperty = materialProperties.GetFloatProperty("_Width");
        }

        protected override (Vector2 Min, Vector2 Max) InvalidateWithBounds()
        {
            widthProperty.Value = Width;
            return (From, To);
        }
    }
}