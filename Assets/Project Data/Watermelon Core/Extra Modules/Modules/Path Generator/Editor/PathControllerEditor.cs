using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Path Module v1.0.0

namespace Watermelon
{
    [CustomEditor(typeof(PathController))]
    public class PathControllerEditor : Editor
    {
        PathController controller;
        GeneratedPath Path
        {
            get
            {
                return controller.path;
            }
        }

        const float segmentSelectDistanceThreshold = .1f;
        int selectedSegmentIndex = -1;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Use mouse to drag controll points.\nShift + Left Click to add new point.\nRight Click to delete point.", MessageType.Info);

            base.OnInspectorGUI();

            if (controller.path == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Create new"))
            {
                Undo.RecordObject(controller, "Create new");
                controller.CreatePath();
            }

            bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");
            if (isClosed != Path.IsClosed)
            {
                Undo.RecordObject(controller, "Toggle closed");
                Path.IsClosed = isClosed;
            }

            bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");
            if (autoSetControlPoints != Path.AutoSetControlPoints)
            {
                Undo.RecordObject(controller, "Toggle auto set controls");
                Path.AutoSetControlPoints = autoSetControlPoints;
            }

            bool autoUpdateCachedValues = GUILayout.Toggle(Path.AutoUpdateCachedValues, "Auto Update Cached Values");
            if (autoUpdateCachedValues != Path.AutoUpdateCachedValues)
            {
                Undo.RecordObject(controller, "Toggle auto update cached values");
                Path.AutoUpdateCachedValues = autoUpdateCachedValues;
            }

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }


        void OnSceneGUI()
        {
            if (controller.path == null)
                return;

            Input();
            Draw();
        }

        void Input()
        {
            Event guiEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                if (selectedSegmentIndex != -1)
                {
                    Undo.RecordObject(controller, "Split segment");
                    Path.SplitSegment(mousePos, selectedSegmentIndex);
                }
                else if (!Path.IsClosed)
                {
                    Undo.RecordObject(controller, "Add segment");
                    Path.AddSegment(mousePos);
                }
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                float minDstToAnchor = controller.anchorDiameter * .5f;
                int closestAnchorIndex = -1;

                for (int i = 0; i < Path.NumPoints; i += 3)
                {
                    float dst = Vector2.Distance(mousePos, Path[i]);
                    if (dst < minDstToAnchor)
                    {
                        minDstToAnchor = dst;
                        closestAnchorIndex = i;
                    }
                }

                if (closestAnchorIndex != -1)
                {
                    Undo.RecordObject(controller, "Delete segment");
                    Path.DeleteSegment(closestAnchorIndex);
                }
            }

            if (guiEvent.type == EventType.MouseMove)
            {
                float minDstToSegment = segmentSelectDistanceThreshold;
                int newSelectedSegmentIndex = -1;

                for (int i = 0; i < Path.NumSegments; i++)
                {
                    Vector3[] points = Path.GetPointsInSegment(i);
                    float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                    if (dst < minDstToSegment)
                    {
                        minDstToSegment = dst;
                        newSelectedSegmentIndex = i;
                    }
                }

                if (newSelectedSegmentIndex != selectedSegmentIndex)
                {
                    selectedSegmentIndex = newSelectedSegmentIndex;
                    HandleUtility.Repaint();
                }
            }

            HandleUtility.AddDefaultControl(0);
        }

        void Draw()
        {
            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector3[] points = Path.GetPointsInSegment(i);
                if (controller.displayControlPoints)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(points[1], points[0]);
                    Handles.DrawLine(points[2], points[3]);
                }
                Color segmentCol = (i == selectedSegmentIndex && Event.current.shift) ? controller.selectedSegmentCol : controller.segmentCol;
                Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
            }


            for (int i = 0; i < Path.NumPoints; i++)
            {
                if (i % 3 == 0 || controller.displayControlPoints)
                {
                    Handles.color = (i % 3 == 0) ? controller.anchorCol : controller.controlCol;
                    float handleSize = (i % 3 == 0) ? controller.anchorDiameter : controller.controlDiameter;
                    var fmh_169_70_638791140636821350 = Quaternion.identity; Vector3 newPos = Handles.FreeMoveHandle(Path[i], handleSize, Vector2.zero, Handles.CylinderHandleCap);
                    if (Path[i] != newPos)
                    {
                        Undo.RecordObject(controller, "Move point");
                        Path.MovePoint(i, newPos.SetY(0f));
                    }
                }
            }
        }

        void OnEnable()
        {
            controller = (PathController)target;
        }

        private void OnDestroy()
        {
            if (controller.path != null)
            {
                if (Path.AutoUpdateCachedValues)
                {
                    Path.UpdateCachedValues();
                }
                EditorUtility.SetDirty(controller.path);
            }
        }
    }
}