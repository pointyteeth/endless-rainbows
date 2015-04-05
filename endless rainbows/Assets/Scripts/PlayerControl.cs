using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerControl : MonoBehaviour {

    public float maximumSpeed;
    public float speed;
    public float[] jumpSpeed = new float[3];
    int jumpCounter = 0;
    Rigidbody2D rigidbody;
    
    public float itemTime;
    ScreenOverlay medal;
    
	// Use this for initialization
	void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        medal = Camera.main.GetComponent<ScreenOverlay>();
	}
    
    void FixedUpdate() {
        // Keep running
        rigidbody.AddForce(Vector3.right*speed);
        rigidbody.velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maximumSpeed);
    }
	
	// Update is called once per frame
	void Update() {
        // Jumping
        if(Input.GetButtonDown("Jump") && jumpCounter < 3) {
            rigidbody.AddForce(Vector3.up*jumpSpeed[jumpCounter]);
            jumpCounter++;
		}
	}
    
    void OnCollisionEnter2D(Collision2D collision) {
        jumpCounter = 0;
    }
    
    void OnTriggerExit2D(Collider2D collider) {
        GameObject thing = collider.gameObject;
        if(thing.GetComponent<Food>() != null) {
            GameEventManager.AddPoints(thing.GetComponent<Food>().PointValue, thing.transform.position);
        } else {
            GameEventManager.AddItem(thing.name);
            switch(thing.name) {
                case "medal":
                    medal.enabled = true;
                    CancelInvoke("DisableMedal");
                    Invoke("DisableMedal", itemTime);
                    break;
                default:
                    break;
            }
        }
        Object.Destroy(thing);
    }
    
    void DisableMedal() {
        medal.enabled = false;
    }
    
}
