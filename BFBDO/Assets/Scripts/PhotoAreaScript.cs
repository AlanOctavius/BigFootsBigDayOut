using UnityEngine;
using System.Collections;

public class PhotoAreaScript : MonoBehaviour {

	private PhotoScript photoScript;

	// Use this for initialization
	void Start () {
		photoScript = gameObject.GetComponentInParent<PhotoScript> ();
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			Transform photoTarget = other.gameObject.transform;
			
			photoScript.SetPhotoTarget(photoTarget);
		}
		
	}

}
