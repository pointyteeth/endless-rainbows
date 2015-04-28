using UnityEngine;
using UnityEngine.UI;
using System;
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
    public Text continueText;
    string control = "spacebar";
    public Image startScreen;
    public Text introText;
    public Text startText;
    public ParticleSystem startGlitter;
    public int quitCount;
    int quitCounter;
    DateTime resetTimer = DateTime.Now;
    DateTime aliveTimer = DateTime.Now;
    
    void Start() {
    
        quitCounter = quitCount;
    
        #if UNITY_WEBPLAYER || UNITY_EDITOR
            control = "SPACEBAR";
        #endif
    
        #if UNITY_STANDALONE
            control = "press BUTTON";
        #endif
        
        #if UNITY_ANDROID
            control = "TAP";
        #endif
        
        startText.text = control + " to start";
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
                {"medal", new[] {"olympic medal", "Gold medal in tolerating the gays!"}},
                {"botox", new[] {"botox", "~*¤{ p r e t t y }¤*~"}}
            };
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        GameEventManager.UpdatePoints += UpdatePoints;
        GameEventManager.StartItem += StartItem;
        GameEventManager.EndScene += EndScene;
        GameEventManager.NewScene += NewScene;
        EnableStartScreen();
	}

	void Update() {
        if(Input.GetButtonDown("Jump")) {
            resetTimer = DateTime.Now;
            if(!GameEventManager.game){
                GameEventManager.TriggerGameStart();
            }
            else if(!GameEventManager.scene && !GameEventManager.firstScene){
                if(quitCounter == 1) {
                    GameEventManager.TriggerGameOver();
                } else {
                    quitCounter--;
                    continueText.text = control + " " + quitCounter + " times to quit";
                    if(quitCounter == 1) {
                        continueText.text = control + " " + quitCounter + " time to quit";
                    }
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if((DateTime.Now - aliveTimer).Minutes >= 30) {
            aliveTimer = DateTime.Now;
            Debug.Log("Alive");
        }
        if(GameEventManager.game && (DateTime.Now - resetTimer).Minutes >= 1) {
            resetTimer = DateTime.Now;
            GameEventManager.TriggerGameOver();
        }
	}
    
    void GameStart() {
        DisableStartScreen();
        continueText.text = control + " to jump";
        continueText.enabled = true;
        itemTitle.enabled = false;
        itemDescription.enabled = false;
    }
    
    void GameOver() {
        EnableStartScreen();
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
        continueText.text = control + " 3 times to quit";
        continueText.enabled = true;
        quitCounter = quitCount;
    }
    
    void NewScene() {
        continueText.enabled = false;
    }
    
    void EnableStartScreen() {
        startScreen.enabled = true;
        introText.enabled = true;
        startText.enabled = true;
        startGlitter.Play();
        Camera.main.cullingMask = 1 << LayerMask.NameToLayer("UI");
    }
    
    void DisableStartScreen() {
        startScreen.enabled = false;
        introText.enabled = false;
        startText.enabled = false;
        startGlitter.Stop();
        Camera.main.cullingMask = -1;
    }
    
}