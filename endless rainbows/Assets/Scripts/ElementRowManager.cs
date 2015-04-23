using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementRowManager : MonoBehaviour {
    
    public Sprite[] elements;
    private bool wasElement = false;
    private float previousElementHeight = 4f;
    public float depth;
    public float minScale;
    public int spriteOrder;
    public bool overlap;
    bool even = true;
    public float maxDepthVariation;
    public float minElementElevation;
    public float maxElementElevation;
    public float maxElementDistance;
    public float startElementStringChance;
    public float continueElementStringChance;
    
	// Use this for initialization
	void Start() {
        GameEventManager.NewColumn += NewColumn;
	}
	
	// Update is called once per frame
	void Update() {
	}
    
    // Add Element to the foreground based on the previous Element
    void NewColumn(Vector3 position) {
        if((!wasElement && Random.value < startElementStringChance) || (wasElement && Random.value < continueElementStringChance)) {
            wasElement = true;
            
            Sprite sprite = elements[Random.Range(0, elements.Length)];
            GameObject element = new GameObject(sprite.ToString());
            element.transform.parent = this.transform;
            element.AddComponent<Item>();
            float height = Mathf.Clamp(Random.Range(-maxElementDistance, maxElementDistance) + previousElementHeight, minElementElevation, maxElementElevation);
            position.y += height;
            position.z = depth + Random.Range(-maxDepthVariation, maxDepthVariation);
            previousElementHeight = height;
            element.transform.position = position;
            element.transform.localScale *= Random.Range(minScale, 1) * Mathf.Max(depth/4f, 1f);
            SpriteRenderer renderer = element.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = spriteOrder;
            if(overlap) {
                if(even) {
                    renderer.sortingOrder += 1;
                }
                even = !even;
            }
            
        } else {
            wasElement = false;
            previousElementHeight = Random.Range(minElementElevation, maxElementElevation);
        }
    }
    
}