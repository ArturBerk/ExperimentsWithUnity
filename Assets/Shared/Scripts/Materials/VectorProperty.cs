using UnityEngine;

namespace Things
{
    internal class VectorProperty : MaterialProperties.Property<Vector4>
    {

        internal VectorProperty(MaterialProperties properties, string name) : base(properties, name)
        {
        }

        public override void Apply(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetVector(PropertyId, Value);
        }
    }

    internal class MatrixProperty : MaterialProperties.Property<Matrix4x4>
    {

        internal MatrixProperty(MaterialProperties properties, string name) : base(properties, name)
        {
        }

        public override void Apply(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetMatrix(PropertyId, Value);
        }
    }
}