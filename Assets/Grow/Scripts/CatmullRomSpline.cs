using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grow
{
    public class CatmullRomSpline
    {

        private List<Vector3> pts;
        //private float[] lengthRatio;
        //public float[] lengths;
        private int numSections;
        private int currPt;
        private float totalLength;

        public CatmullRomSpline(IEnumerable<Vector3> pts)
        {
            this.pts = new List<Vector3>(pts);
            numSections = this.pts.Count - 1;
        }

        public CatmullRomSpline(params Vector3[] pts)
        {
            this.pts = new List<Vector3>(pts);
            numSections = this.pts.Count - 1;
        }

        public void SetPoints(IEnumerable<Vector3> pts)
        {
            this.pts = new List<Vector3>(pts);
            numSections = this.pts.Count - 1;
        }

        public void AddPoint(Vector3 point)
        {
            pts.Add(point);
            numSections = pts.Count - 1;
        }

        public void AddPointAt(Vector3 point, int index)
        {
            pts.Insert(index, point);
            numSections = pts.Count - 1;
        }

        public void RemovePoint(Vector3 point)
        {
            pts.Remove(point);
            numSections = pts.Count - 1;
        }

        public void RemovePointAt(int index)
        {
            pts.RemoveAt(index);
            numSections = pts.Count - 1;
        }

        public Vector3 Evaluate(float ratio)
        {
            float t = ratio > 1f ? 1f : ratio;
            //Debug.Log("t:"+t+" ratio:"+ratio);
            //float t = ratio;
            return Interp(t);
        }

        private Vector3 Interp(float t)
        {
            if (numSections < 1)
            {
                return Vector3.zero;
            }
            // The adjustments done to numSections, I am not sure why I needed to add them
            /*int numSections = this.numSections+1;
            if(numSections>=3)
                numSections += 1;*/
            currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
            float u = t * (float)numSections - (float)currPt;
            currPt = currPt - 1;

            // Debug.Log("currPt:"+currPt+" numSections:"+numSections+" pts.Length :"+pts.Length );
            Vector3 a = currPt >= 0 ? pts[currPt] : pts[0];
            Vector3 b = pts[currPt + 1];
            Vector3 c = pts[currPt + 2];
            Vector3 d = currPt + 3 < pts.Count ? pts[currPt + 3] : pts[pts.Count - 1];

            return .5f * (
                (-a + 3f * b - 3f * c + d) * (u * u * u)
                + (2f * a - 5f * b + 4f * c - d) * (u * u)
                + (-a + c) * u
                + 2f * b
            );
        }

    }

    public static class SplineExtensions
    {
        public static Vector3[] ToPoints(this CatmullRomSpline bezierPath,
            float stepAngle = 2, float step = 0.01f, float maxSegmentLength = Single.PositiveInfinity)
        {
            var previous = bezierPath.Evaluate(0);
            var points = new List<Vector3>();
            points.Add(previous);
            var previousDirection = bezierPath.Evaluate(0.01f) - previous;
            Vector3 previousPoint = previous;
            for (float t = step; t < 1.0f; t += step)
            {
                var point = bezierPath.Evaluate(t);
                var currentDirection = point - previousPoint;
                if (Vector3.Angle(currentDirection, previousDirection) > stepAngle
                    || Vector3.Distance(point, previous) > maxSegmentLength)
                {
                    points.Add(point);
                    previous = point;
                    previousDirection = currentDirection;
                }

                previousPoint = point;
            }

            points.Add(bezierPath.Evaluate(1));
            return points.ToArray();
        }

        public static Vector3[] ToPoints(this CatmullRomSpline bezierPath, int pointCount)
        {
            if (pointCount < 2) pointCount = 2;
            var step = 1.0f / (pointCount - 1);
            var previous = bezierPath.Evaluate(0);
            var points = new List<Vector3>();
            points.Add(previous);
            for (float t = step; t < 1.0f; t += step)
            {
                var point = bezierPath.Evaluate(t);
                points.Add(point);
            }

            points.Add(bezierPath.Evaluate(1));
            return points.ToArray();
        }
    }
}
