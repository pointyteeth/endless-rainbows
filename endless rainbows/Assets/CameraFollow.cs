using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    float offset;

	// Use this for initialization
	void Start () {
        offset = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        // Maintain y-axis
        Vector3 position = transform.position;
        position.y = 0f + offset;
        transform.position = position;
	}
}
