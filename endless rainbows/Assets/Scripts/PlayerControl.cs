using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerControl : MonoBehaviour {

    public float maximumSpeed;
    public float speed;
    public float[] jumpSpeed = new float[3];
    int jumpCounter = 0;
    Rigidbody2D rb;
    bool falling;
    
    public float itemTime;
    public GameObject multiplierGraphic;
    Multiplier multiplier;
    ScreenOverlay medal;
    public GameObject sunglasses;
    public GameObject shirt;
    public GameObject harness;
    public GameObject foodObject;
    public RawImage flash;
    CanvasRenderer flashRenderer;
    public GameObject octopus;
    
	// Use this for initialization
	void Start() {
        rb = GetComponent<Rigidbody2D>();
        multiplier = multiplierGraphic.GetComponent<Multiplier>();
        medal = Camera.main.GetComponent<ScreenOverlay>();
        flashRenderer = flash.GetComponent<CanvasRenderer>();
        flashRenderer.SetAlpha(0);
        GameEventManager.NewScene += NewScene;
        GameEventManager.GameOver += GameOver;
	}
    
    void NewScene() {
        falling = false;
    }
    
    void FixedUpdate() {
        // Keep running
        rb.AddForce(Vector3.right*speed);
        rb.velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maximumSpeed);
    }
	
	// Update is called once per frame
	void Update() {
        // Jumping
        if((Input.GetButtonDown("Jump") || Input.touchCount > 0) && jumpCounter < 3 && !falling) {
            rb.AddForce(Vector3.up*jumpSpeed[jumpCounter]);
            jumpCounter++;
		}
	}
    
    void OnCollisionEnter2D(Collision2D collision) {
        foreach(ContactPoint2D contact in collision.contacts) {
            if(contact.normal.y == 0 && contact.normal.x == -1) {
                falling = true;
            }
        }
        jumpCounter = 0;
    }
    
    void OnTriggerExit2D(Collider2D collider) {
        GameObject thing = collider.gameObject;
        if(thing.GetComponent<Food>() != null) {
            GameEventManager.AddPoints(thing.GetComponent<Food>().PointValue, thing.transform.position);
        } else {
            GameEventManager.AddItem(thing.name);
            switch(thing.name) {
                case "sunglasses":
                    sunglasses.SetActive(true);
                    CancelInvoke("DisableSunglasses");
                    Invoke("DisableSunglasses", itemTime);
                    break;
                case "tie":
                    shirt.SetActive(true);
                    CancelInvoke("DisableTie");
                    Invoke("DisableTie", itemTime);
                    break;
                case "handcuffs":
                    harness.SetActive(true);
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
                case "octopus":
                    octopus.SetActive(true);
                    CancelInvoke("DisableOctopus");
                    Invoke("DisableOctopus", itemTime);
                    break;
                case "camera":
                    flashRenderer.SetAlpha(1);
                    flash.CrossFadeAlpha(0, 0.5f, false);
                    Vector3 position;
                    foreach (Transform food in foodObject.transform) {
                        position = Camera.main.WorldToViewportPoint(food.position);
                        if(position.x > 0 && position.x < 1 && position.y > 0 && position.y < 1) {
                            GameEventManager.AddPoints(food.GetComponent<Food>().PointValue, food.transform.position);
                            Object.Destroy(food.gameObject);
                        }
                    }
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
    
    void DisableSunglasses() {
        sunglasses.SetActive(false);
    }
    
    void DisableTie() {
        shirt.SetActive(false);
    }
    
    void DisableHandcuffs() {
        harness.SetActive(false);
    }
    
    void DisableOctopus() {
        octopus.SetActive(false);
    }
    
    void DisableRifle() {
    }
    
    void DisableSass() {
    }
    
    void DisableBelt() {
    }
    
    void Botox() {
    }
    
    void GameOver() {
        DisableMedal();
        DisableSunglasses();
        DisableTie();
        DisableHandcuffs();
        DisableOctopus();
        DisableRifle();
        DisableSass();
        DisableBelt();
        Botox();
    }
}
