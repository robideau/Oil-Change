/* David Robideau
 * 2/9/2016
 * 
 * This script handles the spinning animations of the player car's wheel models.
 * May be adapted in the future to work with ghost data, but this is significantly more complicated.
 *
 * Ensure that all wheels HAVE NO COLLIDERS OR RIGIDBODIES ATTACHED. This will lead to hilarious physics disasters.
 *
 * Last update - 2/9/2016
 */

using UnityEngine;
using System.Collections;

public class WheelAnimator : MonoBehaviour {

	//Wheel model references
	public GameObject frontLeft;
	public GameObject frontRight;
	public GameObject rearLeft;
	public GameObject rearRight;
	private GameObject[] allWheels;

	//External script references
	public PlayerCarController playerCarController;
	private GameObject[] wheelColliderObjects;

	void Start () {
		allWheels = new GameObject[4] { frontLeft, frontRight, rearLeft, rearRight };
		wheelColliderObjects = playerCarController.getAllWheels ();
	}

	void Update () {

		//Set each wheel's rotation equal to the corresponding wheel collider's
		for (int i = 0; i < 4; i++) {
			Vector3 colliderPosition;
			Quaternion colliderRotation;
			wheelColliderObjects [i].GetComponent<WheelCollider> ().GetWorldPose (out colliderPosition, out colliderRotation); //set values for rotation vars
			allWheels [i].transform.rotation = colliderRotation;
			allWheels [i].transform.Rotate(new Vector3(0, 0, 90)); //Correct for quaternion error w/ local rotation
		}

	}
}
