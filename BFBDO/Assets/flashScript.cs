using UnityEngine;
using System.Collections;

public class flashScript : MonoBehaviour {

	private bool triggered;
	private SpriteRenderer spriteRenderer;
	private float alpha;
	public float decayTime = 0.5f;
	private float timeActive;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		triggered = false;
		spriteRenderer = this.GetComponent<SpriteRenderer> ();
	}


	// Update is called once per frame
	void Update () {
		if (triggered) {
			timeActive += Time.deltaTime;
			alpha = 1 - timeActive/decayTime;
			spriteRenderer.color = new Color(1.0f,1.0f,1.0f,alpha);
			if (timeActive >= decayTime){
				triggered = false;
				timeActive = 0;
				gameObject.SetActive (false);
			}
		}
	}

	public void ActivateSelf(){
		if (!triggered) {
			triggered = true;
			gameObject.SetActive (true);
		}
	}
}
