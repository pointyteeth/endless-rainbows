using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

    public Rigidbody2D rb;
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
        rb.gravityScale = 0.5f;
	}
	
	// Update is called once per frame
	void Update() {
        if((transform.position.y < minY && rb.gravityScale > 0) || (transform.position.y > maxY && rb.gravityScale < 0)) {
            rb.gravityScale *= -1;
        }
	}
}
