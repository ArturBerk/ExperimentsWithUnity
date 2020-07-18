using Drift.Shapes;
using UnityEngine;

namespace Assets.Shapes.Scripts
{
    [ExecuteAlways]
    class ShapeUpdater : MonoBehaviour
    {
        private IInvalidatable[] invalidatables;

        void OnEnable()
        {
            invalidatables = GetComponents<IInvalidatable>();
        }

        void LateUpdate()
        {
            if (invalidatables == null) return;
            foreach (var invalidatable in invalidatables)
            {
                invalidatable.Invalidate();
            }
        }
    }

    public interface IInvalidatable
    {
        void Invalidate();
    }
}
