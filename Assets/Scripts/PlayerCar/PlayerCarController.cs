/* David Robideau
 * 2/3/2016
 * 
 * This controller handles player input and the player car's motion and physics.
 *
 * Last update - 2/5/2016
 */

using UnityEngine;
using System.Collections;

public class PlayerCarController : MonoBehaviour {

	//References to car components
	public GameObject carBody;
	public GameObject carBase;
	public GameObject frontLeftWheel;
	public GameObject frontRightWheel;
	public GameObject rearLeftWheel;
	public GameObject rearRightWheel;
	private Axle[] axles = new Axle[2];

	//Car control options
	public bool frontWheelDrive;

	//Speed and turn resistance paramteters
	public float steerIncrements;
	public float maxTurnAngle;
	public float maxForwardTorque;
	public float maxBackwardTorque;
	public float maxBrakeTorque;

	//Simplified wheel references for use only in script
	private GameObject relativeLeftWheel;
	private GameObject relativeRightWheel;

	void Start () {

		//Initialize axles, determine front wheel/rear wheel drive
		Axle frontAxle = new Axle ();
		frontAxle.init (frontLeftWheel, frontRightWheel, frontWheelDrive);
		Axle rearAxle = new Axle ();
		rearAxle.init (rearLeftWheel, rearRightWheel, !frontWheelDrive);
		axles.SetValue (frontAxle, 0);
		axles.SetValue (rearAxle, 1);

		foreach (Axle axle in axles) {
			if (axle.isDriveAxis ()) {
				relativeLeftWheel = axle.getLeftWheel ();
				relativeRightWheel = axle.getRightWheel ();
			}
		}
			
	}

	void Update () {

		//W pressed but not S
		if (Input.GetKey ("w") && !Input.GetKey ("s")) {
			//While torque value is below maximum, apply more (incr. acceleration)
			if (relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque < maxForwardTorque) {
				relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque = maxForwardTorque * Input.GetAxis ("Vertical") * 2;
				relativeRightWheel.GetComponent<WheelCollider> ().motorTorque = maxForwardTorque * Input.GetAxis ("Vertical") * 2;
			}
		}

		if (Input.GetKey ("s") && !Input.GetKey ("w")) {
			//While torque value is above negative maximum, apply more (inr. negative acceleration)
			if (relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque > -maxBackwardTorque) {
				relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque += maxBackwardTorque * Input.GetAxis("Vertical") * 2;
				relativeRightWheel.GetComponent<WheelCollider> ().motorTorque += maxBackwardTorque * Input.GetAxis("Vertical") * 2;
			}
		}

		//Right turn
		if (Input.GetKey ("d")) {
			if (relativeLeftWheel.GetComponent<WheelCollider> ().steerAngle < maxTurnAngle) {
				relativeLeftWheel.GetComponent<WheelCollider> ().steerAngle += steerIncrements;
				relativeRightWheel.GetComponent<WheelCollider> ().steerAngle += steerIncrements;
			}
		}

		//Left turn
		if (Input.GetKey ("a")) {
			if (relativeRightWheel.GetComponent<WheelCollider> ().steerAngle > -maxTurnAngle) {
				relativeLeftWheel.GetComponent<WheelCollider> ().steerAngle -= steerIncrements;
				relativeRightWheel.GetComponent<WheelCollider> ().steerAngle -= steerIncrements;
			}
		}

		//If no acceleration input, decelerate
		if (!Input.GetKey ("w") && !Input.GetKey ("s")) {
			relativeLeftWheel.GetComponent<WheelCollider> ().brakeTorque = maxBrakeTorque;
			relativeRightWheel.GetComponent<WheelCollider> ().brakeTorque = maxBrakeTorque;
			if (relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque > 0) {
				relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque -= maxForwardTorque * 2;
				relativeRightWheel.GetComponent<WheelCollider> ().motorTorque -= maxForwardTorque * 2;
				if (relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque < 0) {
					relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque = 0;
					relativeRightWheel.GetComponent<WheelCollider> ().motorTorque = 0;
				}
			}
			if (relativeRightWheel.GetComponent<WheelCollider> ().motorTorque < 0) {
				relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque += maxForwardTorque * 2;
				relativeRightWheel.GetComponent<WheelCollider> ().motorTorque += maxForwardTorque * 2;
				if (relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque > 0) {
					relativeLeftWheel.GetComponent<WheelCollider> ().motorTorque = 0;
					relativeRightWheel.GetComponent<WheelCollider> ().motorTorque = 0;
				}
			}
		} else {
			relativeLeftWheel.GetComponent<WheelCollider> ().brakeTorque = 0;
			relativeRightWheel.GetComponent<WheelCollider> ().brakeTorque = 0;
		}

		//If no turn being made, maintain steering angle
		if (!Input.GetKey ("a") && !Input.GetKey ("d")) {
			relativeLeftWheel.GetComponent<WheelCollider> ().steerAngle = 0;
			relativeRightWheel.GetComponent<WheelCollider> ().steerAngle = 0;
		}

	}
		
}

//Holds data for each separate axle - front and back
//Includes wheel references as well as whether or not the axis has drive control
public class Axle {
	GameObject leftWheel;
	GameObject rightWheel;
	bool driveAxis;

	public void init(GameObject leftWheel, GameObject rightWheel, bool driveAxis) {
		this.leftWheel = leftWheel;
		this.rightWheel = rightWheel;
		this.driveAxis = driveAxis;
	}

	public bool isDriveAxis() {
		return driveAxis;
	}
				
	public GameObject getLeftWheel() {
		return leftWheel;
	}

	public GameObject getRightWheel() {
		return rightWheel;
	}
}
