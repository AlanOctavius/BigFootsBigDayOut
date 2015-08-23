using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FameBarUI : MonoBehaviour {

	public HealthyFameFame FameController;

	Image image;

	// Use this for initialization
	void Start () {
		image = gameObject.GetComponent<Image> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		image.fillAmount = FameController.GetFame();
	}
}
