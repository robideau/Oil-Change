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
}
