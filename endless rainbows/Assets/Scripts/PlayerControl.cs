using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class PlayerControl : MonoBehaviour {

    public float maximumSpeed;
    public float speed;
    public float[] jumpSpeed = new float[3];
    public float[] normalJumpSpeed = new float[3];
    public float[] sassyJumpSpeed = new float[3];
    int jumpCounter = 0;
    bool grounded = false;
    Rigidbody2D rb;
    
    public GameObject horse;
    Animator animator;
    public ParticleSystem glitter;
    
    public GameObject tiles;
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
    public GameObject rifle;
    public GameObject sass;
    RuntimeAnimatorController originalHorse;
    public RuntimeAnimatorController unicorn;
    
	// Use this for initialization
	void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = horse.GetComponent<Animator>();
        multiplier = multiplierGraphic.GetComponent<Multiplier>();
        medal = Camera.main.GetComponent<ScreenOverlay>();
        flashRenderer = flash.GetComponent<CanvasRenderer>();
        flashRenderer.SetAlpha(0);
        jumpSpeed = normalJumpSpeed;
        originalHorse = animator.runtimeAnimatorController;
        GameEventManager.GameOver += GameOver;
	}
    
    void FixedUpdate() {
        // Keep running
        rb.AddForce(Vector3.right*speed);
        rb.velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maximumSpeed);
    }
	
	// Update is called once per frame
	void Update() {
        // Check if grounded
        if(GameEventManager.scene) {
            if(GameEventManager.scene) {
                if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.75f).collider != null) {
                    jumpCounter = 0;
                    animator.SetBool("grounded", true);
                } else if(Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), 3f).collider != null) {
                    if(!grounded) {
                        grounded = true;
                        animator.SetTrigger("land");
                    }
                } else if(grounded) {
                    grounded = false;
                    animator.SetBool("grounded", false);
                }
            }
            // Jumping
            if(Input.GetButtonDown("Jump")) {
                if(jumpCounter < 3) {
                    rb.AddForce(Vector3.up*jumpSpeed[jumpCounter]);
                    jumpCounter++;
                    if(jumpCounter == 3) {
                        animator.SetTrigger("flip");
                        glitter.emissionRate = 150;
                        Invoke("ResetGlitter", 1);
                    } else {
                        animator.SetTrigger("jump");
                    }
                }
            }
        }
	}
    
    void ResetGlitter() {
        glitter.emissionRate = 15;
    }
    
    void OnTriggerExit2D(Collider2D collider) {
        GameObject thing = collider.gameObject;
        if(thing.transform.parent.gameObject != tiles) {
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
                    case "rifle":
                        rifle.SetActive(true);
                        CancelInvoke("DisableRifle");
                        Invoke("DisableRifle", itemTime);
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
                    case "sass":
                        jumpSpeed = sassyJumpSpeed;
                        sass.SetActive(true);
                        Invoke("DisableSass", itemTime);
                        break;
                    case "medal":
                        medal.enabled = true;
                        CancelInvoke("DisableMedal");
                        Invoke("DisableMedal", itemTime);
                        break;
                    case "botox":
                        animator.runtimeAnimatorController = unicorn;
                        Invoke("DisableBotox", itemTime);
                        break;
                    default:
                        break;
                }
            }
            Object.Destroy(thing);
        }
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
        rifle.SetActive(false);
    }
    
    void DisableSass() {
        jumpSpeed = normalJumpSpeed;
        sass.SetActive(false);
    }
    
    void DisableBelt() {
    }
    
    void DisableBotox() {
        animator.runtimeAnimatorController = originalHorse;
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
        DisableBotox();
    }
}
