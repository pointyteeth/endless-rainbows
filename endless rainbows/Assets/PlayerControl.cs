using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;
    public float[] jumpSpeed = new float[3];
    
    int jumpCounter = 0;

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
        // Keep running
        transform.Translate(speed*Time.deltaTime, 0f, 0f);
        
        // Jumping
        if(Input.GetButtonDown("Jump") && jumpCounter < 3) {
            rigidbody.AddForce(Vector3.up*jumpSpeed[jumpCounter]);
            jumpCounter++;
		}
	}
    
    void OnCollisionEnter(Collision collision) {
        jumpCounter = 0;
    }
    
}
