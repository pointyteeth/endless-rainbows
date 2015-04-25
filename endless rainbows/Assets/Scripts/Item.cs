using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	// Use this for initialization
	void Start() {
        GameEventManager.GameOver += GameOver;
	}
	
	// Update is called once per frame
	void Update() {
        if(Camera.main.WorldToViewportPoint(this.transform.position).x < -0.2 || Camera.main.WorldToViewportPoint(this.transform.position).y > 2.5) {
            Object.Destroy(gameObject);
        }
	}
    
    void OnDestroy() {
    }
    
    void GameOver() {
        if(this != null) {
            Object.Destroy(gameObject);
        }
    }
}
