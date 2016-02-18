﻿/* David Robideau
 * 2/3/2016
 * 
 * This controller handles player input and the player car's motion and physics.
 *
 * Last update - 2/18/2016
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
	public GameObject cameraRig;

	//References to other scripts
	public GhostRecorder ghostRecorder;

	//Car control options
	public bool frontWheelDrive; 
	public bool shortResetEnabled;
	public bool fullResetEnabled;
	public bool cameraControlEnabled;
	public int shortResetFrequency;
	private bool movementEnabled;
	private bool ghostReplaying;
	private int previousShortReset = 0;
	private int activeCamera = 0;

	//Speed and turn resistance paramteters
	public float steerIncrements;
	public float maxTurnAngle;
	public float maxForwardTorque;
	public float maxBackwardTorque;
	public float maxBrakeTorque;

	//Simplified wheel references for use only in script
	private GameObject relativeLeftWheel;
	private GameObject relativeRightWheel;
	private WheelCollider rightCollider;
	private WheelCollider leftCollider;


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
				leftCollider = relativeLeftWheel.GetComponent<WheelCollider> ();
				rightCollider = relativeRightWheel.GetComponent<WheelCollider> ();
			}
		}

		movementEnabled = true;
			
	}

	void Update () {

		/* PLAYER CONTROL HANDLING */

		if (movementEnabled) {

			//Accelerate
			if (Input.GetKey ("w") && !Input.GetKey ("s")) {
				//While torque value is below maximum, apply more (incr. acceleration)
				if (leftCollider.motorTorque < maxForwardTorque) {
					leftCollider.motorTorque = maxForwardTorque * Input.GetAxis ("Vertical") * 2;
					rightCollider.motorTorque = maxForwardTorque * Input.GetAxis ("Vertical") * 2;
				}
			}

			//Brake, reverse acceleration
			if (Input.GetKey ("s") && !Input.GetKey ("w")) {
				//While torque value is above negative maximum, apply more (inr. negative acceleration)
				if (leftCollider.motorTorque > -maxBackwardTorque) {
					leftCollider.motorTorque += maxBackwardTorque * Input.GetAxis ("Vertical") * 2;
					rightCollider.motorTorque += maxBackwardTorque * Input.GetAxis ("Vertical") * 2;
				}
			}

			//Right turn
			if (Input.GetKey ("d")) {
				if (leftCollider.steerAngle < maxTurnAngle) {
					leftCollider.steerAngle += steerIncrements;
					rightCollider.steerAngle += steerIncrements;
				}
			}

			//Left turn
			if (Input.GetKey ("a")) {
				if (rightCollider.steerAngle > -maxTurnAngle) {
					leftCollider.steerAngle -= steerIncrements;
					rightCollider.steerAngle -= steerIncrements;
				}
			}

			//If no acceleration input, decelerate
			if (!Input.GetKey ("w") && !Input.GetKey ("s")) {
				leftCollider.brakeTorque = maxBrakeTorque;
				rightCollider.brakeTorque = maxBrakeTorque;
				if (leftCollider.motorTorque > 0) {
					leftCollider.motorTorque -= maxForwardTorque * 2;
					rightCollider.motorTorque -= maxForwardTorque * 2;
					if (leftCollider.motorTorque < 0) {
						leftCollider.motorTorque = 0;
						rightCollider.motorTorque = 0;
					}
				}
				if (rightCollider.motorTorque < 0) {
					leftCollider.motorTorque += maxForwardTorque * 2;
					rightCollider.motorTorque += maxForwardTorque * 2;
					if (leftCollider.motorTorque > 0) {
						leftCollider.motorTorque = 0;
						rightCollider.motorTorque = 0;
					}
				}
			} else {
				leftCollider.brakeTorque = 0;
				rightCollider.brakeTorque = 0;
			}

			//If no turn being made, maintain steering angle
			if (!Input.GetKey ("a") && !Input.GetKey ("d")) {
				leftCollider.steerAngle = 0;
				rightCollider.steerAngle = 0;
			}

			//Short reset - reset 5 seconds if no reset has been performed in a certain amount of time
			if (Input.GetKeyDown ("f") && shortResetEnabled) {
				if (Time.frameCount - previousShortReset >= shortResetFrequency) {
					carBase.transform.position = ghostRecorder.getFramePosition (ghostRecorder.getRecordingCount () - (1000 * ghostRecorder.recordingIntervals));
					carBase.transform.rotation = ghostRecorder.getFrameRotation (ghostRecorder.getRecordingCount () - (1000 * ghostRecorder.recordingIntervals));
					carBase.GetComponent<Rigidbody> ().velocity = new Vector3(0, 0, 0);
					previousShortReset = Time.frameCount;
				}
			}

			//Full reset - reset to initial position and rotation
			if (Input.GetKeyDown ("r") && fullResetEnabled) {
				fullReset();
			}
		
		}

		//Temporary control - stop player car movement and replay ghost data
		if (!ghostRecorder.getIsReplaying()) {
			if (Input.GetKeyDown ("g")) {
				movementEnabled = false;
				ghostRecorder.setIsRecording (false);
				ghostRecorder.replayGhost ();
			}
		}

		//Camera control - switch between multiple perspectives
		if (cameraControlEnabled && Input.GetKeyDown("c")) {
			if (activeCamera < cameraRig.transform.childCount-1) {
				activeCamera++;
				cameraRig.transform.GetChild (activeCamera - 1).gameObject.SetActive (false); //turn off old cam
				cameraRig.transform.GetChild (activeCamera).gameObject.SetActive (true); //turn on new cam
			} else {
				cameraRig.transform.GetChild (activeCamera).gameObject.SetActive (false); //turn off old cam
				activeCamera = 0;
				cameraRig.transform.GetChild (activeCamera).gameObject.SetActive (true); //turn on default cam
			}
		}

	}

	public float getMotorTorque() {
		return leftCollider.motorTorque;
	}

	public float getSteerAngle() {
		return leftCollider.steerAngle;
	}

	public GameObject[] getAllWheels() {
		return new GameObject[4] { frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel };
	}

	public void fullReset() {
		carBase.transform.position = ghostRecorder.getFramePosition (0);
		carBase.transform.rotation = ghostRecorder.getFrameRotation (0);
		carBase.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		carBase.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 0, 0);
		leftCollider.motorTorque = 0;
		rightCollider.motorTorque = 0;
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
