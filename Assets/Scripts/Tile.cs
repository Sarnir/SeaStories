using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    ShallowSea,
    Land
}

public class Tile : MonoBehaviour
{
    public TileType Type;
    Sprite sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetTileSize()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>().sprite;

        return sprite.bounds.size.x;
    }
}
