using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

    public PhysicsMaterial2D physicsMaterial;

    public Sprite blank;
    public Sprite[] fill;
    public Sprite[] flat;
    public Sprite[] diagonalDown;
    public Sprite[] diagonalDownCorner;
    public Sprite[] verticalDown;
    public Sprite[] verticalUp;
    public Sprite[] verticalRight;
    public Sprite[] verticalLeft;
    
    private Dictionary<string, SortedDictionary<double, Sprite[]>> afterDistributions;
    private Dictionary<string, SortedDictionary<double, Sprite[]>> aboveDistributions;
    private Dictionary<string, SortedDictionary<double, Sprite[]>> belowDistributions;
    private List<string> deadEnds = new List<string>() {"diagonal_down"};
    
    public Vector3 startPosition;
    public int numberOfTilesX;
    public int numberOfTilesY;
    public float spawnBuffer;
    public float destroyBuffer;
    private Vector3 nextPosition;
    private Vector3 nextColumnPosition;
    private GameObject nextTile;
    private GameObject nextColumnTile;
    private List<GameObject> tiles;
    
	// Use this for initialization
	void Start() {
        afterDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
            {"blank", new SortedDictionary<double, Sprite[]> {
                {1.0, verticalUp}
            }},
            {"flat", new SortedDictionary<double, Sprite[]> {
                {0.7, flat},
                {0.2, diagonalDown},
                {0.1, verticalDown}
            }},
            {"diagonal_down", new SortedDictionary<double, Sprite[]> {
            }},
            {"diagonal_down_corner", new SortedDictionary<double, Sprite[]> {
                {0.8, flat},
                {0.1, diagonalDown}
            }},
            {"vertical_up", new SortedDictionary<double, Sprite[]> {
                {1.0, flat}
            }},
            {"vertical_down", new SortedDictionary<double, Sprite[]> {
            }}
        };
        aboveDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
            {"blank", new SortedDictionary<double, Sprite[]> {
            }},
            {"flat", new SortedDictionary<double, Sprite[]> {
            }},
            {"diagonal_down", new SortedDictionary<double, Sprite[]> {
            }},
            {"vertical_up", new SortedDictionary<double, Sprite[]> {
            }},
            {"vertical_down", new SortedDictionary<double, Sprite[]> {
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
            }},
            {"vertical_up", new SortedDictionary<double, Sprite[]> {
                {1.0, verticalLeft}
            }},
            {"vertical_down", new SortedDictionary<double, Sprite[]> {
                {1.0, verticalRight}
            }},
            {"vertical_left", new SortedDictionary<double, Sprite[]> {
                {1.0, verticalLeft}
            }},
            {"vertical_right", new SortedDictionary<double, Sprite[]> {
                {1.0, verticalRight}
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
            if(Camera.main.WorldToViewportPoint(tile.transform.position).x < destroyBuffer) {
                Object.Destroy(tile);
                tiles.Remove(tile);
            }
        }
        if(Camera.main.WorldToViewportPoint(nextTile.transform.position).x < spawnBuffer) {
            newColumn();
		}
	}
    
    // Generate a new column of tiles
    void newColumn() {
        nextPosition.x += 2;
        nextTile = InstantiateTile(TileFromDistribution(afterDistributions[nextTile.name]), nextPosition);
        nextColumnTile = nextTile;
        nextColumnPosition = nextPosition;
        AddFood(nextTile.transform.position);
        GameEventManager.AddNewColumn(nextTile.transform.position);
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
        tile.transform.position = position;
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        if(tile.name != "blank") {
            BoxCollider2D collider = tile.AddComponent<BoxCollider2D>();
            //PolygonCollider2D collider = tile.AddComponent<PolygonCollider2D>();
            //collider.sharedMaterial = physicsMaterial;
        }
        tiles.Add(tile);
        return tile;
    }
    
    // Remove terrain type, version number, and type from sprite name
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
    
    public Sprite[] foods;
    private bool wasFood = false;
    public int maxFoodElevation;
    public float startFoodStringChance;
    public float continueFoodStringChance;
    
    // Add a food to the foreground based on the previous food
    void AddFood(Vector3 position) {
        if((!wasFood && Random.value < startFoodStringChance) || (wasFood && Random.value < continueFoodStringChance)) {
            wasFood = true;
            
            int number = Random.Range(0, foods.Length);
            Sprite sprite = foods[number];
            int height = Random.Range(2, 2 + maxFoodElevation);
            GameObject food = new GameObject(CleanSpriteName(sprite.ToString()));
            position.y += height;
            food.transform.position = position;
            SpriteRenderer renderer = food.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            BoxCollider2D collider = food.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            Food script = food.AddComponent<Food>();
            script.PointValue = 100*(number + 1);
            
        } else {
            wasFood = false;
        }
    }
}
