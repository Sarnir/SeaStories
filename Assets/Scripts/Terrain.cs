using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeaStories
{
    public enum TerrainID
    {
        DeepSea,
        ShallowSea,
        Sand,
        Grassland,
        Forest,
        Mountains
    }

    [System.Serializable]
    public class TerrainType
    {
        [SerializeField]
        TerrainID ID;
        [SerializeField]
        Color Color;

        public TerrainType(TerrainID id, Color color)
        {
            ID = id;
            Color = color;
        }

        public bool IsLand()
        {
            return ID >= TerrainID.Sand;
        }

        public TerrainID GetID()
        {
            return ID;
        }

        public Color GetColor()
        {
            return Color;
        }
    }

    public class Terrain
    {
        public bool IsLand { get { return _terrainType.IsLand(); } }
        public bool IsShore { get { return _isShore; } }
        public Vector3 Coord { get { return _coord; } }

        TerrainType _terrainType;
        Vector3 _coord;
        bool _isShore;

        public Terrain(TerrainType type, Vector3 coord, bool isShore)
        {
            _terrainType = type;
            _coord = coord;
            _isShore = isShore;
        }
    }
}