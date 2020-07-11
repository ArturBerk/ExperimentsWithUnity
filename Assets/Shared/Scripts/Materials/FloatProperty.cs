using UnityEngine;

namespace Things
{
    internal class FloatProperty : MaterialProperties.Property<float>
    {

        internal FloatProperty(MaterialProperties properties, string name) : base(properties, name)
        {
        }

        public override void Apply(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetFloat(PropertyId, Value);
        }
    }
}