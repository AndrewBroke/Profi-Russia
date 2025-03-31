using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Path Module v1.0.0

namespace Watermelon
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class PathMeshGenerator2D : MonoBehaviour
    {
        public GeneratedPath path;

        [Space(3f)]
        [Min(0.1f)]
        public float roadWidth = 1;
        [Min(0.1f)]
        public float triangleLength = 1f;
        [Space(3f)]
        public float textureSpacing = 1;
        public float textureTiling = 1;

        [Space(5)]
        public bool autoUpdate;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            CheckAllReferencesAssignment();
        }

        public void CheckAllReferencesAssignment()
        {
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        public void GeneratePathMesh(GeneratedPath path)
        {
            this.path = path;

            UpdateRoad();
        }

        public void UpdateRoad()
        {
            Vector3[] points = path.CalculateEvenlySpacedPoints(triangleLength);
            meshFilter.mesh = CreateRoadMesh(points, path.IsClosed);

            int textureRepeat = Mathf.RoundToInt(textureTiling * points.Length * textureSpacing * 0.05f);

            if (meshRenderer.sharedMaterial != null)
            {
                meshRenderer.sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
            }
            else
            {
                Debug.LogError("Please assign material to MeshGenerator's MeshRenderer component.");
            }

        }


        private Mesh CreateRoadMesh(Vector3[] points, bool isClosed)
        {
            Vector3[] verts = new Vector3[points.Length * 2];
            Vector2[] uvs = new Vector2[verts.Length];
            int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
            int[] tris = new int[numTris * 3];
            int vertIndex = 0;
            int triIndex = 0;

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 forward = Vector2.zero;
                if (i < points.Length - 1 || isClosed)
                {
                    forward += (Vector2)(points[(i + 1) % points.Length] - points[i]);
                }
                if (i > 0 || isClosed)
                {
                    forward += (Vector2)(points[i] - points[(i - 1 + points.Length) % points.Length]);
                }

                forward.Normalize();
                Vector2 left = new Vector2(-forward.y, forward.x);

                verts[vertIndex] = (Vector2)points[i] + left * roadWidth * .5f;
                verts[vertIndex + 1] = (Vector2)points[i] - left * roadWidth * .5f;

                float completionPercent = i / (float)(points.Length - 1);
                float v = 1 - Mathf.Abs(2 * completionPercent - 1);
                uvs[vertIndex] = new Vector2(0, v);
                uvs[vertIndex + 1] = new Vector2(1, v);

                if (i < points.Length - 1 || isClosed)
                {
                    tris[triIndex] = vertIndex;
                    tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                    tris[triIndex + 2] = vertIndex + 1;

                    tris[triIndex + 3] = vertIndex + 1;
                    tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                    tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
                }

                vertIndex += 2;
                triIndex += 6;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;

            return mesh;
        }
    }
}