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

	private bool submitConditionsMet;

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

	public IEnumerator checkSubmitConditions() {
		StartCoroutine (scanner.QuickProcessTrack ());
		yield return new WaitForSeconds (2);
		if (scanner.requiredPiecesPlaced()) {
			print ("Required pieces have been placed.");
		} else {

		}
	}
}
