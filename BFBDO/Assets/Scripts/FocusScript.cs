using UnityEngine;
using System.Collections;

public class FocusScript : MonoBehaviour {

	private float distance;
	public float speed = 1;
	private bool frozen = false;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(true);
	}

	public void Activate(bool activeState){
		gameObject.SetActive(activeState);
		frozen = false;
	}

	public void SetTarget(float distance){
		this.distance = distance;
	}
	public void Freeze(){
		//Prevent focus from moving during last 0.5 Secs
		frozen = true;
	}

	public float PlayerDistance(Vector2 playerPosition){
		//finds the distance between the focus logo and the player
		return Vector2.Distance (playerPosition, (Vector2) transform.position);
	}

	// Update is called once per frame
	void Update () {

		if (!frozen){
			transform.localPosition = Vector2.Lerp(transform.localPosition,
			                                       new Vector3(-distance,0,0), // Works here, most likeyl correcting an error else where don't have time to find
			                                       speed * Time.deltaTime);
		}
	}
}
