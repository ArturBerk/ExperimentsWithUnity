using System;
using System.Collections.Generic;
using Assets.Shapes.Scripts;
using Rooms.Assets.Scripts.Common;
using UnityEngine;

namespace Drift.Shapes
{
    [ExecuteAlways]
    class Spline3D : MonoBehaviour, IInvalidatable
    {
        public Vector3[] Points;
        public float Step = 0.1f;

        private List<Vector3> pointList = new List<Vector3>();
        private IPointShape3D shape;

        #if UNITY_EDITOR
        void OnValidate()
        {
            Invalidate();
        }
        #endif

        void OnEnable()
        {
            shape = GetComponent<IPointShape3D>();
            if (shape == null) throw new Exception("Shape not found");
        }

        public void Invalidate()
        {
            if (shape == null || Points == null || Points.Length < 2) return;
            CatmullRomSpline spline = new CatmullRomSpline(Points);
            var step = 1.0f / 1000.0f;
            var previous = new Vector3(Single.PositiveInfinity, Single.PositiveInfinity, Single.PositiveInfinity);
            for (int i = 0; i <= 1000; i++)
            {
                var position = spline.Evaluate(i * step);
                var d = Vector3.Distance(position, previous);
                if (d > Step)
                {
                    pointList.Add(position);
                    previous = position;
                }
            }

            if (pointList.Count < 1) return;
            shape.SetPoints(pointList.ToArray());
            pointList.Clear();
        }
    }
}
