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
    public int itemTextTime;
    public Dictionary<string, string[]> descriptions;
    
    void Start() {
        itemTitle.enabled = false;
        itemDescription.enabled = false;
        descriptions = new Dictionary<string, string[]> {
                {"sunglasses", new[] {"sunglasses", "You can't spell \"badass\" without \"sunglasses\"."}},
                {"tie", new[] {"tie", "A gentleman in the streets..."}},
                {"handcuffs", new[] {"handcuffs", "Now you can get kinky with the opposition."}},
                {"dog", new[] {"dog", "fear is just love spelled differently"}},
                {"badge", new[] {"KGB badge", "You never saw this."}},
                {"heart", new[] {"heart", "The loving populace gives you their hearts. And their votes."}},
                {"camera", new[] {"camera", "Smile for the media!"}},
                {"octopus", new[] {"octopus", "Octopi Ukraine!"}},
                {"rifle", new[] {"rifle", "Time to hunt for cute photo ops."}},
                {"sass", new[] {"can of sass", "You must obey the law, always, not only when they grab you by your special place."}},
                {"belt", new[] {"judo belt", "The gentle way."}},
                {"medal", new[] {"olympic medal", "Gold medal in tolerating the gays!"}},
                {"botox", new[] {"botox", "~*¤{ p r e t t y }¤*~"}}
            };
        GameEventManager.UpdatePoints += UpdatePoints;
        GameEventManager.StartItem += StartItem;
	}

	void Update() {
		if(Input.GetButtonDown("Jump")){
			GameEventManager.TriggerGameStart();
		}
	}
    
    void UpdatePoints(long total, int points, Vector3 position) {
        pointTotal.text = total.ToString();
        Text label = Instantiate(pointValue);
        label.text += points.ToString();
        label.rectTransform.position = position;
        label.rectTransform.SetParent(worldCanvas.transform);
        label.CrossFadeAlpha(0, itemTextTime, false);
    }
    
    void StartItem(string name) {
        itemTitle.enabled = true;
        itemDescription.enabled = true;
        itemTitle.text = descriptions[name][0];
        itemDescription.text = descriptions[name][1];
        CancelInvoke("HideItemText");
        Invoke("HideItemText", itemTextTime);
    }
    
    void HideItemText() {
        itemTitle.enabled = false;
        itemDescription.enabled = false;
    }
    
}