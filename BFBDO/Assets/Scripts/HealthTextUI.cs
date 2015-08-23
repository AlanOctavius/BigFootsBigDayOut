using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthTextUI : MonoBehaviour {

	public HealthControllerScript HealthController;
	Text text;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Suit Integrity : " + HealthController.GetHealth()+"%";
	}
}
