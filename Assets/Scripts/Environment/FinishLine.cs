﻿/* David Robideau
 * 3/1/2016
 * 
 * This script handles all collisions between the car and the environment.
 * Used to detect finish line, hazards, etc.
 *
 * Last update - 3/1/2016
 */

using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {

	public PlayerCarController playerCarController;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {	//If we've crossed the finish line
			playerCarController.hasFinished = true;
		}
	}
}