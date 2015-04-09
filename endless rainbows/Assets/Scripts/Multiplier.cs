using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Multiplier : MonoBehaviour {
    
    public float fadeRate;
    public float growRate;
    Image image;
    public Text marker;
    
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        Hide();
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale *= growRate;
	}
    
    void Hide() {
        enabled = false;
        image.canvasRenderer.SetAlpha(0);
        marker.enabled = false;
    }
    
    public void Show(Sprite sprite, string text) {
        transform.localScale = Vector3.one;
        image.canvasRenderer.SetAlpha(1);
        image.sprite = sprite;
        enabled = true;
        image.CrossFadeAlpha(0, fadeRate, false);
        marker.canvasRenderer.SetAlpha(1);
        marker.enabled = true;
        marker.text = text;
        marker.CrossFadeAlpha(0, fadeRate, false);
        CancelInvoke("Hide");
        Invoke("Hide", fadeRate);
    }
    
}
