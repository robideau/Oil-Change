/* David Robideau
 * 3/24/2016
 * 
 * This script acts as a game tracker for build mode. Used to inform TransitionHandler when certain build events have occured.
 *
 * Last update - 3/24/2016
 */
using UnityEngine;
using System.Collections;

public class BuildTracker : MonoBehaviour {

	public TrackScanner scanner;
	public TransitionHandler transitionHandler;
	public ObjectController objectController;

	private bool requiredPiecesPlaced = false;
	private bool newObjectsPlaced = false;
	private bool submitConditionsMet = false;

	void Start () {
	
	}

	void Update () {
		//DEBUG
		if (Input.GetKeyDown ("y")) {
			scanner.cleanObjectNames ();
			print (scanner.requiredPiecesPlaced ());
		}
		if (Input.GetKeyDown ("u")) {
			StartCoroutine (scanner.QuickProcessTrack ());
		}
	}

	public IEnumerator submitConditionWrapper() {
		StartCoroutine (processSubmitConditions ());
		yield return new WaitForSeconds (2);

		if (submitConditionsCheck ()) {
			//proceed
		} else {
			//display error and do nothing
		}
	}

	public IEnumerator processSubmitConditions() {
		StartCoroutine (scanner.QuickProcessTrack ());
		yield return new WaitForSeconds (2);
		if (scanner.requiredPiecesPlaced()) {
			print ("Required pieces have been placed.");
		} else {
			print ("Require pieces missing.");
			requiredPiecesPlaced = false;
			yield return null;
		}
		// if (object controller has placed new object since last test)

		if (requiredPiecesPlaced && !newObjectsPlaced) {
			submitConditionsMet = true;
		}
	}

	public bool submitConditionsCheck() {
		return submitConditionsMet;
	}

	public void onSubmitClick() {
		StartCoroutine (submitConditionWrapper());
	}
}
