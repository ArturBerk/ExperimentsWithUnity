using UnityEngine;

namespace Things
{
    public static class Extensions
    {
        public static IProperty<float> GetFloatProperty(this MaterialProperties properties, string name)
        {
            return new FloatProperty(properties, name);
        }

        public static IProperty<Vector4> GetVectorProperty(this MaterialProperties properties, string name)
        {
            return new VectorProperty(properties, name);
        }

        public static IProperty<ComputeBuffer> GetComputeBufferProperty(this MaterialProperties properties, string name)
        {
            return new ComputeBufferProperty(properties, name);
        }
    }
}