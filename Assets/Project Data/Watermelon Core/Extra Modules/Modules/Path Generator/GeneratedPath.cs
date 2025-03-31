using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Path Module v1.0.0

namespace Watermelon
{
    [CreateAssetMenu(fileName = "NewPath", menuName = "Content/PathModule/Path")]
    public class GeneratedPath : ScriptableObject
    {
        public List<Vector3> points = new List<Vector3>();
        public Vector3[] meshVertices;
        public int[] meshTriangles;
        public Vector2[] meshUVs;

        [Header("Cached values")]
        [SerializeField] bool autoUpdateCachedValues;
        [Space(3f)]
        public int cachedEvenlySpacedPointsSpacing = 1;
        public Vector3[] cachedEvenlySpacedPoints;

        private bool isClosed;
        private bool autoSetControlPoints;

        public void Initialize(Vector3 centre)
        {
            points = new List<Vector3>
            {
                centre + Vector3.zero,
                centre + (Vector3.right * 2 + Vector3.up) * 0.5f,
                centre + (Vector3.right * 2 + Vector3.down) * 0.5f,
                centre + Vector3.right * 2
            };
        }

        public Vector3 this[int i]
        {
            get
            {
                return points[i];
            }
        }

        public bool IsClosed
        {
            get
            {
                return isClosed;
            }
            set
            {
                if (isClosed != value)
                {
                    isClosed = value;

                    if (isClosed)
                    {
                        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                        points.Add(points[0] * 2 - points[1]);
                        if (autoSetControlPoints)
                        {
                            AutoSetAnchorControlPoints(0);
                            AutoSetAnchorControlPoints(points.Count - 3);
                        }
                    }
                    else
                    {
                        points.RemoveRange(points.Count - 2, 2);
                        if (autoSetControlPoints)
                        {
                            AutoSetStartAndEndControls();
                        }
                    }
                }
            }
        }

        public bool AutoSetControlPoints
        {
            get
            {
                return autoSetControlPoints;
            }
            set
            {
                if (autoSetControlPoints != value)
                {
                    autoSetControlPoints = value;
                    if (autoSetControlPoints)
                    {
                        AutoSetAllControlPoints();
                    }
                }
            }
        }

        public bool AutoUpdateCachedValues
        {
            get
            {
                return autoUpdateCachedValues;
            }
            set
            {
                if (autoUpdateCachedValues != value)
                {
                    autoUpdateCachedValues = value;
                    if (autoUpdateCachedValues)
                    {
                        UpdateCachedValues();
                    }
                }
            }
        }

        public int NumPoints
        {
            get
            {
                return points.Count;
            }
        }

        public int NumSegments
        {
            get
            {
                return points.Count / 3;
            }
        }

