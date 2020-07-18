using System.Collections.Generic;
using UnityEngine;

namespace Things
{
    public class MaterialProperties
    {
        private readonly MaterialPropertyBlock propertyBlock;
        public readonly MeshRenderer Renderer;
        private bool isInvalidated;

        public MaterialProperties(MeshRenderer renderer)
        {
            propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            this.Renderer = renderer;
        }

        public void Dispose()
        {
            Renderer.SetPropertyBlock(null);
        }

        public bool Apply()
        {
            if (!isInvalidated) return false;
            Renderer.SetPropertyBlock(propertyBlock);
            isInvalidated = false;
            return true;
        }

        internal void Invalidate(IProperty property)
        {
            property.Apply(propertyBlock);
            isInvalidated = true;
        }

        internal interface IProperty
        {
            void Apply(MaterialPropertyBlock materialPropertyBlock);
        }

        internal abstract class Property<T> : IProperty<T>, IProperty
        {
            protected readonly int PropertyId;
            private T value;
            private readonly MaterialProperties properties;

            public T Value
            {
                get => value;
                set
                {
                    if (EqualityComparer<T>.Default.Equals(this.value, value)) return;
                    this.value = value;
                    Invalidate();
                }
            }

            public void Invalidate()
            {
                properties.Invalidate(this);
            }

            public abstract void Apply(MaterialPropertyBlock materialPropertyBlock);

            public Property(MaterialProperties properties, string name)
            {
                PropertyId = Shader.PropertyToID(name);
                this.properties = properties;
                Invalidate();
            }
        }
    }
}