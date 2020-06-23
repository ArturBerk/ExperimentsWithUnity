using UnityEngine;
using UnityEngine.Assertions;

namespace Pages
{
    [ExecuteAlways]
    public class Page : MonoBehaviour
    {
        private static readonly int progressProperty = Shader.PropertyToID("_Progress");
        private static readonly int directionProperty = Shader.PropertyToID("_Direction");

        [Range(0, 1)]
        public float Progress;

        public bool Direction;

        private MaterialPropertyBlock propertyBlock;
        private MeshRenderer meshRenderer;

        private void OnEnable()
        {
            propertyBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            Assert.IsNotNull(meshRenderer);
            meshRenderer.GetPropertyBlock(propertyBlock);
            OnValidate();
        }

        private void OnDisable()
        {
            propertyBlock = null;
            meshRenderer.SetPropertyBlock(null);
        }

        #if UNITY_EDITOR
        void OnValidate()
        {
            if (propertyBlock == null) return;
            propertyBlock.SetFloat(progressProperty, Progress);
            propertyBlock.SetFloat(directionProperty, Direction ? 1 : -1);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
        #endif

        public void UpdateProgress(float progress, bool direction)
        {
            Progress = progress;
            Direction = direction;

            if (propertyBlock == null) return;
            propertyBlock.SetFloat(progressProperty, Progress);
            propertyBlock.SetFloat(directionProperty, Direction ? 1 : -1);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
