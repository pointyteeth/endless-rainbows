using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;
    private float oldY;
    public float cameraSpeed;

	// Use this for initialization
	void Start () {
        oldY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        // Smooth y-axis
        transform.Translate(0f, (player.transform.position.y - oldY)*cameraSpeed, 0f);
        oldY = transform.position.y;
        
        // Maintain rotation
        //transform.rotation = Quaternion.identity;
	}
}
