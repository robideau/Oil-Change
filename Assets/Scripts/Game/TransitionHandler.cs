/* David Robideau
 * 3/22/2016
 * 
 * This script handles transitions between build mode and race mode.
 * (De)activates components as necessary, detects transition criteria, and handles data transfers.
 *
 * Last update - 3/22/2016
 */
using UnityEngine;
using System.Collections;

public class TransitionHandler : MonoBehaviour {

	//Empties - hold objects exclusive to either build or race mode for easy deactivation
	public GameObject buildModeComponents;
	public GameObject raceModeComponents;

	//Network manager and data to handle initial connection
	public NetManager netManager;
	public GameObject connectionData;

	void Awake() {
		//TODO - retrieve network data, create connection

		buildMode ();
	}

	private void buildMode() {



		//Deactivate build mode components

	}

	private void raceMode() {
		//Relocate car to start point

		//Replicate track

		//Reset timer

		//Collapse chat (if necessary)

	}
}
