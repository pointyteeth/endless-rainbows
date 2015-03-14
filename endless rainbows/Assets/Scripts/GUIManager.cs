using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    public Text points;
    
    void Start() {
        GameEventManager.UpdatePoints += UpdatePoints;
	}

	void Update() {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
    
    void UpdatePoints(int value) {
        points.text = value.ToString();
    }
    
}