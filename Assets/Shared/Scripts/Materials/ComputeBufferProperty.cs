using UnityEngine;

namespace Things
{
    internal class ComputeBufferProperty : MaterialProperties.Property<ComputeBuffer>
    {
        internal ComputeBufferProperty(MaterialProperties properties, string name) : base(properties, name)
        {
        }

        public override void Apply(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetBuffer(PropertyId, Value);
        }
    }
}