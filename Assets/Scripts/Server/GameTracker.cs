/* David Robideau
 * 3/1/2016
 * 
 * This script tracks game data and handles server updates - timers, ghost data, etc.
 *
 * Last update - 3/3/2016
 */

using UnityEngine;
using System.Collections;

public class GameTracker : MonoBehaviour {

	public NetworkView nv;

	//Current game state
	public bool isPlaying;
	public bool waitingForOpponent;
	public bool opponentIsWaiting;
	public UnityEngine.UI.Text opponentWaitingText;

	//Reference to player car object
	public GameObject playerCar;

	//Game timer
	public float gameTimer;
	public UnityEngine.UI.Text timerText;
	private float startTime;
	public float finalTime;

	void Awake() {
		nv = GetComponent<NetworkView> ();
	}

	//At start - neither player is waiting, movement is not enabled until timer starts
	void Start () {
		opponentIsWaiting = false;
		waitingForOpponent = false;
		playerCar.GetComponent<PlayerCarController> ().setMovementEnabled (false);
		StartCoroutine (countdown());
	}

	void Update () { 
		
		if (isPlaying) {
			updateTimer ();
		}

		//Debug only - replace with "finish" event later
		if (playerCar.GetComponent<PlayerCarController>().hasFinished && isPlaying) {
			stopTimer ();
			//For debug purposes - replace with player names later
			string opponentName;
			if (nv.isMine) {
				opponentName = "Player A";
			}
			else {
				opponentName = "Player B";
			}
			nv.RPC ("broadcastPlayerFinished", RPCMode.Others, opponentName); //broadcast to opponent
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
		timerText.text = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, ms);
		finalTime = timerTime;
	}

	//Set player car to designated starting position
	public void setStartingPosition() {
		//TODO
	}

	//Trigger countdown - start timer once countdown is completed
	public IEnumerator countdown() {
		timerText.text = "3...";
		yield return new WaitForSeconds (1);
		timerText.text = "2...";
		yield return new WaitForSeconds (1);
		timerText.text = "1...";
		yield return new WaitForSeconds (1);
		startTimer ();
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
	}
		
}