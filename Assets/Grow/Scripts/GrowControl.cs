using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Grow
{
    [ExecuteAlways]
    public class GrowControl : MonoBehaviour
    {
        private static readonly int progressProperty = Shader.PropertyToID("_Progress");

        [Range(0, 1)]
        public float Progress;

        private MaterialPropertyBlock propertyBlock;
        private MeshRenderer meshRenderer;

        private ComputeBuffer positionsBuffer;

        public Transform Target;
        public Transform[] Points;

        private unsafe void OnEnable()
        {
            propertyBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            Assert.IsNotNull(meshRenderer);
            meshRenderer.GetPropertyBlock(propertyBlock);
            
            var vector3s = new Vector3[]
            {
                new Vector3(0,0,0), 
                new Vector3(-1,0,1f), 
                new Vector3(1,0,2f), 
                new Vector3(0,0,3f), 
                Target != null ? Target.localPosition : new Vector3(0,0,5), 
            };
            if (Points != null && Points.Length > 2)
            {
                vector3s = Points.Select(t => t.localPosition).ToArray();
            }
            var spline = new CatmullRomSpline(vector3s);
            var points = spline.ToPoints(100);
            positionsBuffer = new ComputeBuffer(points.Length, sizeof(Vector3), ComputeBufferType.Structured);
            positionsBuffer.SetData(points);
            propertyBlock.SetBuffer("_Positions", positionsBuffer);
            propertyBlock.SetFloat("_PositionsLength", positionsBuffer.count);
            meshRenderer.SetPropertyBlock(propertyBlock);

            OnValidate();
        }

        private void OnDisable()
        {
            propertyBlock = null;
            meshRenderer.SetPropertyBlock(null);

            positionsBuffer.Dispose();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (propertyBlock == null) return;
            propertyBlock.SetFloat(progressProperty, Progress);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
#endif

        private void Update()
        {
            if (!Application.isPlaying) return;
            UpdateProgress(Progress);
        }

        public void UpdateProgress(float progress)
        {
            Progress = progress;

            if (propertyBlock == null) return;
            propertyBlock.SetFloat(progressProperty, Progress);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
