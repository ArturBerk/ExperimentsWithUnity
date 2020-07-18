using Drift.Shapes;
using Things;
using UnityEngine;

namespace Drift
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    class CircleShape : QuadShape
    {
        private IProperty<float> widthProperty;
        private IProperty<float> radiusProperty;

        public float Radius;
        public float Width;

        protected override void PrepareProperties(MaterialProperties materialProperties)
        {
            base.PrepareProperties(materialProperties);
            widthProperty = materialProperties.GetFloatProperty("_Width");
            radiusProperty = materialProperties.GetFloatProperty("_Radius");
        }

        protected override (Vector2 Min, Vector2 Max) InvalidateWithBounds()
        {
            widthProperty.Value = Width;
            radiusProperty.Value = Radius;
            return (new Vector2(-Radius, -Radius), new Vector2(Radius, Radius));
        }
    }
}