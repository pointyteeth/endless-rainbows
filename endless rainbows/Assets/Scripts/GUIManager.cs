using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {

    public Canvas worldCanvas;
    public Text pointValue;
    
    public Canvas screenCanvas;
    public Text pointTotal;
    public Text itemTitle;
    public Text itemDescription;
    
    void Start() {
        GameEventManager.UpdatePoints += UpdatePoints;
        GameEventManager.StartItem += StartItem;
	}

	void Update() {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
    
    void UpdatePoints(int total, int points, Vector3 position) {
        pointTotal.text = total.ToString();
        Text label = Instantiate(pointValue);
        label.text += points.ToString();
        label.rectTransform.position = position;
        label.rectTransform.SetParent(worldCanvas.transform);
    }
    
    void StartItem(string name) {
        itemTitle.text = name;
    }
    
}