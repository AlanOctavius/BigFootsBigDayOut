using UnityEngine;
using System.Collections;

public class HealthyFameFame : MonoBehaviour {
	
	public int MaxHP = 100;
	public int MaxFP = 100;
	public int MaxCP = 100;
	int HP;
	int FP;
	int CP;

	void Start() {
		HP = MaxHP;
		FP = 0;
	}

	void Update (){

		SuitDamage(1);
		IncreaseFame(1);


		if (HP <= 0) {
			//HES DEAD JIM
		}
		if (FP >= 100) {
			//Hes Famous JIM
		}
		if (FP >= 100) {
			//Hes Infamous JIM
		}
	}

	void SuitDamage(int damage) {
		HP -= damage;
	}

	void IncreaseFame(int fame){
		FP += fame;
	}

	void IncreaseConsp(int consp){
		CP += consp;
	}

	public float GetFame(){
		return (float) FP/MaxFP;
	}


}