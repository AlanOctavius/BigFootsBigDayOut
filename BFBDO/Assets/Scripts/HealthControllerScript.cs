using UnityEngine;
using System.Collections;

public class HealthControllerScript : MonoBehaviour {

	public ExitAreaScript exitAreaScript;
	public int MaxHP = 100;
	int HP;


	// Use this for initialization
	void Start () {
		HP = MaxHP;
	}
	
	// Update is called once per frame
	void Update () {

		if (HP > 0) {
			if(exitAreaScript.notendgame){
				exitAreaScript.EndGame(false);
			}
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

