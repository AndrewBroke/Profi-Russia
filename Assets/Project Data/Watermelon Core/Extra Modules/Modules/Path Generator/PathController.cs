using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Path Module v1.0.0

namespace Watermelon
{
    public class PathController : MonoBehaviour
    {
        public GeneratedPath path;

        public Color anchorCol = Color.red;
        public Color controlCol = Color.white;
        public Color segmentCol = Color.green;
        public Color selectedSegmentCol = Color.yellow;
        public float anchorDiameter = .1f;
        public float controlDiameter = .075f;
        public bool displayControlPoints = true;

        public void CreatePath()
        {
            if (path != null)
            {
                path.Initialize(Vector3.zero);
            }
        }

        void Reset()
        {
            CreatePath();
        }
    }
}