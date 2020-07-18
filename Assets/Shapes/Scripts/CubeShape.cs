using UnityEngine;

namespace Drift.Shapes
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    abstract class CubeShape : BaseShape
    {
        public static Mesh CubeMesh
        {
            get
            {
                var mesh = new Mesh();
                mesh.vertices = new[]
                {
                        new Vector3(-0.5f, -0.5f, -0.5f),
                        new Vector3(-0.5f, 0.5f, -0.5f),
                        new Vector3(0.5f, 0.5f, -0.5f),
                        new Vector3(0.5f, -0.5f, -0.5f),
                        new Vector3(-0.5f, -0.5f, 0.5f),
                        new Vector3(-0.5f, 0.5f, 0.5f),
                        new Vector3(0.5f, 0.5f, 0.5f),
                        new Vector3(0.5f, -0.5f, 0.5f)
                    };
                mesh.triangles = new[]
                {
                        0, 1, 2, 0, 2, 3,
                        0, 5, 1, 0, 4, 5,
                        1,6,2,1,5,6,
                        2,7,3,2,6,7,
                        3,4,0,3,7,4,
                        4,6,5,4,7,6
                    };

                return mesh;
            }
        }

        private MeshFilter meshFilter;

        protected override void OnEnable()
        {
            InitializePropertiesIfNeeded();
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = CubeMesh;
            Invalidate();
        }

        public override void Invalidate()
        {
            var (min, max) = InvalidateWithBounds();
            var center = (max + min) * 0.5f;
            var size = max - min;
            meshFilter.sharedMesh.bounds = new Bounds(center, size);
            base.Invalidate();
        }

        protected abstract (Vector3 Min, Vector3 Max) InvalidateWithBounds();

    }
}