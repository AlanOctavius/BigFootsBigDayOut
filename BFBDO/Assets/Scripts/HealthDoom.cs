using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthDoom : MonoBehaviour {

	public HealthControllerScript HealthController;
	Image image;
	float HP;
	
	public Sprite Doom0;
	public Sprite Doom1;
	public Sprite Doom2;
	public Sprite Doom3;
	public Sprite Doom4;
	public Sprite Doom5;

	// Use this for initialization
	void Start () {
		image = gameObject.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		HP = HealthController.GetHealth ();
		if (HP < 100 && HP  > 90) {
			image.sprite = Doom0;
		} else if (HP < 90 && HP > 70) {
			image.sprite = Doom1;
		} else if (HP < 70 && HP > 50) {
			image.sprite = Doom2;
		} else if (HP < 50 && HP > 30) {
			image.sprite = Doom3;
		} else if (HP < 30 && HP > 10) {
			image.sprite = Doom4;
		} else if (HP < 10 ) {
			image.sprite = Doom5;
		}
	}
}
