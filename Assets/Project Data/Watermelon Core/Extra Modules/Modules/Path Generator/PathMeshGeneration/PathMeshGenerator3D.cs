using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Path Module v1.0.0
namespace Watermelon
{
    public class PathMeshGenerator3D : MonoBehaviour
    {
        // static settings
        private static float staticRoadWidth = 5f;

        // instance settings
        [Header("Settings")]
        public bool autoUpdate;
        public float tiling = 1;

        [Header("Reference")]
        public GeneratedPath path;

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

        public void UpdateRoad()
        {
            Vector3[] points = path.CalculateEvenlySpacedPoints();
            Mesh trackMesh = CreateRoadMesh(points, path.IsClosed);
            meshFilter.mesh = trackMesh;

            path.meshVertices = trackMesh.vertices;
            path.meshTriangles = trackMesh.triangles;
            path.meshUVs = trackMesh.uv;

            int textureRepeat = Mathf.RoundToInt(tiling * points.Length * path.cachedEvenlySpacedPointsSpacing * .05f);

            if (meshRenderer.sharedMaterial)
            {
                meshRenderer.sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
            }
            else
            {
                Debug.LogError("Please assign material to MeshGenerator's MeshRenderer component.");
            }
        }

        public static void UpdateMeshInfo(GeneratedPath path)
        {
            Mesh mesh = CreateRoadMesh(path.CalculateEvenlySpacedPoints(), path.IsClosed);

            path.meshVertices = mesh.vertices;
            path.meshTriangles = mesh.triangles;
            path.meshUVs = mesh.uv;
        }

        private static Mesh CreateRoadMesh(Vector3[] points, bool isClosed)
        {
            Vector3[] verts = new Vector3[points.Length * 2];
            Vector2[] uvs = new Vector2[verts.Length];
            int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
            int[] tris = new int[numTris * 3];
            int vertIndex = 0;
            int triIndex = 0;

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 forward = Vector3.zero;
                if (i < points.Length - 1 || isClosed)
                {
                    forward += points[(i + 1) % points.Length] - points[i];
                }
                if (i > 0 || isClosed)
                {
                    forward += points[i] - points[(i - 1 + points.Length) % points.Length];
                }

                forward.Normalize();

                Vector3 left = Vector3.Cross(forward, Vector3.up).normalized;

                verts[vertIndex] = points[i] + left * staticRoadWidth * .5f;
                verts[vertIndex + 1] = points[i] - left * staticRoadWidth * .5f;

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

        [ContextMenu("Generate evenly spaced points")]
        public void GenerateEvenlySpacedPoints()
        {
            Vector3[] points = path.cachedEvenlySpacedPoints;
            Transform parrent = new GameObject(path.name + "_EvenlySpacedPoints").transform;

            for (int i = 0; i < points.Length; i++)
            {
                Transform cubeTransform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;

                cubeTransform.position = points[i];
                cubeTransform.localScale = Vector3.zero.SetAll(0.1f);
                cubeTransform.SetParent(parrent);
            }
        }
    }
}