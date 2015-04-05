using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    public Rigidbody2D rigidbody;
    float minY;
    float maxY;

    private int pointValue;
    public int PointValue {
       get { return pointValue; }
       set { pointValue = value; }
    }

	// Use this for initialization
	void Start() {
        maxY = transform.position.y;
        minY = maxY - 0.01f;
	}
	
	// Update is called once per frame
	void Update() {
        if((transform.position.y < minY && rigidbody.gravityScale > 0) || (transform.position.y > maxY && rigidbody.gravityScale < 0)) {
            rigidbody.gravityScale *= -1;
        }
	}
}
