using UnityEngine;
using System.Collections;

public class FameControllerScript : MonoBehaviour {

	public ExitAreaScript exitAreaScript;
	public int MaxFP = 100;

	int FP;
	
	// Use this for initialization
	void Start () {
		FP = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//IncreaseFame(1);

		if (FP >= 100) {
			if(exitAreaScript.notendgame){
				exitAreaScript.EndGame(true);
			}
		}
	}

	public void IncreaseFame(int fame){
		FP += fame;
	}

	public float GetFame(){
		return (float) FP/MaxFP;
	}
}
