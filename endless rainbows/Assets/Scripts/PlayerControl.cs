using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerControl : MonoBehaviour {

    public float maximumSpeed;
    public float speed;
    public float[] jumpSpeed = new float[3];
    int jumpCounter = 0;
    Rigidbody2D rb;
    
    public float itemTime;
    public GameObject multiplierGraphic;
    Multiplier multiplier;
    ScreenOverlay medal;
    public GameObject shirt;
    SpriteRenderer shirtRenderer;
    public GameObject harness;
    SpriteRenderer harnessRenderer;
    
	// Use this for initialization
	void Start() {
        rb = GetComponent<Rigidbody2D>();
        multiplier = multiplierGraphic.GetComponent<Multiplier>();
        medal = Camera.main.GetComponent<ScreenOverlay>();
        shirtRenderer = shirt.GetComponent<SpriteRenderer>();
        harnessRenderer = harness.GetComponent<SpriteRenderer>();
	}
    
    void FixedUpdate() {
        // Keep running
        rb.AddForce(Vector3.right*speed);
        rb.velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maximumSpeed);
    }
	
	// Update is called once per frame
	void Update() {
        // Jumping
        if(Input.GetButtonDown("Jump") && jumpCounter < 3) {
            rb.AddForce(Vector3.up*jumpSpeed[jumpCounter]);
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
                case "tie":
                    shirtRenderer.enabled = true;
                    CancelInvoke("DisableTie");
                    Invoke("DisableTie", itemTime);
                    break;
                case "handcuffs":
                    harnessRenderer.enabled = true;
                    CancelInvoke("DisableHandcuffs");
                    Invoke("DisableHandcuffs", itemTime);
                    break;
                case "dog":
                    GameEventManager.MultiplyPoints(2);
                    multiplier.Show(thing.GetComponent<SpriteRenderer>().sprite, "x2");
                    break;
                case "badge":
                    multiplier.Show(thing.GetComponent<SpriteRenderer>().sprite, "x3");
                    GameEventManager.MultiplyPoints(3);
                    break;
                case "heart":
                    multiplier.Show(thing.GetComponent<SpriteRenderer>().sprite, "^2");
                    GameEventManager.MultiplyPoints(GameEventManager.points);
                    break;
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
    
    void DisableTie() {
        shirtRenderer.enabled = false;
    }
    
    void DisableHandcuffs() {
        harnessRenderer.enabled = false;
    }
    
}
