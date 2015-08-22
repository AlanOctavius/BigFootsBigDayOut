using UnityEngine;
using System.Collections;

public class FocusScript : MonoBehaviour {

	private float distance;
	public float speed = 1;
	private bool frozen = false;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}

	public void SetActive(bool activeState){
		gameObject.SetActive (activeState);
		frozen = false;
	}

	public void SetTarget(float distance){
		this.distance = distance;
	}
	public void Freeze(){
		//Prevent focus from moving during last 0.5 Secs
		frozen = true;
	}

	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf){

		}
		if (!frozen){
			transform.localPosition = Vector2.Lerp(transform.localPosition,
			                                       new Vector3(0,distance,0),
			                                       speed * Time.deltaTime);
		}
	}
}
