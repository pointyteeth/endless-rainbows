using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject player;
    private int goneFor = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(!player.renderer.isVisible) {
            goneFor++;
        }
        if(goneFor > 3) {
            Time.timeScale = 0;
        }
	}
}
