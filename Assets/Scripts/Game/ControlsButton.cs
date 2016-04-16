/* David Robideau
 * 4/16/2016
 * 
 * This script describes the functionality of the quit button and handles all necessary database operations.
 *
 * Last update - 4/16/2016
 */

using UnityEngine;
using System.Collections;

public class ControlsButton : MonoBehaviour {

	public GameObject buildControls;
	public GameObject raceControls;
	public TransitionHandler handler;

	
	public void OnControlsButtonClick() {
		if (handler.buildModeActive) {
			buildControls.SetActive (!buildControls.activeSelf);
		} else {
			buildControls.SetActive (false);
			raceControls.SetActive (!raceControls.activeSelf);
		}
	}

	public void forceCloseBuildControls() {
		buildControls.SetActive (false);
	}

	public void forceCloseRaceControls() {
		raceControls.SetActive (false);
	}
}
