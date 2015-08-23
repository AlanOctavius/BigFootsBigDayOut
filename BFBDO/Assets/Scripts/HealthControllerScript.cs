using UnityEngine;
using System.Collections;

public class HealthControllerScript : MonoBehaviour {

	public int MaxHP = 100;
	int HP;


	// Use this for initialization
	void Start () {
		HP = MaxHP;
	}
	
	// Update is called once per frame
	void Update () {

		//SuitDamage (1);

		if (HP > 0) {
			//The Suit is fully degraded!
		}
	
	}

	public void SuitDamage(int damage) {
		if (HP > 0) {
			HP -= damage;
		}
	}

	public float GetHealth(){
		return (float) HP/MaxHP*100;
	}
}

