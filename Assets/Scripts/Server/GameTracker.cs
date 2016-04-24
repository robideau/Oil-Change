/* David Robideau
 * 3/1/2016
 * 
 * This script tracks game data and handles server updates - timers, ghost data, etc.
 *
 * Last update - 4/7/2016
 */

//Warnings to ignore - DEV ONLY, REMOVE FOR FINAL BUILDS
#pragma warning disable 0618 //deprecated network view

using UnityEngine;
using System.Collections;

public class GameTracker : MonoBehaviour {

	public NetworkView nv;

	//Current game state
	public bool isPlaying;
	public bool waitingForOpponent;
	public bool opponentIsWaiting;
	public UnityEngine.UI.Text opponentWaitingText;
	public accountInfo accInfo;

	//Reference to player car object
	public GameObject playerCar;

	//Game timer
	public float gameTimer;
	public UnityEngine.UI.Text timerText;
	private float startTime;
	public float finalTime;

	public TransitionHandler transitionHandler;

	void Awake() {
		nv = GetComponent<NetworkView> ();
	}

	//At start - neither player is waiting, movement is not enabled until timer starts
	void Start () {
		opponentIsWaiting = false;
		waitingForOpponent = false;
		playerCar.GetComponent<PlayerCarController> ().setMovementEnabled (false);
		timerText.gameObject.SetActive (true);
		StartCoroutine (countdown());
		transitionHandler = GameObject.Find ("GameController").GetComponent<TransitionHandler> ();
		accInfo = GameObject.Find ("Script manager").GetComponent<accountInfo> ();
	}

	void Update () { 
		
		if (isPlaying) {
			updateTimer ();
		}
			
		if (playerCar.GetComponent<PlayerCarController>().hasFinished && isPlaying) {
			stopTimer ();
			//For debug purposes - replace with player names later
			/*
			string opponentName;
			if (nv.isMine) {
				opponentName = "Player A";
			}
			else {
				opponentName = "Player B";
			}
			*/
			nv.RPC ("broadcastPlayerFinished", RPCMode.Others, accInfo.getName()); //broadcast to opponent
		}
	}

	//Start timer, enable player movement
	public void startTimer() {
		startTime = Time.time;
		isPlaying = true;
		playerCar.GetComponent<PlayerCarController> ().setMovementEnabled (true);
	}

	//Update elapsed time and timer text
	private void updateTimer() {
		float timerTime = Time.time - startTime;
		int minutes = (int)timerTime / 60;
		int seconds = (int)timerTime % 60;
		int ms = (int)(timerTime * 100) % 100;
		timerText.text = string.Format ("{0:00}:{1:00}", minutes, seconds);
		finalTime = timerTime;
	}

	//Trigger countdown - start timer once countdown is completed
	public IEnumerator countdown() {
		playerCar.GetComponent<PlayerCarController> ().enabled = false;
		playerCar.GetComponent<PlayerCarController> ().setMovementEnabled (false);
		timerText.text = "3...";
		yield return new WaitForSeconds (1);
		timerText.text = "2...";
		yield return new WaitForSeconds (1);
		timerText.text = "1...";
		yield return new WaitForSeconds (1);
		startTimer ();
		playerCar.GetComponent<PlayerCarController> ().setMovementEnabled (false);
		playerCar.GetComponent<PlayerCarController> ().enabled = true;
	}

	public void stopTimer() {
		updateTimer ();
		isPlaying = false;
		waitingForOpponent = true;
	}

	//RPC to be used when a player has crossed the finish line
	[RPC] void broadcastPlayerFinished(string playerName) {
		opponentWaitingText.gameObject.SetActive (true);
		opponentWaitingText.text = playerName + " is finished!";
		opponentIsWaiting = true;
		transitionHandler.opponentFinished = true;
	}
		
}