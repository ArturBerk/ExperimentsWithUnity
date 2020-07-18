using UnityEngine;

namespace Things
{
    internal class FloatProperty : IProperty<float>, MaterialProperties.IProperty
    {
        protected readonly int PropertyId;
        private float value;
        private readonly MaterialProperties properties;

        public float Value
        {
            get => value;
            set
            {
                if (Mathf.Abs(this.value - value) < 0.0000001f) return;
                this.value = value;
                Invalidate();
            }
        }

        public void Invalidate()
        {
            properties.Invalidate(this);
        }

        public void Apply(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetFloat(PropertyId, value);
        }

        public FloatProperty(MaterialProperties properties, string name)
        {
            PropertyId = Shader.PropertyToID(name);
            this.properties = properties;
            Invalidate();
        }
    }
}