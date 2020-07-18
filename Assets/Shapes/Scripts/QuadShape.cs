using UnityEngine;

namespace Drift.Shapes
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    abstract class QuadShape : BaseShape
    {
        public static Mesh QuadMesh
        {
            get
            {
                    var mesh = new Mesh();
                    mesh.vertices = new[]
                    {
                        new Vector3(-0.5f, -0.5f, 0),
                        new Vector3(-0.5f, 0.5f, 0),
                        new Vector3(0.5f, 0.5f, 0),
                        new Vector3(0.5f, -0.5f, 0)
                    };
                    mesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };

                return mesh;
            }
        }

        private MeshFilter meshFilter;

        protected override void OnEnable()
        {
            InitializePropertiesIfNeeded();
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = QuadMesh;
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

        protected abstract (Vector2 Min, Vector2 Max) InvalidateWithBounds();

    }
}
