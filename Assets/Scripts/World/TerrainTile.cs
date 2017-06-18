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
        TerrainID id;
        [SerializeField]
        Color color;
        [SerializeField]
        float maxHeight;

        public TerrainID ID { get { return id; } }
        public Color Color { get { return color; } }
        public float MaxHeight { get { return maxHeight; } }

        public bool IsLand()
        {
            return ID >= TerrainID.Sand;
        }
    }

    public class TerrainTile
    {
        public bool IsLand { get { return _terrainType.IsLand(); } }
        public bool IsShore { get { return _isShore; } }
        public Vector3 Coord { get { return _coord; } }
        public int Index { get { return _index; } }

        TerrainType _terrainType;
        Vector3 _coord;
        bool _isShore;
        int _index;

        public TerrainTile(TerrainType type, Vector3 coord, int index, bool isShore)
        {
            _terrainType = type;
            _coord = coord;
            _isShore = isShore;
            _index = index;
        }
    }
}