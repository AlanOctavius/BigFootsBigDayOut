using UnityEngine;
using System.Collections;

public class PhotoScript : MonoBehaviour {

	private FocusScript focusScript;
	private flashScript flashScript;
	private GameObject photoArea;
	private Transform photoTarget;



	public enum SearchStatus{NotSearching, Searching, ChaseingPlayer};

	public SearchStatus searchStatus = SearchStatus.NotSearching;
	public float averageSearchTimeRange = 5;
	public float averageSearchTimeMin = 5;
	public float maxChaseTime = 5;
	public float turnAngleShow;
	public float rotationSpeed = 3;
	public float currentTime;
	public float currentSearchTimeMax;

	public float direction;
	public Vector3 Fromposition;
	public Vector3 Topositoion;

	private Quaternion initialRotation;

	// Use this for initialization

	void Start () {
	
		focusScript = transform.FindChild ("focus").gameObject.GetComponent<FocusScript>();
		flashScript = transform.FindChild ("flash").gameObject.GetComponent<flashScript>();
		photoArea = transform.FindChild ("photoArea").gameObject;


		currentTime = 0;
		Random.seed = 42;
	}

	private void changeStatusTo( SearchStatus newStatus){
		searchStatus = newStatus;
		currentTime = 0;
		if (newStatus == SearchStatus.Searching) {
			initialRotation = transform.localRotation;
			photoArea.SetActive(true);
			float random = Random.value;
			currentSearchTimeMax = averageSearchTimeRange * random + averageSearchTimeMin;
		}else if (newStatus == SearchStatus.NotSearching) {
			photoArea.SetActive(false);
		}else if (newStatus == SearchStatus.ChaseingPlayer) {
			focusScript.SetActive(true);
		}
		if (newStatus != SearchStatus.ChaseingPlayer) {
			focusScript.SetActive(false);
		}
	}

	public void SetPhotoTarget(Transform target){
		if (searchStatus == SearchStatus.Searching) {
			changeStatusTo(SearchStatus.ChaseingPlayer);
			photoTarget = target;
		}

	}

	// Update is called once per frame
	void Update () {
		if (searchStatus == SearchStatus.NotSearching) {
			NotSearchingUpdate();
		} else if (searchStatus == SearchStatus.Searching) {
			SearchUpdate();
		} else if (searchStatus == SearchStatus.ChaseingPlayer){
			ChaseUpdate();
		}
	}

	void NotSearchingUpdate(){
		// Should put some movement before taking picture
		float random = Random.value;
		currentSearchTimeMax = averageSearchTimeRange * random + averageSearchTimeMin;
		
		changeStatusTo(SearchStatus.Searching);
	}

	void SearchUpdate(){
		float deltaT = Time.deltaTime;
		currentTime += deltaT;
		
		if (currentTime > currentSearchTimeMax) {
			//search has taken too long
			changeStatusTo (SearchStatus.NotSearching);
		}
		
		float normalisedTime = currentTime * 1;
		
		float turnAngle = Mathf.Sin (normalisedTime) * 45;
		
		turnAngleShow = turnAngle;
		
		transform.rotation = Quaternion.AngleAxis (turnAngle, Vector3.forward);
	}

	void ChaseUpdate(){
		float deltaT = Time.deltaTime;
		currentTime += deltaT;
		if (currentTime > maxChaseTime) {
			// Pause for PauseTime before flashing with the image.
			focusScript.Freeze ();
			flashScript.ActivateSelf ();
			if (currentTime > maxChaseTime + flashScript.decayTime) {
				changeStatusTo (SearchStatus.NotSearching);
			}
		} else {
			Fromposition = transform.position;
			Topositoion = photoTarget.position;
			
			Vector3 vectorToTarget = photoTarget.position - transform.position;
			float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
			
			//create the rotation we need to be in to look at the target
			//Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.down);
			
			//rotate us over time according to speed until we are in the required rotation
			transform.rotation = Quaternion.Slerp (transform.rotation,
			                                       Quaternion.AngleAxis (angle, Vector3.forward),
			                                       rotationSpeed * Time.deltaTime);
			
			focusScript.SetTarget (((Vector2)vectorToTarget).magnitude);
		}
	}
}
