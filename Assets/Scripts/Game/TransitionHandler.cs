/* David Robideau
 * 3/22/2016
 * 
 * This script handles transitions between build mode and race mode.
 * (De)activates components as necessary, detects transition criteria, and handles data transfers.
 *
 * Last update - 3/22/2016
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TransitionHandler : MonoBehaviour {

	//Empties - hold objects exclusive to either build or race mode for easy deactivation
	public GameObject buildModeComponents;
	public GameObject raceModeComponents;

	//Network manager and data to handle initial connection
	public NetManager netManager;
	public GameObject connectionData;
	public bool playerConnected;

	//Player info
	private string playerNameA = "";
	private string playerNameB = "";

	//Track scanner and replicator
	public TrackScanner scanner;
	public TrackReplicator replicator;

	//Track data sender
	public SendData dataSender;

	//Game stat tracker
	public GameTracker gameTracker;

	//Canvas items
	public ModularChat chat;
	public Text buildTimer;

	//Build mode timer info
	public float buildTimeLimit = 10;
	private string defaultBuildTimerText;
	private float startTime;
	private bool buildTimerActive = false;
	private bool buildTimerComplete = false;

	//Track data to be sent between players
	private string trackData;

	void Awake() {
		//Retrieve network data, create connection
		//playerNameA = host player name
		//playerNameB = connecting player name
		//chat.setSenderIDs(playerNameA, playerNameB);

		//Transition to build mode, activate timer
		defaultBuildTimerText = buildTimer.text;
		buildTimerActive = true;
		StartCoroutine (buildMode ());
	}

	void Update() {

		//Update build mode timer
		if (buildTimerActive && playerConnected) {
			if (buildTimer.text == defaultBuildTimerText) {
				startTime = Time.time;
			}
			if (buildTimer.text == "Remaining: 00:00") {
				buildTimer.text = "Time's up!";
				buildTimerActive = false;
				buildTimerComplete = true;
			} else {
				updateBuildTimer ();
			}
		}
			
	}

	private IEnumerator buildMode() {
		//Wait for players to finish or time to run out
		while (!buildTimerComplete) {
			yield return new WaitForSeconds (1);
		}

		//Scan track, send data, clear scene
		dataSender.writeData();
		//trackData = result of RPC call during writeData()

		//Deactivate build mode components
		buildModeComponents.SetActive(false);

		//Transition to race mode
		StartCoroutine(raceMode());
		yield return null;
	}

	private IEnumerator raceMode() {
		//Activate race mode components

		//Relocate car to start point, set active

		//Replicate track
		replicator.replicateTrack(trackData);

		//Reset timer, start game tracker
		gameTracker.gameObject.SetActive(true);

		//Collapse chat
		chat.ChatUI.SetActive(false);

		//Wait for players to finish

		//Determine scores and send to final screen

		yield return null;
	}

	private void updateBuildTimer() {
		float timerTime = buildTimeLimit - (Time.time - startTime);
		int minutes = (int)timerTime / 60;
		int seconds = (int)timerTime % 60;
		buildTimer.text = string.Format ("Remaining: {0:00}:{1:00}", minutes, seconds);
	}
}
