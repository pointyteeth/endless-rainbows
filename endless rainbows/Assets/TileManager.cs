using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {


    public Sprite[] fill;
    public Sprite[] flat;
    public Sprite[] diagonalDown;
    public Sprite[] diagonalDownCorner;
    
    private Dictionary<string, SortedDictionary<double, Sprite[]>> afterDistributions;
    
    public Vector3 startPosition;
    public int numberOfTiles;
    private Vector3 nextPosition;
    private Queue<GameObject> tiles;
    
	// Use this for initialization
	void Start() {
        afterDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
            {"flat", new SortedDictionary<double, Sprite[]> {
                {0.8, flat},
                {0.2, diagonalDown}
            }},
            {"diagonal_down", new SortedDictionary<double, Sprite[]> {
                {0.8, flat},
                {0.2, diagonalDown}
            }}
        };
    
        tiles = new Queue<GameObject>();
        nextPosition = startPosition;
        for(int i = 0; i < numberOfTiles; i++) {
            tiles.Enqueue(InstantiateTile(flat[0]));
		}
	}
	
	// Update is called once per frame
	void Update() {
        if(!tiles.Peek().renderer.isVisible) {
			GameObject old = tiles.Dequeue();
			Object.Destroy(old);
            tiles.Enqueue(InstantiateTile(GetNextTile()));
		}
	}
    
    // Add a tile to the foreground
    GameObject InstantiateTile(Sprite sprite) {
        GameObject tile = new GameObject(CleanSpriteName(sprite.ToString()));
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
        tile.transform.position = nextPosition;
        nextPosition.x += tile.transform.localScale.x*2;
        return tile;
    }
    
    // Remove terrain type and version number from sprite name
    string CleanSpriteName(string name) {
        name = name.Substring(name.IndexOf("_") + 1);
        return name.Substring(0, name.LastIndexOf("_"));
    }
    
    Sprite GetNextTile() {
        return TileFromDistribution(afterDistributions[tiles.Peek().name]);
    }
    
    Sprite TileFromDistribution(SortedDictionary<double, Sprite[]> distribution) {
        double picker = Random.value;
        double sum = 0;
        foreach(KeyValuePair<double, Sprite[]> pair in distribution) {
            sum += pair.Key;
            if(picker < sum) {
                return pair.Value[Random.Range(0, pair.Value.Length)];
            }
        }
        Debug.Log("oops");
        return flat[0];
    }
    
}
