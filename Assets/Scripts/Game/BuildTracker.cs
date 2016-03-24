/* David Robideau
 * 3/24/2016
 * 
 * This script acts as a game tracker for build mode. Used to inform TransitionHandler when certain build events have occured.
 *
 * Last update - 3/24/2016
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildTracker : MonoBehaviour {

	public PlayerCarController playerCar;
	public Text submissionStatusText;
	public Text buildStatus;
	public TrackScanner scanner;
	public TransitionHandler transitionHandler;
	public ObjectController objectController;
	public GameObject buildMenuPanel;
	public Button testButton;
	public Button submitButton;
	public GameTracker gameTracker;
	private NetworkView nv;

	private bool requiredPiecesPlaced = false;
	private bool newObjectsPlaced = false;
	private bool successfulRun = false;
	private bool submitConditionsMet = false;

	void Start () {
		nv = GetComponent<NetworkView> ();
	}

	void Update () {
		
	}

	public IEnumerator submitConditionWrapper() {
		StartCoroutine (processSubmitConditions ());
		yield return new WaitForSeconds (3);
	}

	public IEnumerator processSubmitConditions() {
		StartCoroutine (scanner.QuickProcessTrack ());
		//Wait for scan to complete
		yield return new WaitForSeconds (2);

		//Check required pieces
		if (scanner.requiredPiecesPlaced()) {
			submissionStatusText.text = "Required pieces placed.";
			requiredPiecesPlaced = true;
		} else {
			submissionStatusText.text = "Required piece(s) missing.";
			requiredPiecesPlaced = false;
			toggleBuildMenu (true);
			yield break;
		}

		//Check if object controller has placed a new object since the last test
		if (!objectController.isNewPiecesPlaced ()) {
			submissionStatusText.text = "No new pieces placed.";
			newObjectsPlaced = false;
		} else {
			submissionStatusText.text = "New pieces placed since last successful run.";
			newObjectsPlaced = true;
			toggleBuildMenu (true);
			yield break;
		}
			
		// if track has not yet been completed
		if (playerCar.hasFinished) {
			submissionStatusText.text = "Last run successful.";
			successfulRun = true;
		} else {
			submissionStatusText.text = "Current course has not successfully been run.";
			successfulRun = false;
			toggleBuildMenu (true);
			yield break;
		}
			
		if (requiredPiecesPlaced && !newObjectsPlaced && successfulRun) {
			submitConditionsMet = true;
			submissionStatusText.text = "Submission accepted!";
			toggleBuildMenu (false);
			nv.RPC ("updateBuildStatus", RPCMode.Others, null);
		}
			
	}

	public bool submitConditionsCheck() {
		return submitConditionsMet;
	}

	public void onSubmitClick() {
		//Show submission status
		submissionStatusText.gameObject.SetActive (true);

		//Deactivate other build options
		toggleBuildMenu(false);
		submissionStatusText.text = "Submitting...";

		//Validate submission
		StartCoroutine (submitConditionWrapper());
	}

	public void toggleBuildMenu(bool active) {
		buildMenuPanel.SetActive (active);
		testButton.gameObject.SetActive (active);
		submitButton.gameObject.SetActive (active);
	}
		
	[RPC] void updateBuildStatus() {
		buildStatus.text = "Opponent has finished building.";
	}
}
