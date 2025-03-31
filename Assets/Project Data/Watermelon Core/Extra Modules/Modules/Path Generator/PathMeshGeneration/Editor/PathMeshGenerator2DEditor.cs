using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Path Module v1.0.0

namespace Watermelon
{
    [CustomEditor(typeof(PathMeshGenerator2D))]
    public class PathMeshGenerator2DEditor : Editor
    {
        PathMeshGenerator2D creator;

        void OnSceneGUI()
        {
            if (creator.autoUpdate && Event.current.type == EventType.Repaint)
            {
                creator.UpdateRoad();
            }
        }

        void OnEnable()
        {
            creator = (PathMeshGenerator2D)target;

            creator.CheckAllReferencesAssignment();
        }
    }
}