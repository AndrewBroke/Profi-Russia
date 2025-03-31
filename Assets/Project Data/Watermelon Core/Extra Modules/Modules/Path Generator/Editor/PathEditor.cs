using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Path Module v1.0.0

namespace Watermelon
{
    [CustomEditor(typeof(GeneratedPath))]
    public class PathEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update cached values"))
            {
                (target as GeneratedPath).UpdateCachedValues();
                EditorUtility.SetDirty(target);
            }
        }
    }
}