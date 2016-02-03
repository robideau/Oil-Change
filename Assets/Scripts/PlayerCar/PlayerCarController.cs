/* David Robideau
 * 2/3/2016
 * 
 * This controller handles player input and the player car's motion and physics.
 *
 */

using UnityEngine;
using System.Collections;

public class PlayerCarController : MonoBehaviour {

	public GameObject carBody;
	public GameObject carWheels;

	void Start () {
		
	}

	void Update () {

		if (Input.GetKeyDown ("w")) {
			foreach (Transform wheel in carWheels.transform) {
				//Rotate wheel forward
			}
		}

		if (Input.GetKeyDown ("s")) {
			foreach (Transform wheel in carWheels.transform) {
				//Rotate wheel backwards
			}
		}

		if (Input.GetKeyDown ("d")) {
			foreach (Transform wheel in carWheels.transform) {
				//Isolate front wheels - revise later to use tags?
				if (wheel.gameObject.name.Contains ("Front")) {
					//Turn front wheels right
				}
			}
		}

		if (Input.GetKeyDown ("d")) {
			foreach (Transform wheel in carWheels.transform) {
				//Isolate front wheels - revise later to use tags?
				if (wheel.gameObject.name.Contains ("Front")) {
					//Turn front wheels left
				}
			}
		}

	}
}
