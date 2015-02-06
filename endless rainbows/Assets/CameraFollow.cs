using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // Smooth y-axis and follow x-axis
        Vector3 position = transform.position;
        position.x = player.transform.position.x;
        //position.y = player.transform.position.y;
        transform.position = position;
        
        // Maintain rotation
        //transform.rotation = Quaternion.identity;
	}
}
