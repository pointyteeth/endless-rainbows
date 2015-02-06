using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

    public Sprite blank;
    public Sprite[] fill;
    public Sprite[] flat;
    public Sprite[] diagonalDown;
    public Sprite[] diagonalDownCorner;
    
    private Dictionary<string, SortedDictionary<double, Sprite[]>> afterDistributions;
    private Dictionary<string, SortedDictionary<double, Sprite[]>> aboveDistributions;
    private Dictionary<string, SortedDictionary<double, Sprite[]>> belowDistributions;
    private List<string> deadEnds = new List<string>() {"diagonal_down"};
    
    public Vector3 startPosition;
    public int numberOfTilesX;
    public int numberOfTilesY;
    private Vector3 nextPosition;
    private Vector3 nextColumnPosition;
    private GameObject nextTile;
    private GameObject nextColumnTile;
    private List<GameObject> tiles;
    
	// Use this for initialization
	void Start() {
        afterDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
            {"blank", new SortedDictionary<double, Sprite[]> {
                {1.0, flat}
            }},
            {"flat", new SortedDictionary<double, Sprite[]> {
                {0.7, flat},
                {0.2, diagonalDown}
            }},
            {"diagonal_down", new SortedDictionary<double, Sprite[]> {
            }},
            {"diagonal_down_corner", new SortedDictionary<double, Sprite[]> {
                {0.8, flat},
                {0.1, diagonalDown}
            }}
        };
        aboveDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
            {"blank", new SortedDictionary<double, Sprite[]> {
            }},
            {"flat", new SortedDictionary<double, Sprite[]> {
            }},
            {"diagonal_down", new SortedDictionary<double, Sprite[]> {
            }}
        };
        belowDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
            {"blank", new SortedDictionary<double, Sprite[]> {
            }},
            {"flat", new SortedDictionary<double, Sprite[]> {
                {1.0, fill}
            }},
            {"fill", new SortedDictionary<double, Sprite[]> {
                {1.0, fill}
            }},
            {"diagonal_down", new SortedDictionary<double, Sprite[]> {
                {1.0, diagonalDownCorner}
            }},
            {"diagonal_down_corner", new SortedDictionary<double, Sprite[]> {
                {1.0, fill}
            }}
        };
    
        tiles = new List<GameObject>();
        nextPosition = startPosition;
        nextTile = InstantiateTile(flat[0], nextPosition);
        for(int i = 0; i < numberOfTilesX; i++) {
            newColumn();
		}
	}
	
	// Update is called once per frame
	void Update() {
        for(int i = 0; i < tiles.Count; i++) {
            GameObject tile = tiles[i];
            if(Camera.main.WorldToViewportPoint(tile.transform.position).x < -0.2) {
                Object.Destroy(tile);
                tiles.Remove(tile);
            }
        }
        if(nextTile.renderer.isVisible) {
            newColumn();
		}
	}
    
    // Generate a new column of tiles
    void newColumn() {
        nextPosition.x += 2;
        nextTile = InstantiateTile(TileFromDistribution(afterDistributions[nextTile.name]), nextPosition);
        nextColumnTile = nextTile;
        nextColumnPosition = nextPosition;
        for(int i = 0; i < numberOfTilesY; i++) {
            nextColumnPosition.y -= 2;
            nextColumnTile = InstantiateTile(TileFromDistribution(belowDistributions[nextColumnTile.name]), nextColumnPosition);
            if(IsDeadEnd(nextTile)) {
                nextTile = nextColumnTile;
                nextPosition = nextColumnPosition;
            }
		}
    }
    
    // Check to see if this tile is a dead end tile, and nextTile should be transferred
    bool IsDeadEnd(GameObject tile) {
        return deadEnds.Contains(tile.name);
    }
    
    // Add a tile to the foreground
    GameObject InstantiateTile(Sprite sprite, Vector3 position) {
        GameObject tile = new GameObject(CleanSpriteName(sprite.ToString()));
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        if(tile.name != "blank") {
            BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
        }
        tile.transform.position = position;
        tiles.Add(tile);
        return tile;
    }
    
    // Remove terrain type and version number from sprite name
    string CleanSpriteName(string name) {
        if(name.Contains("_")) {
            name = name.Substring(name.IndexOf("_") + 1);
            name = name.Substring(0, name.LastIndexOf("_"));
        }
        if(name.Contains("(")) {
            name = name.Substring(0, name.LastIndexOf("(") - 1);
        }
        return name;
    }
    
    // Select a sprite from a distribution of sprites
    Sprite TileFromDistribution(SortedDictionary<double, Sprite[]> distribution) {
        double picker = Random.value;
        double sum = 0;
        foreach(KeyValuePair<double, Sprite[]> pair in distribution) {
            sum += pair.Key;
            if(picker < sum) {
                return pair.Value[Random.Range(0, pair.Value.Length)];
            }
        }
        return blank;
    }
    
}
