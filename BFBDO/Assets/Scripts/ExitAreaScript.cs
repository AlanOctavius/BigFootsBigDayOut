using UnityEngine;
using System.Collections;

public class ExitAreaScript : MonoBehaviour {

	public bool notendgame = true;

	void OnTriggerEnter2D( Collider2D otherObj){
		
	}

	public void EndGame(bool fame){
		notendgame = true;
		if (fame) {
			//the player.
		} else {
			//the player broke his suit
		}

	}
}
