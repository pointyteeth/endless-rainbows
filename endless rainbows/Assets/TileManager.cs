using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

    public Sprite[] flatTiles;
    
    public Vector3 startPosition;
    public int numberOfTiles;
    private Vector3 nextPosition;
    private Queue<GameObject> tiles;
    
	// Use this for initialization
	void Start() {
        tiles = new Queue<GameObject>();
        nextPosition = startPosition;
        for(int i = 0; i < numberOfTiles; i++) {
            tiles.Enqueue(AddTile());
		}
	}
	
	// Update is called once per frame
	void Update() {
        if(!tiles.Peek().renderer.isVisible) {
			GameObject old = tiles.Dequeue();
			Object.Destroy(old);
            tiles.Enqueue(AddTile());
		}
	}
    
    // Add a tile to the foreground
    GameObject AddTile() {
        GameObject tile = new GameObject("tile");
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = flatTiles[0];
        BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
        tile.transform.position = nextPosition;
        nextPosition.x += tile.transform.localScale.x*2;
        Debug.Log(tile.transform.localScale.x);
        return tile;
    }
}
