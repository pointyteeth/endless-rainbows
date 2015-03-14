using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float maximumSpeed;
    public float speed;
    public float[] jumpSpeed = new float[3];
    int jumpCounter = 0;
    
	// Use this for initialization
	void Start() {
        //rigidbody2D.fixedAngle = true;
	}
    
    void FixedUpdate() {
        // Keep running
        rigidbody2D.AddForce(Vector3.right*speed);
        rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity, maximumSpeed);
    }
	
	// Update is called once per frame
	void Update() {
        // Jumping
        if(Input.GetButtonDown("Jump") && jumpCounter < 3) {
            rigidbody2D.AddForce(Vector3.up*jumpSpeed[jumpCounter]);
            jumpCounter++;
		}
	}
    
    void OnCollisionEnter2D(Collision2D collision) {
        jumpCounter = 0;
    }
    
    void OnTriggerExit2D(Collider2D collider) {
        GameObject thing = collider.gameObject;
        GameEventManager.AddPoints(thing.GetComponent<Food>().PointValue);
        Object.Destroy(thing);
    }
    
}
