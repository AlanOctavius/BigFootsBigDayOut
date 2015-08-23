using UnityEngine;
using System.Collections;

public class FameControllerScript : MonoBehaviour {

	public int MaxFP = 100;

	int FP;
	
	// Use this for initialization
	void Start () {
		FP = 0;
	}
	
	// Update is called once per frame
	void Update () {
		IncreaseFame(1);

		if (FP >= 100) {
			//Hes Famous JIM
		}
	}

	void IncreaseFame(int fame){
		FP += fame;
	}

	public float GetFame(){
		return (float) FP/MaxFP;
	}
}
