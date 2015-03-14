using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    private int pointValue;
    public int PointValue {
       get { return pointValue; }
       set { pointValue = value; }
    }

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
        if(Camera.main.WorldToViewportPoint(this.transform.position).x < -0.2) {
            Object.Destroy(gameObject);
        }
	}
}
