using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
// HIDE CURSORZZZZ
public class GUIManager : MonoBehaviour {

    public Canvas worldCanvas;
    public Text pointValue;
    
    public Canvas screenCanvas;
    public Text pointTotal;
    public Text itemTitle;
    public Text itemDescription;
    public int itemTextTime;
    public Dictionary<string, string[]> descriptions;
    public Text continueText;
    string control = "spacebar";
    public Image startScreen;
    
    void Start() {
    
        #if UNITY_WEBPLAYER || UNITY_EDITOR
            control = "spacebar";
        #endif
    
        #if UNITY_STANDALONE
            control = "press button";
        #endif
        
        #if UNITY_ANDROID
            control = "tap";
        #endif
        
        Cursor.visible = false;
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
        GameEventManager.GameStart += GameStart;
        GameEventManager.UpdatePoints += UpdatePoints;
        GameEventManager.StartItem += StartItem;
        GameEventManager.EndScene += EndScene;
        GameEventManager.NewScene += NewScene;
	}

	void Update() {
        if(Input.GetButtonDown("Jump")) {
            if(!GameEventManager.game){
                GameEventManager.TriggerGameStart();
                startScreen.enabled = false;
            }
            else if(!GameEventManager.scene && !GameEventManager.firstScene){
                GameEventManager.TriggerGameOver();
                startScreen.enabled = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
	}
    
    void GameStart() {
        startScreen.enabled = true;
        continueText.text = control + " to jump";
        continueText.enabled = true;
        itemTitle.enabled = false;
        itemDescription.enabled = false;
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
    
    void EndScene() {
        continueText.text = control + " to quit";
        continueText.enabled = true;
    }
    
    void NewScene() {
        continueText.enabled = false;
    }
    
}