using UnityEngine;

public class GUIManager : MonoBehaviour {

	public GUIText gameStartText, gameOverText;
    
    void Start () {
		gameOverText.enabled = false;
	}

	void Update () {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
    
}