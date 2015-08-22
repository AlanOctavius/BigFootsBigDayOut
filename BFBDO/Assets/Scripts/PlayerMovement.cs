using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed;

	Rigidbody2D rigidbody2d;


	void Start(){

		rigidbody2d = GetComponent<Rigidbody2D> ();

	}


	// Fixed Update is called once per frame
	void FixedUpdate () {

			float horizontalMovement = Input.GetAxis ("Horizontal");
			float verticalMovement = Input.GetAxis ("Vertical");

			rigidbody2d.angularVelocity = 0;

			//prevent diagonal speed up
			Vector2 playerForce = new Vector3 (horizontalMovement, verticalMovement, 0);
			playerForce.Normalize ();

			rigidbody2d.velocity =  (playerForce * speed);

	}




};
