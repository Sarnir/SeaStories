using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaStories
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CustomPlane : MonoBehaviour
    {
        public int Width;
        public int Length;

        Mesh lastMesh;
        MeshFilter meshFilter;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();

            Create();
        }

        void Create()
        {
            var vertices = new Vector3[Width * Length];
            var uvs = new Vector2[Width * Length];
            var indices = new int[6 * (Width - 1) * (Length - 1)];

            int currentVertex = 0;
            int currentIndice = 0;

            for (int z = 0; z < Length; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    vertices[currentVertex] = new Vector3(x, 0f, z);
                    uvs[currentVertex] = new Vector2(x / (float)(Width - 1), z / (float)(Length - 1));

                    if (x + 1 < Width && z + 1 < Length)
                    {
                        indices[currentIndice] = currentVertex;
                        indices[currentIndice + 1] = currentVertex + Width;
                        indices[currentIndice + 2] = currentVertex + 1;
                        indices[currentIndice + 3] = currentVertex + 1;
                        indices[currentIndice + 4] = currentVertex + Width;
                        indices[currentIndice + 5] = currentVertex + Width + 1;

                        currentIndice += 6;
                    }

                    currentVertex++;
                }
            }

            lastMesh = new Mesh
            {
                vertices = vertices,
                triangles = indices,
                uv = uvs
            };

            lastMesh.RecalculateNormals();

            meshFilter.mesh = lastMesh;
        }
    }
}
