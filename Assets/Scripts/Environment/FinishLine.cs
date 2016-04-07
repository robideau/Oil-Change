/* David Robideau
 * 3/1/2016
 * 
 * This script handles all collisions between the car and the environment.
 * Used to detect finish line, hazards, etc.
 *
 * Last update - 4/7/2016
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishLine : MonoBehaviour {

	public PlayerCarController playerCarController;
	public ScoreKeeper scoreKeeper;
	public TransitionHandler transitionHandler;

	void Start() {
		playerCarController = GameObject.FindWithTag ("Player").transform.GetChild(0).GetComponent<PlayerCarController>();
		scoreKeeper = GameObject.Find ("ScoreKeeper").GetComponent<ScoreKeeper>();
		transitionHandler = GameObject.Find ("GameController").GetComponent<TransitionHandler>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {	//If we've crossed the finish line
			playerCarController.hasFinished = true;
		}
		if (transitionHandler.buildTimerActive && !scoreKeeper.playerTestTimeSet) {
			scoreKeeper.finishRecordingTestTime ();
		} else if (transitionHandler.raceTimerActive && !scoreKeeper.playerRaceTimeSet) {
			scoreKeeper.finishRecordingRaceTime ();
		}
	}
}
