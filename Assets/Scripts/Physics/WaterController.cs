using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterController : MonoBehaviour
{
    MeshFilter meshFilter;
    public int Width;
    public int Length;

    public float waveSpeed;
    public float waveScale;
    public float waveHeight;

    public static WaterController Instance;

    Mesh lastMesh;

    void Start ()
    {
        Instance = this;
        meshFilter = GetComponent<MeshFilter>();

        // create mesh for water
        CreateMesh();
	}

    void Update()
    {
        UpdateWater();
    }

    void UpdateWater()
    {
        var verts = lastMesh.vertices;
        int currentVertex = 0;

        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                float y = GetWaterYPos(new Vector3(x, 0f, z));
                verts[currentVertex] = new Vector3(x, y, z);
                currentVertex++;
            }
        }

        lastMesh.vertices = verts;
        meshFilter.mesh = lastMesh;
    }

    public float GetWaterYPos(Vector3 pos)
    {
        return waveHeight*Mathf.Sin(waveSpeed * Time.frameCount + waveScale * pos.x);
    }

    void CreateMesh()
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

        lastMesh = new Mesh();
        lastMesh.vertices = vertices;
        lastMesh.triangles = indices;
        lastMesh.uv = uvs;
        lastMesh.RecalculateNormals();

        meshFilter.mesh = lastMesh;
    }
}
