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
		if (HP < 100 && HP  > 80) {
			image.sprite = Doom0;
		} else if (HP <79 && HP > 60) {
			image.sprite = Doom1;
		} else if (HP <59 && HP > 40) {
			image.sprite = Doom2;
		} else if (HP <39 && HP > 20) {
			image.sprite = Doom3;
		} else if (HP <19 && HP > 2) {
			image.sprite = Doom4;
		} else if (HP < 1) {
			image.sprite = Doom5;
		}
	}
}
