/* David Robideau
 * 3/22/2016
 * 
 * This script handles transitions between build mode and race mode.
 * (De)activates components as necessary, detects transition criteria, and handles data transfers.
 *
 * Last update - 4/9/2016
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TransitionHandler : MonoBehaviour {

	//Empties - hold objects exclusive to either build or race mode for easy deactivation
	public GameObject buildModeComponents;
	public GameObject raceModeComponents;
	public GameObject skyboxPool;
	public GameObject worldEnd;

	//Cameras
	public Camera mainBuildCamera, mainRaceCamera, sideRaceCamera, frontRaceCamera;

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
	public accountInfo accInfo;

	//Track scanner and replicator
	public TrackScanner scanner;
	public TrackReplicator replicator;

	//Track data sender
	public SendData dataSender;

	//Game stat tracker
	public GameTracker gameTracker;
	public bool buildModeActive = false;
	public bool raceModeActive = false;
	public ScoreKeeper scoreKeeper;
	public bool opponentFinished = false;

	//Canvas items
	public ModularChat chat;
	public Text buildTimer;
	public Text submissionStatus;
	public QuitButton quitButton;
	public GameObject endGameCanvas;
	public Text raceStatusText;
	public ControlsButton controlButton;

	//Results screen items
	public Text bTitle;
	public Text bTimeTitle;
	public Text bTime;
	public Text aTestTimeTitle;
	public Text aTestTime;
	public Text bColumnDiff;

	public Text aTitle;
	public Text aTimeTitle;
	public Text aTime;
	public Text bTestTimeTitle;
	public Text bTestTime;
	public Text aColumnDiff;

	public Text winner;

	//Build mode timer info
	public float buildTimeLimit = 10;
	public int buildCountLimit;
	private string defaultBuildTimerText;
	private bool submittingWait = false;
	private float startTime;
	public bool buildTimerActive = false;
	public bool buildTimerComplete = false;
	public bool raceTimerActive = false;

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

		//Set skybox
		int randomSky = Random.Range(0, 4);
		RenderSettings.skybox = skyboxPool.GetComponent<MeshRenderer> ().materials [randomSky];
		print ("Random: " + randomSky);
		print (skyboxPool.GetComponent<MeshRenderer> ().materials [randomSky].name);
			
		mainBuildCamera.GetComponent<Skybox>().material = skyboxPool.GetComponent<MeshRenderer> ().materials [randomSky];
		mainRaceCamera.GetComponent<Skybox>().material = skyboxPool.GetComponent<MeshRenderer> ().materials [randomSky];
		sideRaceCamera.GetComponent<Skybox>().material = skyboxPool.GetComponent<MeshRenderer> ().materials [randomSky];
		frontRaceCamera.GetComponent<Skybox>().material = skyboxPool.GetComponent<MeshRenderer> ().materials [randomSky];
		worldEnd.GetComponent<MeshRenderer> ().material = worldEnd.transform.FindChild ("MaterialPool").GetComponent<MeshRenderer> ().materials [randomSky];
		accInfo = GameObject.Find ("Script manager").GetComponent<accountInfo> ();
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
				if(submissionStatus.text == "Submission accepted!")
				{
					buildTimerComplete = true;
					buildTimerActive = false;
					StartCoroutine(submissionWait());
				}
				else if(submissionStatus.text == "Submitting...")
				{
					submitButton.gameObject.SetActive (false);
					submittingWait = true;

					buildTimer.text = "Waiting for submission...";
					buildTimerActive = false;
				}
				else
				{
					buildTimer.text = "Time's up!";
					buildTimerActive = false;
					buildTimerComplete = true;
					submitButton.gameObject.SetActive (false);
					StartCoroutine(timeOut());
				}
			} else {
				updateBuildTimer ();
			}
		}

		if(submittingWait)
		{
			if(submissionStatus.text != "Submitting...")
			{
				buildTimerActive = false;
				buildTimerComplete = true;
				if(submissionStatus.text != "Submission accepted!" )
					StartCoroutine(timeOut());
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
		chat.ChatUI.SetActive (false);
		controlButton.forceCloseBuildControls ();
		StartCoroutine(raceMode());
		yield return null;
	}

	private IEnumerator raceMode() {
		//Replicate track
		replicator.replicateTrack(trackData);

		//Relocate car to start point, set active
		gameTracker.playerCar.SetActive(true);
		gameTracker.playerCar.GetComponent<PlayerCarController> ().relocateToStart ();
		gameTracker.playerCar.GetComponent<PlayerCarController> ().hasFinished = false;

		//Reset timer, start game tracker
		gameTracker.gameObject.SetActive(true);
		raceTimerActive = true;

		//Collapse chat
		chat.ChatUI.SetActive(false);

		//Wait for players to finish
		while (!gameTracker.playerCar.GetComponent<PlayerCarController> ().hasFinished &&
			!raceStatus.gameObject.activeSelf) {
			yield return new WaitForSeconds (1);
		}
		while (!opponentFinished) {
			yield return new WaitForSeconds (1);
		}
		while (!gameTracker.playerCar.GetComponent<PlayerCarController> ().hasFinished) {
			yield return new WaitForSeconds (1);
		}
		//Determine scores and send to final screen
		yield return new WaitForSeconds (1);
		//scoreKeeper.outputTextResults ();
		chat.ChatUI.SetActive(false);
		raceTimer.gameObject.SetActive (false);
		raceStatusText.gameObject.SetActive (false);
		createEndResults ();
		yield return new WaitForSeconds (.5f);
		endGameCanvas.SetActive (true);

		yield return null;
	}

	//To be executed if 1 or more players did not successfully submit before time up
	private IEnumerator timeOut() {
		yield return new WaitForSeconds (5);
		quitButton.OnClick ();
	}

	private IEnumerator submissionWait()
	{
		yield return new WaitForSeconds(3);
	}

	private void updateBuildTimer() {
		float timerTime = buildTimeLimit - (Time.time - startTime);
		int minutes = (int)timerTime / 60;
		int seconds = (int)timerTime % 60;
		buildTimer.text = string.Format ("Remaining: {0:00}:{1:00}", minutes, seconds);
	}

	private void createEndResults() {
		//Left column
		bTitle.text = scoreKeeper.getOpponentName () + "'s Track";
		bTimeTitle.text = scoreKeeper.getPlayerName () + "'s Time:";
		bTime.text = scoreKeeper.getSelfRaceTime ().ToString() + "s";
		aTestTimeTitle.text = scoreKeeper.getOpponentName ().ToString() + "'s Test Time:";
		aTestTime.text = scoreKeeper.getOpponentTestTime ().ToString() + "s";
		bColumnDiff.text = (scoreKeeper.getSelfRaceTime () - scoreKeeper.getOpponentTestTime ()).ToString();
		if (scoreKeeper.getSelfRaceTime () - scoreKeeper.getOpponentTestTime () >= 0) {
			bColumnDiff.color = Color.red;
		}

		//Right column
		aTitle.text = scoreKeeper.getPlayerName () + "'s Track";
		aTimeTitle.text = scoreKeeper.getOpponentName () + "'s Time:";
		aTime.text = scoreKeeper.getOpponentRaceTime ().ToString() + "s";
		bTestTimeTitle.text = scoreKeeper.getPlayerName ().ToString() + "'s Test Time:";
		bTestTime.text = scoreKeeper.getSelfTestTime().ToString() + "s";
		aColumnDiff.text = (scoreKeeper.getOpponentRaceTime () - scoreKeeper.getSelfTestTime ()).ToString();
		if (scoreKeeper.getOpponentRaceTime () - scoreKeeper.getSelfTestTime () >= 0) {
			bColumnDiff.color = Color.red;
		}

        float selfTime = (float)(scoreKeeper.getSelfRaceTime() - scoreKeeper.getOpponentTestTime())

        float opponentTime = (float) (scoreKeeper.getOpponentRaceTime() - scoreKeeper.getSelfTestTime())

        int result = 0;
        float score = 0.0f;

        if (selfTime > opponentTime)
        {
            winner.text = scoreKeeper.getOpponentName() + " wins!";
            result = -1;
        }
        else if (selfTime < opponentTime)
        {
            winner.text = scoreKeeper.getPlayerName() + " wins!";
            result = 1;  
        }
        else {
            winner.text = "Draw!";
            result = 0;
        }
        score = selfTime - opponentTime;
        StartCoroutine(forwardPlayerStats(result, score));
    }

    private IEnumerator forwardPlayerStats(int result, float score)
    {
        string name = accInfo.getName();
        string post_url = "http://proj-309-38.cs.iastate.edu/php/updatestats.php?" + "username=" + WWW.EscapeURL(name) + "&result=" + score + "&score=" + score;
        WWW f_check = new WWW(post_url);
        yield return f_check;
    }
}
