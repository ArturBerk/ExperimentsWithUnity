using UnityEngine;

namespace Things
{
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class MaterialPropertiesBehaviour : MonoBehaviour
    {
        private MaterialProperties properties;

        protected virtual void Awake()
        {
            properties = new MaterialProperties(GetComponent<MeshRenderer>());
            PrepareProperties(properties);
            properties.Apply();
        }

        protected virtual void OnDestroy()
        {
            properties.Dispose();
        }

        protected abstract void PrepareProperties(MaterialProperties materialProperties);

        public bool ApplyProperties()
        {
            return properties.Apply();
        }
    }
}