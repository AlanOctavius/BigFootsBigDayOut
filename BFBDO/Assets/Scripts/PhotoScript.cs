using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoScript : MonoBehaviour {

	private FocusScript focusScript;
	private flashScript flashScript;
	private GameObject photoArea;
	private Transform photoTarget;


	private float targetAngle;
	public enum SearchStatus{NotSearching, Searching, ChaseingPlayer};

	public SearchStatus searchStatus = SearchStatus.NotSearching;
	public float averageSearchTimeRange = 5;
	public float averageSearchTimeMin = 5;
	public float maxChaseTime = 5;
	public float turnAngleShow;
	public float rotationSpeed = 3;
	public float movementSpeed = 2;
	public float smallDistance = 2;
	public float currentTime;
	public float currentSearchTimeMax;
	public float oscilationsPerSecond = 1;


	private List<Transform> waypoints;
	private int currentWaypointIndex;

	private Quaternion initialRotation;

	// Use this for initialization

	void Start () {
	
		flashScript = transform.FindChild ("flash").gameObject.GetComponent<flashScript>();
		photoArea = transform.FindChild ("photoArea").gameObject;
		focusScript = photoArea.transform.FindChild ("focus").gameObject.GetComponent<FocusScript>();
		Transform waypointHolder = GameObject.FindGameObjectWithTag ("waypoint").transform;

		waypoints = new List<Transform> ();
		foreach (Transform waypoint in waypointHolder){
			waypoints.Add(waypoint);
		}
		//find nearest waypoint for inital placement
		currentWaypointIndex = 0;
		float currentSmallestDistance = Vector2.Distance((Vector2)transform.position, (Vector2) waypoints[currentWaypointIndex].position);

		foreach (Transform waypoint in waypoints){

			if (waypoint == waypoints[currentWaypointIndex]){continue;};

			float distance = Vector2.Distance((Vector2)transform.position, (Vector2) waypoint.position);

			if (distance > currentSmallestDistance){
				continue;
			}else{
				currentSmallestDistance = distance;
				currentWaypointIndex = waypoints.IndexOf(waypoint);

			}

		}


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
			focusScript.Activate(true);
		}
		if (newStatus != SearchStatus.ChaseingPlayer) {
			focusScript.Activate(false);
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
		float distance = Vector2.Distance((Vector2)transform.position,
		                                  (Vector2) waypoints[currentWaypointIndex].position);

		if (distance <= smallDistance) {
			//set random search time
			float random = Random.value;
			currentSearchTimeMax = averageSearchTimeRange * random + averageSearchTimeMin;

			//get waypoint target
			Vector3 targetPos = waypoints[currentWaypointIndex].FindChild("Target").position;


			//rotate camera area to waypoint target
			Vector3 vectorToTarget = transform.position - targetPos;
			targetAngle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg ;
			photoArea.transform.rotation = Quaternion.AngleAxis (targetAngle, Vector3.forward);

			changeStatusTo(SearchStatus.Searching);
			if (currentWaypointIndex + 1 >= waypoints.Count){
				currentWaypointIndex = 0;
				waypoints.Reverse ();
			}else{
				currentWaypointIndex++;
			}
			return;
		}
		
		transform.position = Vector3.MoveTowards (transform.position,
		                                          waypoints [currentWaypointIndex].position,
		                                          movementSpeed * Time.deltaTime);

	}

	void SearchUpdate(){
		float deltaT = Time.deltaTime;
		currentTime += deltaT;
		
		if (currentTime > currentSearchTimeMax) {
			//search has taken too long
			changeStatusTo (SearchStatus.NotSearching);
		}

		
		float turnAngle = Mathf.Sin (currentTime * oscilationsPerSecond) * 45;

		photoArea.transform.rotation =  Quaternion.AngleAxis (targetAngle + turnAngle, Vector3.forward);
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

			Vector3 vectorToTarget = transform.position - photoTarget.position ;
			float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg ;

			//create the rotation we need to be in to look at the target
			//Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.down);
			
			//rotate us over time according to speed until we are in the required rotation
			photoArea.transform.rotation = Quaternion.Slerp (photoArea.transform.rotation,
			                                       Quaternion.AngleAxis (angle, Vector3.forward),
			                                       rotationSpeed * Time.deltaTime);

			//focusScript.gameObject.transform.RotateAround(transform.position,
			//                                              Vector3.forward,
			//                                              angle);
			focusScript.SetTarget (((Vector2)vectorToTarget).magnitude);
		}
	}
}
