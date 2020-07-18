using UnityEngine;

namespace Things
{
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class MaterialPropertiesBehaviour : MonoBehaviour
    {
        public MaterialProperties Properties;

        protected virtual void Awake()
        {
            InitializePropertiesIfNeeded();
        }

        protected virtual void InitializePropertiesIfNeeded()
        {
            if (Properties != null) return;
            Properties = new MaterialProperties(GetComponent<MeshRenderer>());
            PrepareProperties(Properties);
            Properties.Apply();
        }

        protected virtual void OnDestroy()
        {
            Properties.Dispose();
        }

        protected abstract void PrepareProperties(MaterialProperties materialProperties);

        public bool ApplyProperties()
        {
            return Properties.Apply();
        }
    }
}