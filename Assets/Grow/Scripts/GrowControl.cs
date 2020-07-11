using System.Linq;
using Things;
using UnityEngine;
using UnityEngine.Assertions;

namespace Grow
{
    [ExecuteAlways]
    public class GrowControl : MaterialPropertiesBehaviour
    {
        [Range(0, 1)]
        public float Progress;

        private IProperty<ComputeBuffer> positionsProperty;
        private IProperty<float> positionsLengthProperty;
        private IProperty<float> progressProperty;

        public Transform Target;
        public Transform[] Points;
        
        protected override void PrepareProperties(MaterialProperties materialProperties)
        { 
            positionsProperty = materialProperties.GetComputeBufferProperty("_Positions");
            positionsLengthProperty = materialProperties.GetFloatProperty("_PositionsLength");
            progressProperty = materialProperties.GetFloatProperty("_Progress");
        }

        private unsafe void OnEnable()
        {
            // Editor mode
            if (positionsProperty == null)
            {
                Awake();
            }
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
            var positionsBuffer = new ComputeBuffer(points.Length, sizeof(Vector3), ComputeBufferType.Structured);
            positionsBuffer.SetData(points);
            positionsProperty.Value = positionsBuffer;
            positionsLengthProperty.Value = positionsBuffer.count;

            ApplyProperties();

            OnValidate();
        }

        private void OnDisable()
        {
            positionsProperty.Value.Dispose();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UpdateProgress(Progress);
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
            if (positionsProperty == null) return;
            progressProperty.Value = Progress;
            ApplyProperties();
        }
    }
}
