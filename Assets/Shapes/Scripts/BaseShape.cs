using Assets.Shapes.Scripts;
using Things;
using UnityEngine;

namespace Drift.Shapes
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    abstract class BaseShape : MaterialPropertiesBehaviour, IInvalidatable
    {
        private IProperty<float> depthProperty;
        private IProperty<float> smoothProperty;
        private IProperty<Color> colorProperty;

        public float Depth = 0;
        public float Smooth = 0;
        [ColorUsage(true, true)]
        public Color Color = Color.white;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (Properties == null) return;
            Invalidate();
        }
#endif

        protected virtual void OnEnable()
        {
            InitializePropertiesIfNeeded();
            Invalidate();
        }

        protected override void PrepareProperties(MaterialProperties materialProperties)
        {
            depthProperty = materialProperties.GetFloatProperty("_Depth");
            smoothProperty = materialProperties.GetFloatProperty("_Smooth");
            colorProperty = materialProperties.GetColorProperty("_Color");
        }

        public virtual void Invalidate()
        {
            depthProperty.Value = Depth;
            smoothProperty.Value = Smooth;
            colorProperty.Value = Color;
            ApplyProperties();
        }

    }
}