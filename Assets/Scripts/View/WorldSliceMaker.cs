/* 
 * Taken from my work-in-progress unity asset available at
 * https://github.com/rectdev/wheel/
 * 
 */

using UnityEngine;
namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public static class WorldSliceMaker
    {
        public static Mesh Generate(float separation, float arc, float radius = 1, float thickness = 0.1f, int arcResolution = 8)
        {
            arc = Mathf.PI * arc / 180;
            Mesh mesh = new Mesh();
            mesh.name = "GRPF_SliceMesh_C" + arcResolution + "_R" + radius.ToString("0.00") + "_T_" + thickness;

            int faceOffset = arcResolution + 1;
            Vector3[] vertices = new Vector3[4 * faceOffset + 8]; //8 for sides
            int[] triangles = new int[(arcResolution + 1) * 12];
            int sideOffset = 2 * (arcResolution - 1) * 3;

            vertices[0] =
            vertices[faceOffset * 2] =
            vertices[faceOffset * 4] =
            vertices[faceOffset * 4 + 1] = new Vector3(separation, 0, thickness);

            vertices[faceOffset] =
            vertices[faceOffset * 3] =
            vertices[faceOffset * 4 + 2] =
            vertices[faceOffset * 4 + 3] = new Vector3(separation, 0, -thickness);

            float arcStep = arc / (arcResolution - 1);

            for (int i = 0; i < arcResolution; i++)
            {
                float sliceAngle = 0;
                float startAngle = 0;
                sliceAngle += startAngle;
                sliceAngle -= arc / 2f;

                float x = Mathf.Cos(sliceAngle + arcStep * i) * radius + separation;
                float y = Mathf.Sin(sliceAngle + arcStep * i) * radius;


                vertices[i + 1] = new Vector3(x, y, thickness);
                vertices[faceOffset + i + 1] = new Vector3(x, y, -thickness);

                vertices[faceOffset * 2 + i + 1] = new Vector3(x, y, thickness);
                vertices[faceOffset * 3 + i + 1] = new Vector3(x, y, -thickness);

                if (i == 0)
                {
                    vertices[faceOffset * 4 + 4] = new Vector3(x, y, thickness);
                    vertices[faceOffset * 4 + 5] = new Vector3(x, y, -thickness);
                }
                else
                {
                    triangles[12 + sideOffset + i * 6] = faceOffset * 2 + i;
                    triangles[12 + sideOffset + i * 6 + 1] = faceOffset * 3 + i;
                    triangles[12 + sideOffset + i * 6 + 2] = faceOffset * 3 + i + 1;

                    triangles[12 + sideOffset + i * 6 + 3] = faceOffset * 2 + i;
                    triangles[12 + sideOffset + i * 6 + 4] = faceOffset * 3 + i + 1;
                    triangles[12 + sideOffset + i * 6 + 5] = faceOffset * 2 + i + 1;
                }


                if (i == arcResolution - 1)
                {
                    vertices[faceOffset * 4 + 6] = new Vector3(x, y, thickness);
                    vertices[faceOffset * 4 + 7] = new Vector3(x, y, -thickness);
                }
                else
                {
                    triangles[12 + i * 3] = 0;
                    triangles[12 + i * 3 + 1] = i + 1;
                    triangles[12 + i * 3 + 2] = i + 2;

                    triangles[12 + (arcResolution + i - 1) * 3] = faceOffset;
                    triangles[12 + (arcResolution + i - 1) * 3 + 1] = faceOffset + 2 + i;
                    triangles[12 + (arcResolution + i - 1) * 3 + 2] = faceOffset + 1 + i;
                }
            }

            triangles[0] = faceOffset * 4;
            triangles[1] = faceOffset * 4 + 2;
            triangles[2] = faceOffset * 4 + 5;

            triangles[3] = faceOffset * 4 + 0;
            triangles[4] = faceOffset * 4 + 5;
            triangles[5] = faceOffset * 4 + 4;

            triangles[6] = faceOffset * 4 + 1;
            triangles[7] = faceOffset * 4 + 7;
            triangles[8] = faceOffset * 4 + 3;

            triangles[9] = faceOffset * 4 + 1;
            triangles[10] = faceOffset * 4 + 6;
            triangles[11] = faceOffset * 4 + 7;

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        public static GameObject GenerateGameObject(float separation, float arc, Material mat, float radius = 1, float thickness = 0.1f, int arcResolution = 8)
        {
            return CreateGameObject(Generate(separation, arc, radius, thickness, arcResolution), mat);
        }

        private static GameObject CreateGameObject(Mesh mesh, Material mat)
        {
            GameObject slice = new GameObject();
            MeshRenderer meshRenderer = slice.AddComponent<MeshRenderer>();
            meshRenderer.material = mat;
            MeshFilter meshFilter = slice.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshCollider meshCollider = slice.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
            return slice;
        }
    }
}