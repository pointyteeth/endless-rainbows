using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;
    public float[] jumpSpeed = new float[3];
    
    int jumpCounter = 0;

	// Use this for initialization
	void Start() {
        rigidbody2D.fixedAngle = true;
	}
	
	// Update is called once per frame
	void Update() {
        // Keep running
        transform.Translate(speed*Time.deltaTime, 0f, 0f);
        
        // Jumping
        if(Input.GetButtonDown("Jump") && jumpCounter < 3) {
            rigidbody2D.AddForce(Vector3.up*jumpSpeed[jumpCounter]);
            jumpCounter++;
		}
	}
    
    void OnCollisionEnter2D(Collision2D collision) {
        jumpCounter = 0;
    }
    
}
