/* David Robideau
 * 3/22/2016
 * 
 * This script handles transitions between build mode and race mode.
 * (De)activates components as necessary, detects transition criteria, and handles data transfers.
 *
 * Last update - 4/03/2016
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
	private NetworkView nv;
	public bool playerConnected;
	public playableGame gameData;

	//Build mode status indicators
	public BuildTracker buildTracker;
	public Text buildStatus;
	public Text buildCountText;
	public Button submitButton;

	//Race mode status indicators
	public Text raceTimer;
	public Text raceStatus;

	//Player info
	//private string playerNameA = "";
	//private string playerNameB = "";

	//Track scanner and replicator
	public TrackScanner scanner;
	public TrackReplicator replicator;

	//Track data sender
	public SendData dataSender;

	//Game stat tracker
	public GameTracker gameTracker;
	public bool buildModeActive = false;
	public bool raceModeActive = false;

	//Canvas items
	public ModularChat chat;
	public Text buildTimer;
	public Text submissionStatus;
	public QuitButton quitButton;

	//Build mode timer info
	public float buildTimeLimit = 10;
	public int buildCountLimit;
	private string defaultBuildTimerText;
	private float startTime;
	private bool buildTimerActive = false;
	private bool buildTimerComplete = false;

	//Track data to be sent between players
	private string trackData = "Data not received.";

	void Awake() {
		//Retrieve network data, create connection
		gameData = GameObject.Find("SessionData").GetComponent<playableGame>();
		buildTimeLimit = gameData.getBuildTime ();
		buildCountLimit = gameData.getBuildLimit ();
		netManager.gameName = gameData.getName ();
		StartCoroutine(netManager.joinSpecifiedServer (gameData.getName (), gameData.checkHost ()));
		nv = netManager.GetComponent<NetworkView>();

		//Set player colors
		int randomColor = Random.Range(0, 8);
		gameTracker.playerCar.transform.FindChild ("DefaultCar").FindChild ("Frame").gameObject.GetComponent<MeshRenderer> ().material = gameTracker.playerCar.transform.FindChild ("DefaultCar").GetComponent<MeshRenderer> ().materials[randomColor];

		//Transition to build mode, activate timer
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
				submitButton.gameObject.SetActive (false);
				//Eject to main menu, display message stating that one or more players did not finish
			} else {
				updateBuildTimer ();
			}
		}

		if (chat.chatInput.isFocused) {
			chat.enableInput ();
		} else {
			chat.disableInput ();
		}
			
	}

	private IEnumerator testScanner() {
		StartCoroutine (scanner.QuickProcessTrack ());
		yield return new WaitForSeconds (2);
		scanner.cleanObjectNames ();
		yield return new WaitForSeconds (1);
		print (scanner.getScannedLevelData ());
	}

	private IEnumerator buildMode() {
		buildStatus.gameObject.SetActive (false);
		submissionStatus.gameObject.SetActive (false);
		buildTracker.toggleBuildMenu (false);
		buildCountText.gameObject.SetActive (false);

		//Wait for connection
		while (!playerConnected) {
			yield return new WaitForSeconds (1);
		}

		buildTracker.toggleBuildMenu (true);
		buildTimer.gameObject.SetActive(true);
		buildStatus.gameObject.SetActive (true);
		buildModeComponents.SetActive(true);
		submissionStatus.gameObject.SetActive (true);
		buildCountText.gameObject.SetActive (true);

		//Transition, start timer
		defaultBuildTimerText = buildTimer.text;
		buildTimerActive = true;
		buildModeActive = true;

		//Wait for players to finish or time to run out
		while (!buildTimerComplete && !(buildStatus.text == "Opponent has finished building." && buildTracker.submitConditionsCheck())) {
			yield return new WaitForSeconds (1);
		}

		//Scan track, send data, clear scene
		scanner.deleteOnScan = true;
		StartCoroutine(dataSender.writeData());
		yield return new WaitForSeconds (3);

		trackData = dataSender.getReceivedData ();

		//Deactivate build mode components
		while (GameObject.FindGameObjectWithTag ("BuildObject") != null) {
			Destroy(GameObject.FindGameObjectWithTag("BuildObject"));
			yield return null;
		}
		while (GameObject.FindGameObjectWithTag ("ParentedBuildObject") != null) {
			Destroy(GameObject.FindGameObjectWithTag("ParentedBuildObject"));
			yield return null;
		}
		while (GameObject.FindGameObjectWithTag ("Finish") != null) {
			Destroy(GameObject.FindGameObjectWithTag("Finish"));
			yield return null;
		}
		while (GameObject.FindGameObjectWithTag ("Start") != null) {
			Destroy(GameObject.FindGameObjectWithTag("Start"));
			yield return null;
		}
		buildTimer.gameObject.SetActive(false);
		buildStatus.gameObject.SetActive (false);
		buildModeComponents.SetActive(false);
		submissionStatus.gameObject.SetActive (false);
		buildCountText.gameObject.SetActive (false);

		//Transition to race mode
		buildModeActive = false;
		raceModeActive = true;
		StartCoroutine(raceMode());
		yield return null;
	}

	private IEnumerator raceMode() {
		//Replicate track
		replicator.replicateTrack(trackData);

		//Relocate car to start point, set active
		gameTracker.playerCar.SetActive(true);
		gameTracker.playerCar.GetComponent<PlayerCarController> ().relocateToStart ();

		//Reset timer, start game tracker
		gameTracker.gameObject.SetActive(true);

		//Collapse chat
		chat.ChatUI.SetActive(false);

		//Wait for players to finish
		while (!gameTracker.playerCar.GetComponent<PlayerCarController> ().hasFinished &&
		       !raceStatus.gameObject.activeSelf) {
			yield return new WaitForSeconds (1);
		}

		//Determine scores and send to final screen

		yield return null;
	}

	//To be executed if 1 or more players did not successfully submit before time up
	private IEnumerator timeOut() {
		yield return new WaitForSeconds (5);
		quitButton.OnClick ();
	}

	private void updateBuildTimer() {
		float timerTime = buildTimeLimit - (Time.time - startTime);
		int minutes = (int)timerTime / 60;
		int seconds = (int)timerTime % 60;
		buildTimer.text = string.Format ("Remaining: {0:00}:{1:00}", minutes, seconds);
	}
}
