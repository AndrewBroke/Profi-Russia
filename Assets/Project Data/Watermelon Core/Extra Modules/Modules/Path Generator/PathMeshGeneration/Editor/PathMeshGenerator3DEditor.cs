using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Path Module v1.0.0

namespace Watermelon
{
    [CustomEditor(typeof(PathMeshGenerator3D))]
    public class PathMeshGenerator3DEditor : Editor
    {
        PathMeshGenerator3D creator;

        void OnSceneGUI()
        {
            if (creator.autoUpdate && Event.current.type == EventType.Repaint)
            {
                creator.UpdateRoad();
            }
        }

        void OnEnable()
        {
            creator = (PathMeshGenerator3D)target;

            creator.CheckAllReferencesAssignment();
        }
    }
}