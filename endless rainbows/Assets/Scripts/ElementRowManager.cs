using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementRowManager : MonoBehaviour {
    
    private List<GameObject> tiles;
    
    public Sprite[] elements;
    private bool wasElement = false;
    private float previousElementHeight = 4f;
    public float depth;
    public float maxDepthVariation;
    public float minElementElevation;
    public float maxElementElevation;
    public float maxElementDistance;
    public float startElementStringChance;
    public float continueElementStringChance;
    
	// Use this for initialization
	void Start() {
        tiles = new List<GameObject>();
        GameEventManager.NewColumn += NewColumn;
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
	}
    
    // Add Element to the foreground based on the previous Element
    void NewColumn(Vector3 position) {
        if((!wasElement && Random.value < startElementStringChance) || (wasElement && Random.value < continueElementStringChance)) {
            wasElement = true;
            
            Sprite sprite = elements[Random.Range(0, elements.Length)];
            GameObject element = new GameObject(sprite.ToString());
            element.transform.parent = this.transform;
            tiles.Add(element);
            float height = Mathf.Clamp(Random.Range(-maxElementDistance, maxElementDistance) + previousElementHeight, minElementElevation, maxElementElevation);
            position.y += height;
            position.z = depth + Random.Range(-maxDepthVariation, maxDepthVariation);
            previousElementHeight = height;
            element.transform.position = position;
            element.transform.localScale *= depth/5f;
            SpriteRenderer renderer = element.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            
        } else {
            wasElement = false;
            previousElementHeight = Random.Range(minElementElevation, maxElementElevation);
        }
    }
}