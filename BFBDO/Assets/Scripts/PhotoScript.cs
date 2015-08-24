using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoScript : MonoBehaviour {

	private FocusScript focusScript;
	private flashScript flashScript;
	private PhotoAreaScript photoAreaScript;
	private GameObject photoArea;
	private Transform photoTarget;
	private HealthControllerScript healthControllerScript;
	private FameControllerScript fameControllerScript;

	private float targetAngle;
	public enum SearchStatus{NotSearching, Searching, ChaseingPlayer};
	public enum Direction{Up,Down,Left,Right};

	public Direction currentDirection = Direction.Up;
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
	public float focusDamageRadius = 1;
	public int focusDamage = 20;
	public int famePerPic = 20;

	public float debugAngel;
	public float debugAngel2;

	public Animator anim;

	private bool facingLeft = false;
	private List<Transform> waypoints;
	private int currentWaypointIndex;

	private bool playerFlashed;

	private Quaternion initialRotation;
	private Transform oldTransform;
	private Vector3 oldPosition;

	// Use this for initialization

	void Start () {
	
		healthControllerScript = GameObject.Find("HealthController").gameObject.GetComponent<HealthControllerScript> ();
		fameControllerScript = GameObject.Find ("FameController").gameObject.GetComponent<FameControllerScript> ();
		flashScript = transform.FindChild ("flash").gameObject.GetComponent<flashScript>();
		photoArea = transform.FindChild ("photoArea").gameObject;
		photoAreaScript = photoArea.GetComponent<PhotoAreaScript>();
		focusScript = photoArea.transform.FindChild ("focus").gameObject.GetComponent<FocusScript>();
		Transform waypointHolder = GameObject.FindGameObjectWithTag ("waypoint").transform;

		anim = GetComponent<Animator>();

		oldTransform = transform;
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
		oldTransform = transform;
		oldPosition = transform.position;
		if (searchStatus == SearchStatus.NotSearching) {
			NotSearchingUpdate();
		} else if (searchStatus == SearchStatus.Searching) {
			SearchUpdate();
		} else if (searchStatus == SearchStatus.ChaseingPlayer){
			ChaseUpdate();
		}

		///TODO:
		/// Pick direction and set trigger
		/// 
		float distance = Vector2.Distance ((Vector2)transform.position, (Vector2)oldPosition);
		anim.SetFloat ("Speed", distance);
		Vector3 vectorToTarget = oldPosition - transform.position ;
		float angle =  Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
		angle = nfmod (angle, 360f);
		debugAngel = distance;
		debugAngel2 = angle;
		if (angle > 315 || angle <= 45 && currentDirection != Direction.Up) {
			//up
			currentDirection = Direction.Up;
			anim.SetTrigger("GoingUp");
		} else if (angle > 135 && angle <= 225 && currentDirection != Direction.Down) {
			//down
			currentDirection = Direction.Down;
			anim.SetTrigger("GoingDown");
		} else {
			//sideways
			if (angle > 45 && angle <= 135 && currentDirection != Direction.Right){
				// right
				currentDirection = Direction.Right;
				anim.SetTrigger("GoingSideways");
				if(facingLeft){
					Flip();
				}
			}else if (angle > 225 && angle <= 315 && currentDirection != Direction.Left){
				//left
				currentDirection = Direction.Left;
				anim.SetTrigger("GoingSideways");
				if(!facingLeft){
					Flip();
				}
			}

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
			Transform target = waypoints[currentWaypointIndex].FindChild("Target");


			//rotate camera area to waypoint target
			targetAngle = getAngle(transform, target);
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
			//Here!
			if (photoAreaScript.hasPlayer && playerFlashed == false){
				//Debug.Log("Photo has Sassy Ass");
				float playerDistance = focusScript.PlayerDistance(photoTarget.position);
				if (playerDistance < focusDamage){
					healthControllerScript.SuitDamage(focusDamage);
				}else if ( playerDistance > focusDamageRadius){
					fameControllerScript.IncreaseFame(famePerPic);
				}
				playerFlashed = true;
			}
			//Here!
			if (currentTime > maxChaseTime + flashScript.decayTime) {
				changeStatusTo (SearchStatus.NotSearching);
				playerFlashed = false;

			}
		} else {

			float angle = getAngle(transform, photoTarget);

			//create the rotation we need to be in to look at the target
			//Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.down);
			
			//rotate us over time according to speed until we are in the required rotation
			photoArea.transform.rotation = Quaternion.Slerp (photoArea.transform.rotation,
			                                       Quaternion.AngleAxis (angle, Vector3.forward),
			                                       rotationSpeed * Time.deltaTime);

			//focusScript.gameObject.transform.RotateAround(transform.position,
			//                                              Vector3.forward,
			//                                              angle);
			Vector3 vectorToTarget = transform.position - photoTarget.position ;
			focusScript.SetTarget (((Vector2)vectorToTarget).magnitude);
		}
	}

	private float getAngle(Transform start, Transform end){
		Vector3 vectorToTarget = start.position - end.position ;
		return  Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg ;
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing
		facingLeft = !facingLeft;
		
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	float nfmod(float a,float b){
		return a - b * Mathf.Floor(a / b);
	}
}
