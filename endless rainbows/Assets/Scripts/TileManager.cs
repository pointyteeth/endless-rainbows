using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

    public PhysicsMaterial2D physicsMaterial;
    public GameObject player;
    public GameObject tilesObject;
    public GameObject foodObject;
    public GameObject itemsObject;

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
    
    public float startDrop;
    public int numberOfTilesY;
    public float spawnBuffer;
    public float destroyBuffer;
    private Vector3 nextPosition;
    private Vector3 nextColumnPosition;
    private GameObject nextTile;
    private GameObject nextColumnTile;
    private int gapLength = 0;
    public int maxGap;
    
	// Use this for initialization
	void Start() {
        afterDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
            {"blank", new SortedDictionary<double, Sprite[]> {
                {0.4, verticalUp}
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
                {0.1, diagonalDown},
                {0.2, verticalDown}
            }},
            {"vertical_up", new SortedDictionary<double, Sprite[]> {
                {1.0, flat}
            }},
            {"vertical_down", new SortedDictionary<double, Sprite[]> {
            }}
        };
        /*aboveDistributions = new Dictionary<string, SortedDictionary<double, Sprite[]>> {
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
        };*/
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
        
        if(debugItems) {
            itemChance = debuggingItemsChance;
        }
	}
	
	// Update is called once per frame
	void Update() {
        if(GameEventManager.game) {
            if(nextTile == null) {
                if(!GameEventManager.firstScene) {
                    GameEventManager.EndedScene();
                }
                Vector3 position = player.transform.position;
                position.y -= startDrop;
                position.x -= 10;
                nextPosition = position;
                nextTile = InstantiateTile(flat[0], nextPosition);
                for(int i = 0; i < 20; i++) {
                    MakeNewColumn(flat[Random.Range(0, flat.Length)]);
                }
                Invoke("MakeNewScene", 4);
            }
            else if(Camera.main.WorldToViewportPoint(nextTile.transform.position).x < spawnBuffer) {
                MakeNewColumn(TileFromDistribution(afterDistributions[nextTile.name]));
            }
        }
	}
    
    void MakeNewScene() {
        if(GameEventManager.game) {
            GameEventManager.AddedNewScene();
        }
    }
    
    // Generate a new column of tiles, starting with the given sprite
    void MakeNewColumn(Sprite next) {
        nextPosition.x += 2;
        if(next.name == "blank") {
            if(gapLength == maxGap) {
                next = verticalUp[0];
                gapLength = 0;
            } else {
                gapLength++;
            }
        }
        nextTile = InstantiateTile(next, nextPosition);
        nextColumnTile = nextTile;
        nextColumnPosition = nextPosition;
        AddFood(nextTile.transform.position);
        if(nextTile.name != "blank") {
            GameEventManager.AddNewColumn(nextTile.transform.position);
        }
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
            tile.AddComponent<BoxCollider2D>();
            //tile.AddComponent<PolygonCollider2D>();
            //collider.sharedMaterial = physicsMaterial;
        }
        tile.AddComponent<Item>();
        tile.transform.parent = tilesObject.transform;
        return tile;
    }
    
    // Remove terrain type, version number, and type from sprite name
    string CleanSpriteName(string name) {
        if(name.Contains("_")) {
            name = name.Substring(name.IndexOf("_") + 1);
            if(name.Contains("_")) {
                name = name.Substring(0, name.LastIndexOf("_"));
            }
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
    public Sprite[] items;
    bool wasFood = false;
    public int minFoodElevation;
    public int maxFoodElevation;
    public float startFoodStringChance;
    public float continueFoodStringChance;
    public float itemChance;
    public bool debugItems;
    public float debuggingItemsChance;
    
    // Add a food to the foreground based on the previous food
    // Possibly add an item instead
    void AddFood(Vector3 position) {
        if((!wasFood && Random.value < startFoodStringChance) || (wasFood && Random.value < continueFoodStringChance)) {
            wasFood = true;
            
            int height = Random.Range(2 + minFoodElevation, 2 + maxFoodElevation);
            GameObject item = new GameObject();
            position.y += height;
            item.transform.position = position;
            BoxCollider2D collider = item.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            item.AddComponent<Item>();
            
            Sprite sprite;
            if(Random.value > itemChance) { // place food
                int number = (int) (Mathf.Pow(Random.value, 2)*foods.Length);
                sprite = foods[number];
                Food foodScript = item.AddComponent<Food>();
                foodScript.PointValue = 100*(number + 1);
                Rigidbody2D rigidbody = item.AddComponent<Rigidbody2D>();
                foodScript.rb = rigidbody;
                item.transform.parent = foodObject.transform;
            } else { // place item
                sprite = items[Random.Range(0, items.Length)];
                item.transform.parent = itemsObject.transform;
            }
            item.name = CleanSpriteName(sprite.ToString());
            SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            
        } else {
            wasFood = false;
        }
    }
}
