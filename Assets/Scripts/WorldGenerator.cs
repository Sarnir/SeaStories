using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(BoxCollider))]
public class WorldGenerator : MonoBehaviour
{
    public int Width;
    public int Length;
    public int Seed;

    public AnimationCurve HeightCurve;

    public bool useFalloffMap;
    public AnimationCurve falloffCurve;
    
    public SeaStories.TerrainType[] TerrainTypes;
    public int CitiesNum;

    [Header("Perlin Noise")]
    public float Scale = 0.5f;
    public int Octaves = 1;
    [Range(0f, 1f)]
    public float Persistence = 1;
    public float Lacunarity = 1;
    public Vector2 Offset;

    public bool AutoUpdateInEditor;
    
    Vector3[] vertices;
    Vector2[] uvs;
    int[] indices;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    BoxCollider meshCollider;

    List<SeaStories.TerrainTile> landTerrain = new List<SeaStories.TerrainTile>();
    List<SeaStories.TerrainTile> shoreTiles = new List<SeaStories.TerrainTile>();
    List<City> cities = new List<City>();

    void Start ()
    {
        Setup();
	}

    void Setup()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<BoxCollider>();

        landTerrain.Clear();
        shoreTiles.Clear();

        if (Application.isPlaying)
        {
            for(int i = 0; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
        }
        else
        {
            while (transform.childCount > 0)
            { 
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        CreateWorld();
    }

    void OnValidate()
    {
        if (Width < 1) Width = 1;
        if (Length < 1) Length = 1;
        if (Lacunarity < 1) Lacunarity = 1;
        if (Octaves < 1) Octaves = 1;
    }

    public void CreateWorldFromEditor()
    {
        Setup();
    }

    public void CreateWorld()
    {
        var falloffMap = GenerateFalloffMap();
        var heightMap = GenerateNoiseMap(falloffMap);
        var texture = GenerateMap(heightMap);
        GenerateMesh(heightMap);

        meshRenderer.sharedMaterial.mainTexture = texture;
        meshRenderer.sharedMaterial.mainTextureOffset = new Vector2(-1/(Width*2f), 1/(Length*2f));

        meshCollider.center = new Vector3((Width - 1) * 0.5f, 0f, (Length - 1) * 0.5f);
        meshCollider.size = new Vector3(Width - 1, 0f, Length - 1);
    }

    void GenerateMesh(float[] heightMap)
    {
        vertices = new Vector3[Width* Length];
        uvs = new Vector2[Width * Length];
        indices = new int[6*(Width-1)*(Length - 1)];

        int currentVertex = 0;
        int currentIndice = 0;

        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                vertices[currentVertex] = new Vector3(x, HeightCurve.Evaluate(heightMap[currentVertex]), z);
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

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    Texture2D GenerateMap(float[] heightMap)
    {
        Texture2D texture = new Texture2D(Width-1, Length-1);

        var colorMap = GenerateColorMap(heightMap);
        GenerateCitiesOnColorMap(CitiesNum, colorMap);

        texture.SetPixels(colorMap);
        texture.Apply();
        texture.filterMode = FilterMode.Point;

        return texture;
    }

    void GenerateCitiesOnColorMap(int number, Color[] colorMap)
    {
        for(int i = 0; i < number; i++)
        {
            var shoreTile = shoreTiles[Random.Range(0, shoreTiles.Count - 1)];
            colorMap[shoreTile.Index] = Color.red;

            // create city object
            cities.Add(City.Create("City " + (i + 1), gameObject.transform, shoreTile.Coord));
        }
    }

    Color[] GenerateColorMap(float[] heightMap)
    {
        Color[] colorMap = new Color[heightMap.Length];
        int currentPixel = 0;
        for (int i = 0; i < colorMap.Length; i++)
        {
            if (i % Width != 0 && i > Width)
            {
                float height = heightMap[i];
                colorMap[currentPixel] = GetColorFromHeight(height);
                int x = i % Width;
                var terrainType = GetTerrainTypeFromHeight(height);
                if (terrainType.IsLand())
                {
                    // check adjacent tiles for water
                    // Width * y + x
                    bool isShore = false;

                    if(!GetTerrainTypeFromHeight(heightMap[i - 1]).IsLand() ||
                        !GetTerrainTypeFromHeight(heightMap[i + 1]).IsLand() ||
                        !GetTerrainTypeFromHeight(heightMap[i - Width]).IsLand() ||
                        !GetTerrainTypeFromHeight(heightMap[i + Width]).IsLand())
                    {
                        isShore = true;
                        shoreTiles.Add(new SeaStories.TerrainTile(terrainType,
                            new Vector3(x, HeightCurve.Evaluate(height), (i - 1 - x) / Width), currentPixel, true));
                        //colorMap[currentPixel] = Color.red;
                    }

                    landTerrain.Add(new SeaStories.TerrainTile(terrainType,
                        new Vector3(x, height, (i - x) / Width), currentPixel, isShore));
                }
                currentPixel++;
            }
        }
        
        return colorMap;
    }

    float[] GenerateFalloffMap()
    {
        float falloffX;
        float falloffY;
        float[] falloffMap = new float[Width * Length];
        for (int y = 0; y < Length; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                falloffX = falloffCurve.Evaluate(x/(float)Width);
                falloffY = falloffCurve.Evaluate(y/(float)Length);
                falloffMap[Width * y + x] =  Mathf.Max(falloffX, falloffY);
            }
        }

        return falloffMap;
    }

    float[] GenerateNoiseMap(float[] falloffMap)
    {
        if (Scale <= 0f)
        {
            var errorMap = new float[Width * Length];
            return errorMap;
        }
        
        float[] noiseValues = new float[Width * Length];
        int currentIndex = 0;
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        Vector2[] offsets = new Vector2[Octaves];
        Random.InitState(Seed);
        for(int i = 0; i < Octaves; i++)
        {
            offsets[i] = new Vector2(Random.Range(-10000, 10000) + Offset.x,
                Random.Range(-10000, 10000) + Offset.y);
        }

        currentIndex = 0;

        for (int y = 0; y < Length; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float heightValue = 0;

                for (int i = 0; i < Octaves; i++)
                {
                    // get perlin noise value from -1 to 1
                    float sampleX = (x - Width*0.5f) / Scale * frequency + offsets[i].x;
                    float sampleY = (y - Length * 0.5f) / Scale * frequency + offsets[i].y;
                    var value = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    heightValue += value * amplitude;

                    amplitude *= Persistence;
                    frequency *= Lacunarity;

                    if (heightValue < minHeight)
                        minHeight = heightValue;
                    else if (heightValue > maxHeight)
                        maxHeight = heightValue;

                    noiseValues[currentIndex] = heightValue;
                }
                currentIndex++;
            }
        }

        // normalize it

        for (int i = 0; i < noiseValues.Length; i++)
        {
            noiseValues[i] = Mathf.InverseLerp(minHeight, maxHeight, noiseValues[i]);
            if(useFalloffMap)
                noiseValues[i] = Mathf.Clamp01(noiseValues[i] - falloffMap[i]);
        }

        return noiseValues;
    }

    Color GetColorFromHeight(float height)
    {
        for(int i = 0; i < TerrainTypes.Length; i++)
        {
            if (height <= TerrainTypes[i].MaxHeight)
                return TerrainTypes[i].Color;
        }

        return Color.black;
    }

    SeaStories.TerrainType GetTerrainTypeFromHeight(float height)
    {
        for (int i = 0; i < TerrainTypes.Length; i++)
        {
            if (height <= TerrainTypes[i].MaxHeight)
                return TerrainTypes[i];
        }

        return TerrainTypes[0];
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string Name;
        public float MaxHeight;
        public Color Color;
    }
}
