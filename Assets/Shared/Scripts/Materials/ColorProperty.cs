using UnityEngine;

namespace Things
{
    internal class ColorProperty : MaterialProperties.Property<Color>
    {

        internal ColorProperty(MaterialProperties properties, string name) : base(properties, name)
        {
        }

        public override void Apply(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetColor(PropertyId, Value);
        }
    }
}