        public void AddSegment(Vector3 anchorPos)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add((points[points.Count - 1] + anchorPos.SetY(0f)) * .5f);
            points.Add(anchorPos.SetY(0f));

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(points.Count - 1);
            }
        }

        public void SplitSegment(Vector3 anchorPos, int segmentIndex)
        {
            points.InsertRange(segmentIndex * 3 + 2, new Vector3[] { Vector3.zero, anchorPos, Vector3.zero });
            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
            }
            else
            {
                AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
            }
        }

        public void DeleteSegment(int anchorIndex)
        {
            if (NumSegments > 2 || !isClosed && NumSegments > 1)
            {
                if (anchorIndex == 0)
                {
                    if (isClosed)
                    {
                        points[points.Count - 1] = points[2];
                    }
                    points.RemoveRange(0, 3);
                }
                else if (anchorIndex == points.Count - 1 && !isClosed)
                {
                    points.RemoveRange(anchorIndex - 2, 3);
                }
                else
                {
                    points.RemoveRange(anchorIndex - 1, 3);
                }
            }
        }

        public Vector3[] GetPointsInSegment(int i)
        {
            return new Vector3[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };
        }

        public void MovePoint(int i, Vector3 pos)
        {
            Vector3 deltaMove = pos - points[i];

            if (i % 3 == 0 || !autoSetControlPoints)
            {
                points[i] = pos;

                if (autoSetControlPoints)
                {
                    AutoSetAllAffectedControlPoints(i);
                }
                else
                {

                    if (i % 3 == 0)
                    {
                        if (i + 1 < points.Count || isClosed)
                        {
                            points[LoopIndex(i + 1)] += deltaMove;
                        }
                        if (i - 1 >= 0 || isClosed)
                        {
                            points[LoopIndex(i - 1)] += deltaMove;
                        }
                    }
                    else
                    {
                        bool nextPointIsAnchor = (i + 1) % 3 == 0;
                        int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                        int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                        if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                        {
                            float dst = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                            Vector3 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                            points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dst;
                        }
                    }
                }
            }

            cachedEvenlySpacedPoints = CalculateEvenlySpacedPoints();
        }

        public Vector3[] CalculateEvenlySpacedPoints(float spacing = -1f, float resolution = 1)
        {
            if (spacing == -1)
            {
                spacing = cachedEvenlySpacedPointsSpacing;
            }

            List<Vector3> evenlySpacedPoints = new List<Vector3>();
            evenlySpacedPoints.Add(points[0]);
            Vector3 previousPoint = points[0];
            float dstSinceLastEvenPoint = 0;

            for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
            {
                Vector3[] p = GetPointsInSegment(segmentIndex);
                float controlNetLength = Vector3.Distance(p[0], p[1]) + Vector3.Distance(p[1], p[2]) + Vector3.Distance(p[2], p[3]);
                float estimatedCurveLength = Vector3.Distance(p[0], p[3]) + controlNetLength / 2f;
                int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
                float t = 0;
                while (t <= 1)
                {
                    t += 1f / divisions;
                    Vector3 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                    dstSinceLastEvenPoint += Vector3.Distance(previousPoint, pointOnCurve);

                    while (dstSinceLastEvenPoint >= spacing)
                    {
                        float overshootDst = dstSinceLastEvenPoint - spacing;
                        Vector3 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                        evenlySpacedPoints.Add(newEvenlySpacedPoint);
                        dstSinceLastEvenPoint = overshootDst;
                        previousPoint = newEvenlySpacedPoint;
                    }

                    previousPoint = pointOnCurve;
                }
            }

            evenlySpacedPoints.Add(points[points.Count - 1]);

            return evenlySpacedPoints.ToArray();
        }

        public void UpdateCachedValues()
        {
            Debug.Log("Updating cached values at: " + name);
            cachedEvenlySpacedPoints = CalculateEvenlySpacedPoints();

            PathMeshGenerator3D.UpdateMeshInfo(this);
        }

        private void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
        {
            for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
            {
                if (i >= 0 && i < points.Count || isClosed)
                {
                    AutoSetAnchorControlPoints(LoopIndex(i));
                }
            }

            AutoSetStartAndEndControls();
        }

        private void AutoSetAllControlPoints()
        {
            for (int i = 0; i < points.Count; i += 3)
            {
                AutoSetAnchorControlPoints(i);
            }

            AutoSetStartAndEndControls();
        }

        private void AutoSetAnchorControlPoints(int anchorIndex)
        {
            Vector3 anchorPos = points[anchorIndex];
            Vector3 dir = Vector3.zero;
            float[] neighbourDistances = new float[2];

            if (anchorIndex - 3 >= 0 || isClosed)
            {
                Vector3 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
                dir += offset.normalized;
                neighbourDistances[0] = offset.magnitude;
            }
            if (anchorIndex + 3 >= 0 || isClosed)
            {
                Vector3 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
                dir -= offset.normalized;
                neighbourDistances[1] = -offset.magnitude;
            }

            dir.Normalize();

            for (int i = 0; i < 2; i++)
            {
                int controlIndex = anchorIndex + i * 2 - 1;
                if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
                {
                    points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * .5f;
                }
            }
        }

        private void AutoSetStartAndEndControls()
        {
            if (!isClosed)
            {
                points[1] = (points[0] + points[2]) * .5f;
                points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * .5f;
            }
        }

        private int LoopIndex(int i)
        {
            return (i + points.Count) % points.Count;
        }
    }
}
